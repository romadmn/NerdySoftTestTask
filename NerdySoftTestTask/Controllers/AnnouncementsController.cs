using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace NerdySoftTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementsController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        // GET: api/Announcements
        [HttpGet]
        public async Task<ActionResult<List<AnnouncementGetDto>>> GetAllAsync()
        {
            return Ok(await _announcementService.GetAllAsync());
        }

        // GET: api/Announcements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnnouncementDetailsDto>> GetByIdAsync([FromRoute] int id)
        {
            var announcement = await _announcementService.GetByIdAsync(id);
            if (announcement == null)
                return NotFound();
            return Ok(announcement);
        }

        // POST: api/Announcements
        [HttpPost]
        public async Task<ActionResult<AnnouncementPostDto>> PostAsync([FromForm] AnnouncementPostDto announcementDto)
        {
            var insertedAnnouncement = await _announcementService.AddAsync(announcementDto);
            return CreatedAtAction("GetByIdAsync", new { id = insertedAnnouncement.Id }, insertedAnnouncement);
        }

        // PUT: api/Announcements/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromForm] AnnouncementPutDto announcementDto)
        {
            if (id != announcementDto.Id)
            {
                return BadRequest();
            }

            if (announcementDto == null)
            {
                return BadRequest();
            }

            var isAnnouncementUpdated = await _announcementService.UpdateAsync(announcementDto);
            if (!isAnnouncementUpdated)
            {
                return BadRequest();
            }
            return NoContent();
        }

        // DELETE: api/Announcements/5
        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var isAnnouncementRemoved = await _announcementService.RemoveAsync(id);
            if (!isAnnouncementRemoved)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
