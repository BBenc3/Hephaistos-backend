using Microsoft.Extensions.Options;
using System.Net;

namespace ProjectHephaistos.Services
{
    public class FtpConfig
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class FtpService
    {
        private readonly FtpConfig _config;

        public FtpService(IOptions<FtpConfig> config)
        {
            _config = config.Value;
        }

        public void UploadFile(string localFilePath, string remoteFileName)
        {
            var request = (FtpWebRequest)WebRequest.Create($"ftp://{_config.Url}/{remoteFileName}");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(_config.Username, _config.Password);

            using (var fileStream = File.OpenRead(localFilePath))
            using (var requestStream = request.GetRequestStream())
            {
                fileStream.CopyTo(requestStream);
            }
        }

        public void DownloadFile(string remoteFileName, string localFilePath)
        {
            var request = (FtpWebRequest)WebRequest.Create($"ftp://{_config.Url}/{remoteFileName}");
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(_config.Username, _config.Password);

            using (var response = (FtpWebResponse)request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var fileStream = File.Create(localFilePath))
            {
                responseStream.CopyTo(fileStream);
            }
        }

        public IEnumerable<string> ListDirectory(string remotePath)
        {
            var request = (FtpWebRequest)WebRequest.Create($"ftp://{_config.Url}/{remotePath}");
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(_config.Username, _config.Password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            using (var response = (FtpWebResponse)request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            {
                var result = new List<string>();
                while (!reader.EndOfStream)
                {
                    result.Add(reader.ReadLine());
                }
                return result;
            }
        }
    }
}
