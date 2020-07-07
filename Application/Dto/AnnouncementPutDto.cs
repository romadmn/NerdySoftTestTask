using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class AnnouncementPutDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public List<string> FieldMasks { get; set; }
    }
}
