using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

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
                command.Parameters.AddWithValue("@assignedEmployeeId", ticket.AssignedEmployee?.Id ?? 0);
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
            command.CommandText = @"
                INSERT OR IGNORE INTO Employees (Login, PasswordHash, FullName, Role)
                VALUES (@login, @passwordHash, @FullName, @Role)";

            command.Parameters.AddWithValue("@login", "admin");
            command.Parameters.AddWithValue("@passwordHash", ComputeHash("123"));
            command.Parameters.AddWithValue("@FullName", "Admin");
            command.Parameters.AddWithValue("@Role", "Admin");

            command.ExecuteNonQuery();
        }

        public bool ValidateLogin(string login, string password, out string fullName, out string role)
        {
            fullName = string.Empty;
            role = string.Empty;

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT PasswordHash, FullName, Role
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
                    return true;
                }
            }

            return false; //login not found or wrong pass
        }


        public ObservableCollection<Ticket> GetAllTickets()
        {
            var tickets = new ObservableCollection<Ticket>();
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Tickets";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Ticket ticket = new Ticket
                        {
                            Id = reader.GetInt32(0),
                            Subject = reader.GetString(1),
                            Description = reader.GetString(2),
                            CreatedAt = DateTime.Parse(reader.GetString(3)),
                            Status = Enum.Parse<Ticket.TicketStatus>(reader.GetString(4)),
                            AssignedEmployee = null,
                            AccessCode = reader.GetString(6),
                            ClientContact = reader.GetString(7),
                        };

                        tickets.Add(ticket);
                    }
                }
            }
            return tickets;
        }
    }
}
