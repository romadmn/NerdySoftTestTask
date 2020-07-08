using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using AutoMapper;
using Domain;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Services
{
    [TestFixture]
    class AnnouncementServiceTest
    {
        private IAnnouncementService _announcementService;
        private Mock<IRepository<Announcement>> _announcementRepositoryMock;
        private IMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _announcementRepositoryMock = new Mock<IRepository<Announcement>>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Application.MapperProfilers.AnnouncementProfiler());
            });
            _mapper = mappingConfig.CreateMapper();
            _announcementService = new AnnouncementService(_announcementRepositoryMock.Object, _mapper);
            var announcementsMock = GetAnnouncements().AsQueryable();
        }

        [SetUp]
        public void SetUp()
        {
            _announcementRepositoryMock.Reset();
        }


        [Test]
        public async Task GetByIdAsync_AnnouncementExists_Returns_AnnouncementDtoWithRequestedId()
        {
            var announcementsMock = new Announcement() { Id = 1 };
            _announcementRepositoryMock.Setup(s => s.FindByCondition(It.IsAny<Expression<Func<Announcement, bool>>>())).ReturnsAsync(announcementsMock);

            var announcementResult = await _announcementService.GetByIdAsync(1);

            announcementResult.Should().BeOfType<AnnouncementDetailsDto>();
            announcementResult.Id.Should().Be(1);
        }

        private List<Announcement> GetTestAnnouncements()
        {
            return new List<Announcement>
            {
                new Announcement(){ Id = 1},
                new Announcement(){ Id = 2}
            };
        }
       

        [Test]
        public async Task GetByIdAsync_AnnouncementDoesNotExist_Returns_Null()
        {
            var announcementsMock = GetTestAnnouncements().AsQueryable().BuildMock();
            _announcementRepositoryMock.Setup(s => s.GetAll()).Returns(announcementsMock.Object);

            var bookResult = await _announcementService.GetByIdAsync(3);

            bookResult.Should().BeNull();
        }
        [Test]
        public async Task AddAsync_AnnouncementIsValid_Returns_AnnouncementGetDto()
        {
            var announcementDto = new AnnouncementPostDto();
            _announcementRepositoryMock.Setup(s => s.Add(It.IsAny<Announcement>()));
            _announcementRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var announcementResult = await _announcementService.AddAsync(announcementDto);

            announcementResult.Should().BeOfType<AnnouncementGetDto>();
            _announcementRepositoryMock.Verify(x => x.Add(It.IsAny<Announcement>()), Times.Once);
            _announcementRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveAsync_AnnouncementExists_Returns_True()
        {
            var announcement = new Announcement();
            _announcementRepositoryMock.Setup(s => s.FindByCondition(It.IsAny<Expression<Func<Announcement, bool>>>())).ReturnsAsync(announcement);
            _announcementRepositoryMock.Setup(s => s.Remove(It.IsAny<Announcement>()));
            _announcementRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var deleteResult = await _announcementService.RemoveAsync(1);

            deleteResult.Should().BeTrue();
            _announcementRepositoryMock.Verify(x => x.Remove(It.IsAny<Announcement>()), Times.Once);
            _announcementRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveAsync_AnnouncementDoesNotExist_Returns_False()
        {
            var announcement = new Announcement();
            _announcementRepositoryMock.Setup(s => s.FindByCondition(x=>x.Id == It.IsAny<int>())).ReturnsAsync(null as Announcement);
            _announcementRepositoryMock.Setup(s => s.Remove(It.IsAny<Announcement>()));
            _announcementRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var deleteResult = await _announcementService.RemoveAsync(3);

            deleteResult.Should().BeFalse();
        }

        [Test]
        public async Task Update_AnnouncementExists_Returns_True()
        {
            var announcementsMock = GetTestAnnouncements().AsQueryable().BuildMock();
            _announcementRepositoryMock.Setup(s => s.GetAll()).Returns(announcementsMock.Object);
            _announcementRepositoryMock.Setup(s => s.Update(It.IsAny<Announcement>(), It.IsAny<List<string>>()));
            _announcementRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var announcementDto = new AnnouncementPutDto() { Id = 1, FieldMasks = new List<string> { "Title" } };

            var result = await _announcementService.UpdateAsync(announcementDto);

            result.Should().BeTrue();
        }

        [Test]
        public async Task Update_AnnouncementDoesNotExist_Returns_False()
        {
            var announcementsMock = GetTestAnnouncements().AsQueryable().BuildMock();
            _announcementRepositoryMock.Setup(s => s.GetAll()).Returns(announcementsMock.Object);
            _announcementRepositoryMock.Setup(s => s.Update(It.IsAny<Announcement>(), It.IsAny<List<string>>()));
            _announcementRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var announcementDto = new AnnouncementPutDto() { Id = 3, FieldMasks = new List<string> { "Title" } };

            var result = await _announcementService.UpdateAsync(announcementDto);

            result.Should().BeFalse();
        }

        private List<Announcement> GetAnnouncements()
        {
            return new List<Announcement>()
            {
                new Announcement() { Id = 1, Title = "TestApp", Description = "Roman", DateAdded = DateTime.MaxValue},
                new Announcement() { Id = 2, Title = "For NerdySoft", Description = "Ferents", DateAdded = DateTime.MinValue}
            };
        }
       
        [Test]
        public async Task GetAll_Returns_ListOfAnnouncementsWithSameCount()
        {
            var announcementsMock = GetAnnouncements().AsQueryable().BuildMock();
            _announcementRepositoryMock.Setup(s => s.GetAll()).Returns(announcementsMock.Object);

            var announcementsResult = await _announcementService.GetAllAsync();

            announcementsResult.Should().BeOfType<List<AnnouncementGetDto>>();
            announcementsResult.Should().HaveCount(2);
        }
    }
}
