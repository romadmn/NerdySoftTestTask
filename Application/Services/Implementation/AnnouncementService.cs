using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Dto;
using AutoMapper;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementation
{
    public class AnnouncementService : Interfaces.IAnnouncementService
    {
        private readonly IRepository<Announcement> _announcementRepository;
        private readonly IMapper _mapper;

        public AnnouncementService(IRepository<Announcement> announcementRepository, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<AnnouncementGetDto>> GetAllAsync()
        {
            var announcements = await _announcementRepository.GetAll().ToListAsync();
                
            if (announcements == null)
            {
                return null;
            }
            return _mapper.Map<List<AnnouncementGetDto>>(announcements);
        }

        /// <inheritdoc />
        public async Task<AnnouncementDetailsDto> GetByIdAsync(int announcementId)
        {
            var announcement = _mapper.Map<AnnouncementDetailsDto>(
                await _announcementRepository.FindByCondition(x => x.Id == announcementId));
            var similarAnnouncements = _announcementRepository.GetAll().ToList()
                .Where(x=> x.Title.Split(' ')
                    .Any(w=> announcement.Title.Contains(w))).Where(x => x.Description.Split(' ')
                    .Any(w => announcement.Description.Contains(w))).Where(x=>x.Id != announcementId).Take(3);
            announcement.SimilarAnnouncements = _mapper.Map<List<AnnouncementGetDto>>(similarAnnouncements);
            return announcement;
        }

        /// <inheritdoc />
        public async Task<AnnouncementGetDto> AddAsync(AnnouncementPostDto announcementDto)
        {
            var announcement = _mapper.Map<Announcement>(announcementDto);
            _announcementRepository.Add(announcement);
            await _announcementRepository.SaveChangesAsync();
            return _mapper.Map<AnnouncementGetDto>(announcement);
        }

        /// <inheritdoc />
        public async Task<bool> RemoveAsync(int announcementId)
        {
            var announcement = await _announcementRepository.FindByCondition(x=>x.Id == announcementId);
            if (announcement == null)
            {
                return false;
            }
            _announcementRepository.Remove(announcement);
            var affectedRows = await _announcementRepository.SaveChangesAsync();
            return affectedRows > 0;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(AnnouncementPutDto announcementDto)
        {
            var book = _mapper.Map<Announcement>(announcementDto);
            var oldAnnouncement = await _announcementRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(a => a.Id == book.Id);
            if (oldAnnouncement == null)
            {
                return false;
            }
            await _announcementRepository.Update(book, announcementDto.FieldMasks);
            var affectedRows = await _announcementRepository.SaveChangesAsync();
            return affectedRows > 0;
        }

    }
}
