using DocuTrack.DataModels;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
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

        #region sailors
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
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving sailors from the database", ex);
            }

            return sailors;
        }

        internal static void updateSailor(Sailor sailor)
        {
            try
            {
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE Sailors SET Name = @name, Surname = @surname, GID = @gid, IsRefresh = @isRefresh WHERE ID = @id";
                command.Parameters.AddWithValue("@name", sailor.Name);
                command.Parameters.AddWithValue("@surname", sailor.Surname);
                command.Parameters.AddWithValue("@isRefresh", sailor.IsRefresh);
                command.Parameters.AddWithValue("@gid", sailor.GID);
                command.Parameters.AddWithValue("@id", sailor.ID);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating sailor in the database", ex);
            }
        }

        internal static Sailor addSailor(Sailor sailor)
        {
            try
            {
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Sailors (GID, Name, Surname, IsRefresh) VALUES (@gid, @name, @surname, @isRefresh)";
                command.Parameters.AddWithValue("@gid", sailor.GID);
                command.Parameters.AddWithValue("@name", sailor.Name);
                command.Parameters.AddWithValue("@surname", sailor.Surname);
                command.Parameters.AddWithValue("@isRefresh", sailor.IsRefresh);
                command.ExecuteNonQuery();

                var commandForGet = connection.CreateCommand();
                commandForGet.CommandText = "SELECT ID FROM Sailors WHERE GID =  @gid";
                command.Parameters.AddWithValue("@gid", sailor.GID);
                var reader = commandForGet.ExecuteReader();
                sailor.ID = reader.GetInt32(0); // ID
                connection.Close();
                return sailor;
            }
            catch (Exception ex)
            {
                throw new Exception("Greska pri dodavanju pomorca u bazu: "  + ex.Message, ex);
            }
        }

        internal static void deleteSailor(Sailor sailor)
        {
            try
            {
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Sailors WHERE ID = @id";
                command.Parameters.AddWithValue("@id", sailor.ID);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting sailor from the database", ex);
            }
        }

        #endregion

        #region CertificateTypes
        internal static List<CertificateType> geCertificateTypes()
        {
            var types = new List<CertificateType>();
            try
            {
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT ID, Name FROM CertificateTypes";
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var type = new CertificateType
                    {
                        ID = reader.GetInt32(0), // ID
                        Name = reader.GetString(1) // Name
                    };
                    types.Add(type);
                }
                connection.Close();


            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving certificate types from the database", ex);
            }

            return types;
        }
        internal static void updateCertificateType(CertificateType type)
        {
            try
            {
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE CertificateTypes SET Name = @name WHERE ID = @id";
                command.Parameters.AddWithValue("@name", type.Name);
                command.Parameters.AddWithValue("@id", type.ID);
                command.ExecuteNonQuery();
                connection.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error updating certificate type in the database", ex);
            }
        }
        internal static void addCertificateType(CertificateType type)
        {
            try
            {
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO CertificateTypes (Name) VALUES (@name)";
                command.Parameters.AddWithValue("@name", type.Name);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding certificate type to the database", ex);
            }
        }
        internal static void deleteCertificateType(CertificateType type)
        {
            try
            {
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM CertificateTypes WHERE ID = @id";
                command.Parameters.AddWithValue("@id", type.ID);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting certificate type from the database", ex);
            }
        }
        #endregion





    }

}
