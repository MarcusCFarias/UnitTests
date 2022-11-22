using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class InstallerHelperTests
    {
        private Mock<IFileDownLoader> _fileDownloader { get; set; }
        private InstallerHelper _installerHelper { get; set; }

        [SetUp]
        public void SetUp()
        {
            this._fileDownloader = new Mock<IFileDownLoader>();
            this._installerHelper = new InstallerHelper(_fileDownloader.Object);
        }


        [Test]
        public void DownloadInstaller_DownloadFails_ReturnFalse()
        {
            _fileDownloader
                    .Setup(x => x.DownloadInstaller(It.IsAny<string>(), It.IsAny<string>()))
                    .Throws<WebException>();

            var result = _installerHelper.DownloadInstaller("customer", "installer");

            Assert.That(result, Is.False);
        }

        [Test]
        public void DownloadInstaller_DownloadComplete_ReturnTrue()
        {
            var result = _installerHelper.DownloadInstaller("customer", "installer");

            Assert.That(result, Is.True);
        }
    }
}
