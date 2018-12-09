using System;
using System.IO;
using System.Net;
using System.Text;

namespace CMSSolutions.Websites.Extensions
{
    public class FTPClient
    {
        public string ServerIP { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int BufferSize { get; set; }

        public FTPClient()
        {
            BufferSize = 2048;
        }

        public FTPClient(string hostIP, string userName, string password)
        {
            ServerIP = hostIP;
            UserName = userName;
            Password = password;
            BufferSize = 2048;
        }

        private string GetUrl(string path)
        {
            return "ftp://" + ServerIP + "/" + path;
        }

        public void Download(string remoteFile, string localFile)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(GetUrl(remoteFile));
                ftpRequest.Credentials = new NetworkCredential(UserName, Password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                Stream ftpStream = ftpResponse.GetResponseStream();
                if (ftpStream != null)
                {
                    var localFileStream = new FileStream(localFile, FileMode.Create);
                    var byteBuffer = new byte[BufferSize];
                    int bytesRead = ftpStream.Read(byteBuffer, 0, BufferSize);
                    while (bytesRead > 0)
                    {
                        localFileStream.Write(byteBuffer, 0, bytesRead);
                        bytesRead = ftpStream.Read(byteBuffer, 0, BufferSize);
                    }
                    localFileStream.Close();
                    ftpStream.Close();
                }

                ftpResponse.Close();               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Upload(string remoteFile, string localFile)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(GetUrl(remoteFile));
                ftpRequest.Credentials = new NetworkCredential(UserName, Password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                Stream ftpStream = ftpRequest.GetRequestStream();
                if (ftpStream != null)
                {
                    var localFileStream = new FileStream(localFile, FileMode.Create);
                    var byteBuffer = new byte[BufferSize];
                    int bytesSent = localFileStream.Read(byteBuffer, 0, BufferSize);
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, BufferSize);
                    }
                    localFileStream.Close();
                    ftpStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(string deleteFile)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(GetUrl(deleteFile));
                ftpRequest.Credentials = new NetworkCredential(UserName, Password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Rename(string currentFileNameAndPath, string newFileName)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(GetUrl(currentFileNameAndPath));
                ftpRequest.Credentials = new NetworkCredential(UserName, Password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                ftpRequest.RenameTo = newFileName;
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateDirectory(string newDirectory)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(GetUrl(newDirectory));
                ftpRequest.Credentials = new NetworkCredential(UserName, Password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetFileCreatedDateTime(string fileName)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(GetUrl(fileName));
                ftpRequest.Credentials = new NetworkCredential(UserName, Password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                Stream ftpStream = ftpResponse.GetResponseStream();
                if (ftpStream != null)
                {
                    var ftpReader = new StreamReader(ftpStream);
                    string fileInfo = ftpReader.ReadToEnd();
                    ftpReader.Close();
                    ftpStream.Close();
                    ftpResponse.Close();
                    return fileInfo;
                }

                ftpResponse.Close();
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetFileSize(string fileName)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(GetUrl(fileName));
                ftpRequest.Credentials = new NetworkCredential(UserName, Password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                Stream ftpStream = ftpResponse.GetResponseStream();
                if (ftpStream != null)
                {
                    var ftpReader = new StreamReader(ftpStream);
                    string fileInfo = null;
                    while (ftpReader.Peek() != -1)
                    {
                        fileInfo = ftpReader.ReadToEnd();
                    }
                    ftpReader.Close();
                    ftpStream.Close();
                    ftpResponse.Close();

                    return fileInfo;
                }

                ftpResponse.Close();
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] DirectoryList(string directory)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(GetUrl(directory));
                ftpRequest.Credentials = new NetworkCredential(UserName, Password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                Stream ftpStream = ftpResponse.GetResponseStream();
                if (ftpStream != null)
                {
                    var ftpReader = new StreamReader(ftpStream);
                    string directoryRaw = null;
                    while (ftpReader.Peek() != -1)
                    {
                        directoryRaw += ftpReader.ReadLine() + "|";
                    }

                    ftpReader.Close();
                    ftpStream.Close();
                    ftpResponse.Close();
                    if (directoryRaw != null)
                    {
                        return directoryRaw.Split("|".ToCharArray());
                    }
                }

                ftpResponse.Close();
                return new[] { "" }; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] DirectoryListDetailed(string directory)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(GetUrl(directory));
                ftpRequest.Credentials = new NetworkCredential(UserName, Password);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                Stream ftpStream = ftpResponse.GetResponseStream();
                if (ftpStream != null)
                {
                    var ftpReader = new StreamReader(ftpStream);
                    string directoryRaw = null;
                    while (ftpReader.Peek() != -1)
                    {
                        directoryRaw += ftpReader.ReadLine() + "|";
                    }
                    ftpReader.Close();
                    ftpStream.Close();
                    ftpResponse.Close();
                    if (directoryRaw != null)
                    {
                        string[] directoryList = directoryRaw.Split("|".ToCharArray());
                        return directoryList;
                    }
                }

                ftpResponse.Close();
                return new[] { "" }; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}