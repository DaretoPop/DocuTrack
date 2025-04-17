using Microsoft.Data.Sqlite;
using System;
using Tmds.DBus.Protocol;

namespace DocuTrack.Data
{

    public static class DatabaseHelper
    {
        private static readonly string ConnectionString =
            $"Data Source={AppContext.BaseDirectory}Data/identifier.sqlite";

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
    }
    
}
