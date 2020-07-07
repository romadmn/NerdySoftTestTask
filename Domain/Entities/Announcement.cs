using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Announcement : IEntityBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
