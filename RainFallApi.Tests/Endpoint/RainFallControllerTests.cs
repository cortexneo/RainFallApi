using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RainFallApi.Domain;
using RainFallApi.Domain.Models;
using RainFallApi.Presentation.Controllers;
using System.Net;

namespace RainFallApi.Tests.Endpoint
{
    [TestFixture]
    public class RainFallControllerTests
    {
        private Mock<IRainFallReaderService> _mockRainFallReaderService;
        private RainfallController _rainfallController;
        [SetUp]
        public void Setup()
        {
            _mockRainFallReaderService = new Mock<IRainFallReaderService>();
            _rainfallController = new RainfallController(_mockRainFallReaderService.Object);
        }

        [Test]
        public async Task GetRainfallReadings_Should_Return_RainfallReadingResponse()
        {
            //Arrange
            var rainfallReadingResponse = new RainfallReadingResponse
            {
                Readings = new List<RainfallReading> { new() { AmountMeasured = 0, DateMeasured  = DateTime.Now } }
            };

            _mockRainFallReaderService
                .Setup(x => x.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(rainfallReadingResponse);

            //Act
            var result = await _rainfallController.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>());
            var okResult = result as OkObjectResult;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(okResult?.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(rainfallReadingResponse, Is.InstanceOf<RainfallReadingResponse>());
            });
            _mockRainFallReaderService
                .Verify(x => x.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task GetRainfallReadings_Returns_NotFound()
        {
            //Arrange
            var rainfallReadingResponse = new RainfallReadingResponse
            {
                Readings = new List<RainfallReading>()
            };

            _mockRainFallReaderService
                .Setup(x => x.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(rainfallReadingResponse);

            //Act
            var result = await _rainfallController.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>());

            //Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
            _mockRainFallReaderService
                .Verify(x => x.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task GetRainfallReadings_Returns_BadRequest()
        {
            //Arrange
            _mockRainFallReaderService
                .Setup(x => x.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>())).ThrowsAsync(new ArgumentNullException());

            //Act
            var result = await _rainfallController.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>());

            //Assert
            Assert.That(result, Is.TypeOf<BadRequestResult>());
            _mockRainFallReaderService
                .Verify(x => x.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }


        [Test]
        public async Task GetRainfallReadings_Returns_InternalServerError()
        {
            //Arrange
            _mockRainFallReaderService
                .Setup(x => x.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>())).ThrowsAsync(new Exception());

            //Act
            var result = await _rainfallController.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>()) as ObjectResult;

            //Assert
            Assert.That(result?.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            _mockRainFallReaderService
                .Verify(x => x.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }
    }
}
