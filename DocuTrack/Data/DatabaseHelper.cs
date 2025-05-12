using DocumentFormat.OpenXml.Packaging;
using DocuTrack.DataModels;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        private static readonly string CertificateTypeDocumentsBaseFolder = $"Data/CertificateTypes";
        private static readonly string CertificateFilesBaseFolder = $"Data/Certificates";



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

                // Fix: Retrieve the last inserted row ID using SQLite's built-in function
                command.CommandText = "SELECT last_insert_rowid()";
                sailor.ID = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
                return sailor;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding sailor to the database: " + ex.Message, ex);
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
        internal static CertificateType addCertificateType(CertificateType type)
        {
            try
            {
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO CertificateTypes (Name) VALUES (@name)";
                command.Parameters.AddWithValue("@name", type.Name);
                command.ExecuteNonQuery();
                command.CommandText = "SELECT last_insert_rowid()";
                type.ID = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                return type;
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
                //DELETE ALL FILES IN THE FOLDER
                var directoryPath = $"{AppContext.BaseDirectory}/{CertificateTypeDocumentsBaseFolder}/{type.ID}";
                if (Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath, true);
                }
                //DELETE ALL FILES IN THE FOLDER
                var connection2 = GetConnection();
                var command2 = connection2.CreateCommand();
                command2.CommandText = "DELETE FROM RequestFiles WHERE CertificateTypeID = @id";
                command2.Parameters.AddWithValue("@id", type.ID);
                command2.ExecuteNonQuery();
                connection2.Close();
                //Delete all certificates for the people

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

        #region Certificate Types - Documents
        // File path : Data/CertificateTypes/ID/Type/FileName

        internal static string getCertificateTypeDocumentPath(RequestFile file)
        {
            return $"{AppContext.BaseDirectory}/{CertificateTypeDocumentsBaseFolder}/{file.CertificateTypeID}/{file.RequestType}/{file.FileName}";
        }

        internal static List<RequestFile> getCertificateTypeDocuments(int certificateTypeID)
        {
            var files = new List<RequestFile>();
            try
            {
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT ID, FilePath, CertificateTypeID, RequestType FROM RequestFiles WHERE CertificateTypeID = @certificateTypeID";
                command.Parameters.AddWithValue("@certificateTypeID", certificateTypeID);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var file = new RequestFile
                    {
                        ID = reader.GetInt32(0), // ID
                        FileName = reader.GetString(1), // FilePath

                        CertificateTypeID = reader.GetInt32(2), // CertificateTypeID
                        RequestType = (RequestTypeEnum)reader.GetInt32(3)
                    };
                    file.FilePath = getCertificateTypeDocumentPath(file);
                    files.Add(file);
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving certificate type documents from the database", ex);
            }
            return files;
        }

        internal static byte[] getCertificateTypeDocumentData(RequestFile file)
        {
            return null;
        }
        
        internal static void addCertificateTypeDocument(RequestFile file, byte[] fileData)
        {
            try
            {
                //create a directory if it doesnt exist
                var directoryPath = $"{AppContext.BaseDirectory}/{CertificateTypeDocumentsBaseFolder}/{file.CertificateTypeID}/{file.RequestType}";
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                //check if the file exists and change the name of the new one so it wont override
                var fileName = Path.GetFileName(file.FilePath);
                var filePath = Path.Combine(directoryPath, fileName);
                if (File.Exists(filePath))
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    var fileExtension = Path.GetExtension(fileName);
                    var newFileName = $"{fileNameWithoutExtension}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
                    file.FileName = newFileName;
                    file.FilePath = Path.Combine(directoryPath, newFileName);
                }
                else
                {
                    file.FilePath = filePath;
                }

                File.WriteAllBytes(file.FilePath, fileData);
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO RequestFiles (FilePath, RequestType, CertificateTypeID) VALUES (@filePath, @requestType, @certificateTypeID)";
                command.Parameters.AddWithValue("@filePath", file.FileName); //Save File Name insetad
                //Save RequestType as int
                command.Parameters.AddWithValue("@requestType", (int)file.RequestType);
                command.Parameters.AddWithValue("@certificateTypeID", file.CertificateTypeID);
                command.ExecuteNonQuery();
                connection.Close();
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding certificate type document to the database", ex);
            }
        }

        internal static void deleteCertificateTypeDocument(RequestFile file)
        {
            try
            {


                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM RequestFiles WHERE ID = @id";
                command.Parameters.AddWithValue("@id", file.ID);
                command.ExecuteNonQuery();
                connection.Close();

                //delete the file from the directory
                if (File.Exists(file.FilePath))
                {
                    File.Delete(file.FilePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting certificate type document from the database", ex);
            }
        }

        internal static void addCertificateTypeDocuments(List<RequestFile> documentsForAdd)
        {
            if (documentsForAdd.Count > 0)
            {
                
                foreach (var file in documentsForAdd)
                {
                    //get the file in byte array
                    byte[] fileData = File.ReadAllBytes(file.FilePath);

                    //Override the file path with the new one
                    file.FilePath = getCertificateTypeDocumentPath(file);
                    // Save the file to the new path
                    addCertificateTypeDocument(file, fileData);
                }
            }
        }

        internal static List<Certificate> GetCertificateForSailor(Sailor? sailor)
        {
            try
            {
                if (sailor == null)
                {
                    return new List<Certificate>();
                }


                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT c.ID, SailorID, CertificateTypeID, DateAcquired, DateExpiration, Place, Name FROM Certificates c INNER JOIN CertificateTypes t ON t.ID = c.CertificateTypeID WHERE SailorID = @sailorID";
                command.Parameters.AddWithValue("@sailorID", sailor.ID);
                command.ExecuteNonQuery();
                using var reader = command.ExecuteReader();
                var certificates = new List<Certificate>();
                while (reader.Read())
                {
                    var certificate = new Certificate
                    {
                        ID = reader.GetInt32(0), // ID

                        SailorID = reader.GetInt32(1), // SailorID
                        CertificateTypeID = reader.GetInt32(2), // CertificateTypeID
                        DateAcquired = reader.GetString(3), // DateAcquired
                        DateExpiration = reader.GetString(4), // DateExpiration
                        Place = reader.GetString(5), // Place
                        Name = reader.GetString(6) // Name
                    };
                    certificates.Add(certificate);
                }
                connection.Close();

                var output = new List<Certificate>();
                //Group by CertificateTypeID and get the latest one, while others are added to the versions
                var oderededCertificatesByIDDesc = certificates.OrderByDescending(x => x.ID).ToList();
                foreach (var certificate in oderededCertificatesByIDDesc)
                {
                    //check if output contains an certificate with that certificateTypeID 
                    if (output.Any(x => x.CertificateTypeID == certificate.CertificateTypeID))
                    {
                        //add the certificate to the versions
                        var index = output.FindIndex(x => x.CertificateTypeID == certificate.CertificateTypeID);
                        certificate.isOldVersion = true;
                        output[index].Versions.Add(certificate);
                        //output.Add(certificate);
                    }
                    else
                    {
                        
                            output.Add(certificate);
                    }
                }

                return output;

            }
            catch (Exception ex)
            {
                throw new Exception("Error GetCertificateForSailor from db", ex);
            }
        }

        internal static List<CertificateFile> getCertificateFilesForSailor(Certificate? certificate)
        {
            try
            {
                if (certificate == null)
                {
                    return new List<CertificateFile>();
                }

                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT ID, CertificateID, FilePath FROM CertificateFiles WHERE CertificateID = @certificateID";
                command.Parameters.AddWithValue("@certificateID", certificate.ID);
                command.ExecuteNonQuery();
                using var reader = command.ExecuteReader();
                var output = new List<CertificateFile>();
                while (reader.Read())
                {
                    var file = new CertificateFile
                    {
                        ID = reader.GetInt32(0), // ID

                        CertificateID = reader.GetInt32(1), // SailorID
                        FilePath = reader.GetString(2)
                    };
                    output.Add(file);
                }
                connection.Close();
                return output;

            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting certificate type document from the database", ex);
            }
        }
        private static string GetFullFilePath(CertificateFile file)
        {
            return Path.Combine(AppContext.BaseDirectory, "Data", "Certificates", file.CertificateID.ToString() ,file.FilePath);
        }

        private static string ConvertDocxToPdf(string docxFilePath)
        {
            try
            {
                // Define the output PDF path
                var outputPdfPath = Path.ChangeExtension(docxFilePath, ".pdf");

                // Read the .docx content using Open XML SDK
                using (var wordDoc = WordprocessingDocument.Open(docxFilePath, false))
                {
                    var body = wordDoc.MainDocumentPart.Document.Body;
                    var text = body.InnerText;

                    // Use PdfSharp to create a PDF
                    var document = new PdfSharp.Pdf.PdfDocument();
                    var page = document.AddPage();
                    var graphics = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
                    var font = new PdfSharp.Drawing.XFont("Arial", 12);
                    graphics.DrawString(text, font, PdfSharp.Drawing.XBrushes.Black,
                        new PdfSharp.Drawing.XRect(0, 0, page.Width, page.Height),
                        PdfSharp.Drawing.XStringFormats.TopLeft);

                    document.Save(outputPdfPath);
                }

                return outputPdfPath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error converting .docx to PDF: {ex.Message}", ex);
            }
        }

        internal static string? GeneratePrintFile(ObservableCollection<CertificateFile> selectedFiles)
        {
            try
            {
                // Define the output directory and file name
                var outputDirectory = Path.Combine(AppContext.BaseDirectory, "PrintOutput");
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                var outputFilePath = Path.Combine(outputDirectory, "CombinedOutput.pdf");

                // If only one file is selected, return the file path directly
                if (selectedFiles.Count() == 1)
                {
                    var singleFile = selectedFiles.First();
                    var singleFilePath = GetFullFilePath(singleFile);
                    return singleFilePath;
                }

                // Combine multiple files into a single PDF
                using (var outputDocument = new PdfSharp.Pdf.PdfDocument())
                {
                    foreach (var file in selectedFiles)
                    {
                        var filePath = GetFullFilePath(file);

                        if (Path.GetExtension(filePath).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                        {
                            // Add PDF pages to the output document
                            using (var inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import))
                            {
                                foreach (var page in inputDocument.Pages)
                                {
                                    outputDocument.AddPage(page);
                                }
                            }
                        }
                        else if (Path.GetExtension(filePath).Equals(".docx", StringComparison.OrdinalIgnoreCase))
                        {
                            var pdfPath = ConvertDocxToPdf(filePath);
                            using (var inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(pdfPath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import))
                            {
                                foreach (var page in inputDocument.Pages)
                                {
                                    outputDocument.AddPage(page);
                                }
                            }
                        }
                        else
                        {
                            // Handle other file types (e.g., images)
                            var page = outputDocument.AddPage();
                            using (var graphics = PdfSharp.Drawing.XGraphics.FromPdfPage(page))
                            {
                                var image = PdfSharp.Drawing.XImage.FromFile(filePath);
                                graphics.DrawImage(image, 0, 0, page.Width, page.Height);
                            }
                        }
                    }

                    // Save the combined PDF
                    outputDocument.Save(outputFilePath);
                }

                return outputFilePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating print file: {ex.Message}", ex);
            }
        }

        internal static string? GenerateZahtevi(int certificateTypeID, RequestTypeEnum obnova)
        {
            try
            {
                var files = getCertificateTypeDocuments(certificateTypeID);
                var outputDirectory = Path.Combine(AppContext.BaseDirectory, "PrintOutput");
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                var outputFilePath = Path.Combine(outputDirectory, "CombinedOutput.pdf");
                var zahtevi = files.Where(x => x.RequestType == obnova);
                if (zahtevi.Count() == 0)
                {
                    return null;
                }

                if (zahtevi.Count() == 1)
                {
                    var singleFile = zahtevi.First();
                    
                    return singleFile.FilePath;
                }

                using (var outputDocument = new PdfSharp.Pdf.PdfDocument())
                {
                    foreach (var file in zahtevi)
                    {
                        var filePath = file.FilePath;

                        if (Path.GetExtension(filePath).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                        {
                            // Add PDF pages to the output document
                            using (var inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import))
                            {
                                foreach (var page in inputDocument.Pages)
                                {
                                    outputDocument.AddPage(page);
                                }
                            }
                        }
                        else if (Path.GetExtension(filePath).Equals(".docx", StringComparison.OrdinalIgnoreCase))
                        {
                            var pdfPath = ConvertDocxToPdf(filePath);
                            using (var inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(pdfPath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import))
                            {
                                foreach (var page in inputDocument.Pages)
                                {
                                    outputDocument.AddPage(page);
                                }
                            }
                        }
                        else
                        {
                            // Handle other file types (e.g., images)
                            var page = outputDocument.AddPage();
                            using (var graphics = PdfSharp.Drawing.XGraphics.FromPdfPage(page))
                            {
                                var image = PdfSharp.Drawing.XImage.FromFile(filePath);
                                graphics.DrawImage(image, 0, 0, page.Width, page.Height);
                            }
                        }
                    }

                    // Save the combined PDF
                    outputDocument.Save(outputFilePath);
                }

                return outputFilePath;



            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating Zahtevi: {ex.Message}", ex);
            }
        }


        internal static List<Certificate> GetCertificatesWhichWillExpire(int days = 300) //switch to 300
        {
            try
            {
                var list = new List<Certificate>();


                DateTime dateThreshold = DateTime.Today.AddDays(days);
                string formattedDate = dateThreshold.ToString("yyyy-MM-dd");

                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT c.ID, c.SailorID, c.CertificateTypeID, c.DateAcquired, c.DateExpiration, c.Place, s.ID, s.GID, s.Name,s.Surname, s.IsRefresh, CT.ID, CT.Name FROM Certificates c INNER JOIN Sailors s ON c.SailorID = s.ID INNER JOIN CertificateTypes CT on c.CertificateTypeID = CT.ID WHERE (c.SailorID, c.CertificateTypeID, c.ID) IN (    SELECT SailorID, CertificateTypeID, MAX(ID)    FROM Certificates    GROUP BY SailorID, CertificateTypeID) AND c.DateExpiration <= @expDate ORDER BY c.DateExpiration ASC;";
                command.Parameters.AddWithValue("@expDate", formattedDate);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {

                    var certificate = new Certificate()
                    {
                        ID = reader.GetInt32(0), // ID
                        SailorID = reader.GetInt32(1), // SailorID  
                        CertificateTypeID = reader.GetInt32(2), // CertificateTypeID
                        DateAcquired = reader.GetString(3), // DateAcquired
                        DateExpiration = reader.GetString(4), // DateExpiration
                        Place = reader.GetString(5), // Place
                        Name = reader.GetString(12),
                        Sailor = new Sailor()
                        {
                            ID = reader.GetInt32(6), // ID
                            GID = reader.GetString(7), // GID
                            Name = reader.GetString(8), // Name
                            Surname = reader.GetString(9), // Surname
                            IsRefresh = reader.GetBoolean(10) // IsRefresh
                        },
                        CertificateType = new CertificateType()
                        {
                            ID = reader.GetInt32(11), // ID
                            Name = reader.GetString(12) // Name
                        }
                    };
                    list.Add(certificate);
                }
                connection.Close();


                return list;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error GetCertificatesWhichWillExpire{ex.Message}", ex);
            }
        }



        internal static void addCertificateFile(CertificateFile file, byte[] fileData)
        {
            try
            {
                //create a directory if it doesnt exist
                var directoryPath = $"{AppContext.BaseDirectory}/{CertificateFilesBaseFolder}/{file.CertificateID}";
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                //check if the file exists and change the name of the new one so it wont override
                var fileName = Path.GetFileName(file.FilePath);
                var filePath = Path.Combine(directoryPath, fileName);
                if (File.Exists(filePath))
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    var fileExtension = Path.GetExtension(fileName);
                    var newFileName = $"{fileNameWithoutExtension}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
                    file.FileName = newFileName;
                    file.FilePath = Path.Combine(directoryPath, newFileName);
                }
                else
                {
                    file.FilePath = filePath;
                }

                File.WriteAllBytes(file.FilePath, fileData);
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO CertificateFiles (FilePath, CertificateID) VALUES (@filePath, @certificateTypeID)";
                command.Parameters.AddWithValue("@filePath", file.FileName); //Save File Name insetad
                command.Parameters.AddWithValue("@certificateTypeID", file.CertificateID);
                command.ExecuteNonQuery();
                connection.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error adding certificate type document to the database", ex);
            }
        }
        internal static void addCertificateFiles(List<CertificateFile> documentsForAdd)
        {
            if (documentsForAdd.Count > 0)
            {

                foreach (var file in documentsForAdd)
                {
                    //get the file in byte array
                    byte[] fileData = File.ReadAllBytes(file.FilePath);

                    //Override the file path with the new one
                    file.FilePath = getCertificatFilePath(file);
                    // Save the file to the new path
                    addCertificateFile(file, fileData);
                }
            }
        }

        private static string getCertificatFilePath(CertificateFile file)
        {
            return $"{AppContext.BaseDirectory}/{CertificateFilesBaseFolder}/{file.CertificateID}/{file.FileName}";
        }

        internal static int AddCertificate(Certificate certificate)
        {
            try
            {
                var connection = GetConnection();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Certificates (SailorID, CertificateTypeID, DateAcquired, DateExpiration, Place) VALUES (@sailorID, @certificateTypeID, @dateAcquired, @dateExpiration, @place)";
                command.Parameters.AddWithValue("@sailorID", certificate.SailorID);
                command.Parameters.AddWithValue("@certificateTypeID", certificate.CertificateTypeID);
                command.Parameters.AddWithValue("@dateAcquired", certificate.DateAcquired);
                command.Parameters.AddWithValue("@dateExpiration", certificate.DateExpiration);
                command.Parameters.AddWithValue("@place", certificate.Place);
                command.ExecuteNonQuery();
                // Fix: Retrieve the last inserted row ID using SQLite's built-in function
                command.CommandText = "SELECT last_insert_rowid()";
                certificate.ID = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                return certificate.ID;
            }
            catch (Exception e)
            {
                throw new Exception("Error adding certificate to the database", e);
            }
        }

        internal static void AddCertificate(Certificate certificate, List<CertificateFile> files)
        {
            try
            {
                var id= AddCertificate(certificate);
                foreach (var file in files)
                {
                    file.CertificateID = id;
                }
                addCertificateFiles(files);
            }
            catch (Exception e)
            {
                throw new Exception("Error adding certificate to the database", e);
            }
        }

        #endregion




    }

}
