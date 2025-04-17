using DocuTrack.DataModels;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using Tmds.DBus.Protocol;

namespace DocuTrack.Data
{

    public static class DatabaseHelper
    {
        private static readonly string ConnectionString =
#if DEBUG
            // Use the project directory for Debug builds
            $"Data Source=Data/identifier.sqlite";
#else
    // Use AppContext.BaseDirectory for Release builds
    $"Data Source={AppContext.BaseDirectory}Data/identifier.sqlite";
#endif

        public static SqliteConnection GetConnection()
        {
            var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        public static bool Authenticate(string Username, string Password)
        {
            try
            {
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(1) FROM Accounts WHERE Username = @username AND Password = @password";
                command.Parameters.AddWithValue("@username", Username);
                command.Parameters.AddWithValue("@password", Password);

                var result = (long)command.ExecuteScalar();
                if (result == 1) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user", ex);
            }
        }

        internal static List<Sailor> GetSailors()
        {
            var sailors = new List<Sailor>();
            try
            {
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT GID, Name, Surname, IsRefresh, ID FROM Sailors";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var sailor = new Sailor
                    {
                        GID = reader.GetString(0), // GID
                        Name = reader.GetString(1), // Name
                        Surname = reader.GetString(2), // Surname
                        IsRefresh = reader.GetBoolean(3), // IsRefresh
                        ID = reader.GetInt32(4) // IsRefresh
                    };

                    sailors.Add(sailor);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving sailors from the database", ex);
            }

            return sailors;
        }
    }
    
}
