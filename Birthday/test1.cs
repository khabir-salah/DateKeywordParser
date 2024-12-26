using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

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

        DownloadFile(ftpFilePath, username, password);
    }

    private string ResolveFileName(string pattern)
    {
        return Regex.Replace(pattern, @"\{([^}]+)\}", match =>
        {
            string keyword = match.Groups[1].Value;
            DateTime resolvedDate = _parser.Parse(keyword);
            return resolvedDate.ToString("yyyy-MM-dd"); // Default format
        });
    }

    private void DownloadFile(string ftpFilePath, string username, string password)
    {
        try
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFilePath);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(username, password);

            using FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            using Stream responseStream = response.GetResponseStream();
            using FileStream fileStream = new FileStream(Path.GetFileName(ftpFilePath), FileMode.Create);

            responseStream.CopyTo(fileStream);
            Console.WriteLine($"Download Complete, status {response.StatusDescription}");
        }
        catch (WebException ex)
        {
            if (ex.Response is FtpWebResponse ftpResponse)
                Console.WriteLine($"Error: {ftpResponse.StatusDescription}");
            else
                Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

