using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NerdySoftTestTask.Controllers;
using NUnit.Framework;

namespace ApplicationTest.Controllers
{
    [TestFixture]
    class AnnouncementsControllerTest
    {
        private Mock<IAnnouncementService> _announcementServiceMock;
        private AnnouncementsController _announcementsController;

        [OneTimeSetUp]
        public void SetUp()
        {
            _announcementServiceMock = new Mock<IAnnouncementService>();
            _announcementsController = new AnnouncementsController(_announcementServiceMock.Object); ;
        }

        [Test]
        public async Task GetAllAnnouncementsAsync_Returns_OkObjectResultWithRequestedCount()
        {
            var testBooks = new List<AnnouncementGetDto>()
                {
                    new AnnouncementGetDto(),
                    new AnnouncementGetDto()
                };
            _announcementServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(testBooks);

            var getAllAnnouncementsResult = await _announcementsController.GetAllAsync();

            var okResult = getAllAnnouncementsResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var announcements = okResult.Value as List<AnnouncementGetDto>;
            announcements.Should().HaveCount(testBooks.Count);
        }

        private List<AnnouncementPutDto> GetTestAnnouncements()
        {
            return new List<AnnouncementPutDto>
            {
                new AnnouncementPutDto(),
                new AnnouncementPutDto()
            };
        }

        [Test]
        public async Task GetAnnouncementAsync_AnnouncementExists_Returns_OkObjectResultWithRequestedId()
        {
            var testAnnouncement = new AnnouncementDetailsDto() { Id = 1 };

            _announcementServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(testAnnouncement);

            var getAnnouncementResult = await _announcementsController.GetByIdAsync(It.IsAny<int>());

            var okResult = getAnnouncementResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var resultAnnouncement = okResult.Value as AnnouncementDetailsDto;
            resultAnnouncement.Id.Should().Be(testAnnouncement.Id);
        }

        private AnnouncementPutDto GetTestAnnouncement()
        {
            return new AnnouncementPutDto() { Id = 1 };
        }

        [Test]
        public async Task GetAnnouncementAsync_AnnouncementDoesNotExist_Returns_NotFoundResult()
        {
            _announcementServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as AnnouncementDetailsDto);

            var result = await _announcementsController.GetByIdAsync(It.IsAny<int>());

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PutAnnouncementAsync_AnnouncementExists_Returns_NoContent()
        {
            var testAnnouncement = GetTestAnnouncement();
            _announcementServiceMock.Setup(m => m.UpdateAsync(It.IsAny<AnnouncementPutDto>())).ReturnsAsync(true);

            var putAnnouncementResult = await _announcementsController.PutAsync(testAnnouncement.Id, testAnnouncement);

            putAnnouncementResult.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task PutAnnouncementAsync_AnnouncementDoesNotExist_Return_NotFound()
        {
            var testAnnouncement = GetTestAnnouncement();
            _announcementServiceMock.Setup(m => m.UpdateAsync(It.IsAny<AnnouncementPutDto>())).ReturnsAsync(false);

            var putAnnouncementResult = await _announcementsController.PutAsync(testAnnouncement.Id, testAnnouncement);

            putAnnouncementResult.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task DeleteAnnouncementAsync_AnnouncementExists_Returns_OkResult()
        {
            _announcementServiceMock.Setup(m => m.RemoveAsync(It.IsAny<int>())).ReturnsAsync(true);

            var deleteAnnouncementResult = await _announcementsController.DeleteAsync(It.IsAny<int>());

            deleteAnnouncementResult.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task DeleteAnnouncementAsync_AnnouncementDoesNotExist_Returns_NotFoundResult()
        {
            _announcementServiceMock.Setup(m => m.RemoveAsync(It.IsAny<int>())).ReturnsAsync(false);

            var deleteAnnouncementResult = await _announcementsController.DeleteAsync(It.IsAny<int>());

            deleteAnnouncementResult.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task PostAnnouncementAsync_Returns_CreatedAtActionResult()
        {
            var testAnnouncement = new AnnouncementGetDto() { Id = 1 };
            _announcementServiceMock.Setup(m => m.AddAsync(It.IsAny<AnnouncementPostDto>())).ReturnsAsync(testAnnouncement);

            var createdAtActionResult = await _announcementsController.PostAsync(It.IsAny<AnnouncementPostDto>());
            var result = (AnnouncementGetDto)((CreatedAtActionResult)createdAtActionResult.Result).Value;

            result.Should().BeOfType<AnnouncementGetDto>();
            createdAtActionResult.Result.Should().BeOfType<CreatedAtActionResult>();
            result.Should().BeEquivalentTo(testAnnouncement, options => options.Excluding(a => a.Id));
        }
    }
}
