class Program
{
    static void Main()
    {
        string ftpServer = "ftp://ftp.example.com";
        string username = "yourUsername";
        string password = "yourPassword";
        string pattern = "data-{YESTERDAY}.txt";

        var downloader = new FtpFileDownloader();
        downloader.Download(pattern, ftpServer, username, password);
    }
}

