using System.Globalization;
using System.Net;

namespace Birthday
{
    public class DateKeywordParser
    {
        //public DateTime Parse(string input)
        //{
        //    // Implement parsing logic here
        //    // Handle keywords like NOW, YESTERDAY, TODAY
        //    // Handle relative offsets like NOW-1d, NOW+2h
        //    // Handle specific formats like Format(NOW, "yyyy-MM-dd")
        //}

        public string FormatDate(DateTime date, string format, CultureInfo culture)
        {
            return date.ToString(format, culture);
        }

        private DateTime GetDateFromKeyword(string keyword)
        {
            switch (keyword.ToUpper())
            {
                case "NOW":
                    return DateTime.Now;
                case "YESTERDAY":
                    return DateTime.Now.AddDays(-1);
                case "TODAY":
                    return DateTime.Today;
                default:
                    throw new ArgumentException("Unsupported keyword");
            }
        }
    }

    class FtpFileDownloader
    {
        static void Main(string[] args)
        {
            string ftpServer = "ftp://ftp.example.com";
            string username = "yourUsername";
            string password = "yourPassword";

            // Calculate yesterday's date
            DateTime yesterday = DateTime.Now.AddDays(-1);
            string formattedDate = yesterday.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string fileName = $"data-{formattedDate}.txt";

            // Full FTP file path
            string ftpFilePath = $"{ftpServer}/{fileName}";

            // Download the file
            DownloadFile(ftpFilePath, username, password);
        }

        static void DownloadFile(string ftpFilePath, string username, string password)
        {
            try
            {
                // Create a request to the FTP server
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFilePath);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(username, password);

                // Get the response from the server
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream fileStream = new FileStream(Path.GetFileName(ftpFilePath), FileMode.Create))
                {
                    responseStream.CopyTo(fileStream);
                    Console.WriteLine($"Download Complete, status {response.StatusDescription}");
                }
            }
            catch (WebException ex)
            {
                // Handle errors (e.g., file not found)
                if (ex.Response is FtpWebResponse ftpResponse)
                {
                    Console.WriteLine($"Error: {ftpResponse.StatusDescription}");
                }
                else
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
