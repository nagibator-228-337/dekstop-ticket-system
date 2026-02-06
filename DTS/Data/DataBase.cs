using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DTS.Data
{
    public class DataBase
    {
        private readonly string _dbPath;
        public DataBase()
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            _dbPath = Path.Combine(folder, "DTS.db");
            InitDataBase();
            AddAdmin();
        }

        private string ComputeHash(string password) //For storing and handling the password hash instead of the plain password
        {
            using SHA256 sha = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha.ComputeHash(bytes);

            return Convert.ToHexString(hash);
        }

        public void InitDataBase()
        {

            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                CREATE TABLE IF NOT EXISTS Tickets (
                    Id INTEGER PRIMARY KEY,
                    Subject TEXT NOT NULL,
                    Description TEXT,
                    CreatedAt TEXT NOT NULL,
                    Status TEXT,
                    AssignedEmployeeId INTEGER,
                    AccessCode TEXT,
                    ClientContact TEXT
                    );

                CREATE TABLE IF NOT EXISTS Employees(
                    Id INTEGER PRIMARY KEY,
                    Login TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    Role TEXT DEFAULT 'Default',
                    FullName TEXT
                );
                
                CREATE TABLE IF NOT EXISTS Messages (
                    Id INTEGER PRIMARY KEY,
                    TicketId INTEGER NOT NULL,
                    AuthorType INTEGER NOT NULL,
                    AuthorId INTEGER NOT NULL,
                    SentAt TEXT NOT NULL,
                    Text TEXT NOT NULL
                );
                 ";


                command.ExecuteNonQuery();
            }
        }

        public void AddTicket(Ticket ticket)
        {
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                INSERT INTO Tickets
                   (Subject, Description, CreatedAt, Status, AssignedEmployeeId, AccessCode, ClientContact)
                   VALUES
                    (@subject, @description, @createdAt, @status, @assignedEmployeeId, @accessCode, @clientContact)
                ";
                command.Parameters.AddWithValue("@subject", ticket.Subject ?? "");
                command.Parameters.AddWithValue("@description", ticket.Description ?? "");
                command.Parameters.AddWithValue("@createdAt", ticket.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@status", ticket.Status.ToString());
                // correct null or id (if we have)
                command.Parameters.AddWithValue("@assignedEmployeeId", ticket.AssignedEmployeeId.HasValue ? (object)ticket.AssignedEmployeeId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@accessCode", ticket.AccessCode ?? "");
                command.Parameters.AddWithValue("@clientContact", ticket.ClientContact ?? "");

                command.ExecuteNonQuery();
            }
        }

        public void AddAdmin()
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"
                INSERT OR IGNORE INTO Employees (Login, PasswordHash, FullName, Role)
                VALUES (@login, @passwordHash, @FullName, @Role)";

            command.Parameters.AddWithValue("@login", "admin");
            command.Parameters.AddWithValue("@passwordHash", ComputeHash("123"));
            command.Parameters.AddWithValue("@FullName", "Admin");
            command.Parameters.AddWithValue("@Role", "Admin");

            command.ExecuteNonQuery();
        }

        public bool ValidateLogin(string login, string password, out string fullName, out string role, out int id)
        {
            fullName = string.Empty;
            role = string.Empty;
            id = 0;

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"
                SELECT PasswordHash, FullName, Role, Id
                FROM Employees
                WHERE Login = @login
                LIMIT 1";
            command.Parameters.AddWithValue("@login", login);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                string hashFromDb = reader.GetString(0);
                string enteredHash = ComputeHash(password);

                if (hashFromDb == enteredHash)
                {
                    fullName = reader.GetString(1);
                    role = reader.GetString(2);
                    id = reader.GetInt32(3);
                    return true;
                }
            }

            return false; //wrong login or pass
        }

        public ObservableCollection<Ticket> GetAllTickets()
        {
            var tickets = new ObservableCollection<Ticket>();
            // getting employee for connection id->employee 
            var employeesById = GetAllEmployees().ToDictionary(e => e.Id);

            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Tickets";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int? assignedId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5);

                        Ticket ticket = new Ticket
                        {
                            Id = reader.GetInt32(0),
                            Subject = reader.GetString(1),
                            Description = reader.GetString(2),
                            CreatedAt = DateTime.Parse(reader.GetString(3)),
                            Status = Enum.Parse<Ticket.TicketStatus>(reader.GetString(4)),
                            AssignedEmployeeId = assignedId,
                            AssignedEmployee = (assignedId.HasValue && employeesById.TryGetValue(assignedId.Value, out var emp)) ? emp : null,
                            AccessCode = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            ClientContact = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        };

                        tickets.Add(ticket);
                    }
                }
            }
            return tickets;
        }

        public ObservableCollection<Employee> GetAllEmployees()
        {
            var employees = new ObservableCollection<Employee>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Login, PasswordHash, Role, FullName FROM Employees";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var login = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                var passwordHash = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                var roleString = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                var fullName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);

                var employee = new Employee
                {
                    Id = id,
                    Login = login,
                    PasswordHash = passwordHash,
                    FullName = fullName
                };
                employees.Add(employee);
            }

            return employees;
        }

        public Employee? GetEmployeeById(int id)
        {
            return GetAllEmployees().FirstOrDefault(e => e.Id == id);
        }

        public ObservableCollection<Ticket> GetTicketsByEmployee(int employeeId)
        {
            var tickets = new ObservableCollection<Ticket>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"
                SELECT *
                FROM Tickets
                WHERE AssignedEmployeeId = @employeeId
                ";
            command.Parameters.AddWithValue("@employeeId", employeeId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int? assignedId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5);

                tickets.Add(new Ticket
                {
                    Id = reader.GetInt32(0),
                    Subject = reader.GetString(1),
                    Description = reader.GetString(2),
                    CreatedAt = DateTime.Parse(reader.GetString(3)),
                    Status = Enum.Parse<Ticket.TicketStatus>(reader.GetString(4)),
                    AssignedEmployeeId = assignedId,
                    AssignedEmployee = assignedId.HasValue ? GetEmployeeById(assignedId.Value) : null,
                    AccessCode = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    ClientContact = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                });
            }

            return tickets;
        }

        public Ticket? GetTicketByCode(string code)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"
                SELECT *
                FROM Tickets
                WHERE AccessCode = @code
                LIMIT 1
                ";

            command.Parameters.AddWithValue("@code", code);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                int? assignedId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5);

                return new Ticket
                {
                    Id = reader.GetInt32(0),
                    Subject = reader.GetString(1),
                    Description = reader.GetString(2),
                    CreatedAt = DateTime.Parse(reader.GetString(3)),
                    Status = Enum.Parse<Ticket.TicketStatus>(reader.GetString(4)),
                    AssignedEmployeeId = assignedId,
                    AssignedEmployee = assignedId.HasValue ? GetEmployeeById(assignedId.Value) : null,
                    AccessCode = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    ClientContact = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                };
            }

            return null;
        }

        public void UpdateAssignedEmployee(Ticket ticket, int employeeId)
        {
            UpdateAssignedEmployee(ticket, (int?)employeeId);
        }

        // null method allows passing null to unassign employee
        public void UpdateAssignedEmployee(Ticket ticket, int? employeeId)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"
                UPDATE Tickets
                SET AssignedEmployeeId = @employeeId
                WHERE Id = @ticketId
                ";

            if (employeeId.HasValue)
                command.Parameters.AddWithValue("@employeeId", employeeId.Value);
            else
                command.Parameters.AddWithValue("@employeeId", DBNull.Value);

            command.Parameters.AddWithValue("@ticketId", ticket.Id);

            Debug.WriteLine($"UpdateAssignedEmployee: ticket.Id={ticket.Id}, employeeId={(employeeId.HasValue ? employeeId.Value.ToString() : "NULL")}");

            int affected = command.ExecuteNonQuery();
            Debug.WriteLine($"UpdateAssignedEmployee: rowsAffected={affected}");

            // quick verification: read value from DB
            if (ticket.Id > 0)
            {
                using var verifyCmd = connection.CreateCommand();
                verifyCmd.CommandText = "SELECT AssignedEmployeeId FROM Tickets WHERE Id = @ticketId LIMIT 1";
                verifyCmd.Parameters.AddWithValue("@ticketId", ticket.Id);
                var dbValue = verifyCmd.ExecuteScalar();
                Debug.WriteLine($"UpdateAssignedEmployee VERIFY: ticket.Id={ticket.Id}, DB.AssignedEmployeeId={dbValue}");
            }
            else
            {
                Debug.WriteLine("UpdateAssignedEmployee: ticket.Id == 0 (no verification by Id)");
            }

            // updating objects in memory
            ticket.AssignedEmployeeId = employeeId;
            ticket.AssignedEmployee = employeeId.HasValue ? GetEmployeeById(employeeId.Value) : null;
        }
    }
}


