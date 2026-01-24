using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
                command.Parameters.AddWithValue("@accessCode", ticket.AccessCode ??"");
                command.Parameters.AddWithValue("@clientContact", ticket.ClientContact ?? "");

                command.ExecuteNonQuery();
            }
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
