using System.Net;

namespace TestNinja.Mocking
{
    public class InstallerHelper
    {
        public IFileDownLoader _fileDownloader;

        private string _setupDestinationFile;

        public InstallerHelper(IFileDownLoader fileDownLoader)
        {
            this._fileDownloader = fileDownLoader;
        }

        public bool DownloadInstaller(string customerName, string installerName)
        {
            try
            {
                var url = string.Format("http://example.com/{0}/{1}", customerName, installerName);

                _fileDownloader.DownloadInstaller(url, _setupDestinationFile);

                return true;
            }
            catch (WebException)
            {
                return false;
            }

        }
    }
}