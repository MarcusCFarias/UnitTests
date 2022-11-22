using NUnit.Framework;
using TestNinja.Mocking;
using Moq;
using System.Collections.Generic;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceTests
    {
        private Mock<IFileReader> _fileReader;
        private Mock<IVideoRepository> _repository;
        private VideoService _videoService;

        [SetUp]
        public void SetUp()
        {
            _fileReader = new Mock<IFileReader>();
            _repository = new Mock<IVideoRepository>();
            _videoService = new VideoService(_fileReader.Object, _repository.Object);
        }

        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnError()
        {
            _fileReader.Setup(fr => fr.Read("video.txt")).Returns("");

            var result = _videoService.ReadVideoTitle();

            Assert.That(result, Does.Contain("Error").IgnoreCase);
        }

        [Test]
        public void GetUnprocessedVideos_AllVideosAreProcessed_ReturnEmptyString()
        {
            _repository.Setup(r => r.GetUnprocessedVideos()).Returns(new List<Video>());

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void GetUnprocessedVideos_AFewUnprocessedVideos_ReturnStringWithListIdsVideos()
        {
            List<Video> listVideos = new List<Video>();
            listVideos.Add(new Video() { Id = 1 });
            listVideos.Add(new Video() { Id = 2 });
            listVideos.Add(new Video() { Id = 3 });

            _repository.Setup(r => r.GetUnprocessedVideos()).Returns(listVideos);

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo("1,2,3"));
        }
    }
}
