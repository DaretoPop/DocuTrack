using System;
using System.IO;

namespace DocuTrack.Services
{
    public static class TrialManager
    {
        private static readonly string TrialFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "DocuTrack", "trial_start.txt");

        private static readonly string DatabasePath = Path.Combine(
#if DEBUG
            "Data", "identifier.sqlite"
#else
            AppContext.BaseDirectory, "Data", "identifier.sqlite"
#endif
        );

        public static bool IsTrialExpired()
        {
            DateTime startDate;

            // ↓ REMOVE or COMMENT OUT this block so startDate isn’t reset each launch
            // if (File.Exists(TrialFilePath))
            //     File.Delete(TrialFilePath);

            if (!File.Exists(TrialFilePath))
            {
                startDate = DateTime.Now;
                Directory.CreateDirectory(Path.GetDirectoryName(TrialFilePath)!);
                File.WriteAllText(TrialFilePath, startDate.ToString("O"));
            }
            else
            {
                var text = File.ReadAllText(TrialFilePath);
                if (!DateTime.TryParse(text, out startDate))
                    return true; 
            }

            // TEST: expire after 1 minute
            return DateTime.Now > startDate.AddMinutes(1);

            // PRODUCTION: expire after 7 days
            // return DateTime.Now > startDate.AddDays(7);
        }

        public static void DeleteDatabase()
        {
            if (File.Exists(DatabasePath))
                File.Delete(DatabasePath);
        }
    }
}

// Remove to open again app 
// /home/pop/.local/share/DocuTrack/trial_start.txt <- (LINUX)
// del $env:LOCALAPPDATA\DocuTrack\trial_start.txt <- WIN


