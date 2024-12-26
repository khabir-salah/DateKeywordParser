// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


string ftpServer = "ftp://localhost";
string username = "Salahudeen";
string password = "1234567890";
string pattern = "TODAY";

var downloader = new FtpFileDownloader();
downloader.Download(pattern, ftpServer, username, password);
