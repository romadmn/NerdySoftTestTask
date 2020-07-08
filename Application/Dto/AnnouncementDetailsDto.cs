using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class AnnouncementDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public List<AnnouncementGetDto> SimilarAnnouncements { get; set; }
    }
}
