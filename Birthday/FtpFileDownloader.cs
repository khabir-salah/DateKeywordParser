
using System.Text.RegularExpressions;
using Birthday;

public class FtpFileDownloader
{
    private readonly DateKeywordParser _parser;

    public FtpFileDownloader()
    {
        _parser = new DateKeywordParser();
    }

    public void Download(string pattern, string ftpServer, string username, string password)
    {
        string resolvedFileName = ResolveFileName(pattern);
        string ftpFilePath = $"{ftpServer}/{resolvedFileName}";


        //A method to download file from FileZile after conversion of the correct date
        //DownloadFile(ftpFilePath, username, password);
        Console.WriteLine($"Formated Date = {resolvedFileName}");
    }

    private string ResolveFileName(string pattern)
    {
        // Handle specific formats like Format(NOW, "yyyy-MM-dd")
        if (pattern.StartsWith("Format("))
        {
            return _parser.Format(pattern);
        }

        // Handle single keywords (e.g., YESTERDAY, NOW-1d)
        if (Regex.IsMatch(pattern, @"^[A-Z]+[-+]\d+[dhms]$", RegexOptions.IgnoreCase) ||
            Regex.IsMatch(pattern, @"^[A-Z]+$", RegexOptions.IgnoreCase))
        {
            DateTime resolvedDate = _parser.Parse(pattern);
            return resolvedDate.ToString("yyyy-MM-dd");
        }

        // Handle placeholders (e.g., data-{YESTERDAY}.txt, log-{NOW-1d}.log)
        return Regex.Replace(pattern, @"\{([^}]+)\}", match =>
        {
            string keyword = match.Groups[1].Value;

            if (keyword.StartsWith("Format("))
            {
                return _parser.Format(keyword);
            }

            DateTime resolvedDate = _parser.Parse(keyword);
            return resolvedDate.ToString("yyyy-MM-dd");
        });
    }

}

