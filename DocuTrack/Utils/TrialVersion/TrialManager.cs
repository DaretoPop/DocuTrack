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

            // ↓ REMOVE this block so startDate isn’t reset each launch
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
            return DateTime.Now > startDate.AddMinutes(4);

            // expire after 7 days
            // return DateTime.Now > startDate.AddDays(7);
        }

      

        public static TimeSpan GetTimeRemaining()
{
            if (!File.Exists(TrialFilePath))
                return TimeSpan.Zero;

            var text = File.ReadAllText(TrialFilePath);
            if (!DateTime.TryParse(text, out var startDate))
                return TimeSpan.Zero;

            // match with expired date
            var expiry = startDate.AddMinutes(4);    // or AddDays(7)
            var remaining = expiry - DateTime.Now;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }
    }
}



