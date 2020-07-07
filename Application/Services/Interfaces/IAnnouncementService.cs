using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;

namespace Application.Services.Interfaces
{
    public interface IAnnouncementService
    {
        /// <summary>
        /// Retrieve announcement by ID
        /// </summary>
        /// <param name="announcementId">announcement's ID</param>
        /// <returns>returns announcement DTO</returns>
        Task<AnnouncementDetailsDto> GetByIdAsync(int announcementId);

        /// <summary>
        /// Retrieve all announcements
        /// </summary>
        /// <returns>returns list of announcement DTOs</returns>
        Task<List<AnnouncementGetDto>> GetAllAsync();

        /// <summary>
        /// Update specified announcement
        /// </summary>
        /// <param name="announcement">announcement DTO instance</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(AnnouncementPutDto announcement);

        /// <summary>
        /// Remove announcement from database
        /// </summary>
        /// <param name="announcementId">announcement's ID</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(int announcementId);

        /// <summary>
        /// Create new announcement and add it into Database
        /// </summary>
        /// <param name="announcement">announcement DTO instance</param>
        /// <returns>Returns inserted announcement's ID</returns>
        Task<AnnouncementGetDto> AddAsync(AnnouncementPostDto announcement);

    }
}
