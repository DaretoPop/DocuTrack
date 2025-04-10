using Microsoft.Data.Sqlite;
using System;

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
    }
    
}
