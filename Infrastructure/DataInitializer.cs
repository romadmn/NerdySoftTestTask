using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Internal;


namespace Infrastructure
{
    public class DataInitializer
    {
        public static void Initialize(NerdySoftContext context)
        {
            if (!context.Announcement.Any())
            {
                context.Add(new Announcement()
                {
                    Title = "Roman Ferents would like to work in NerdySoft!",
                    Description = "Give me an offer and I instantly click with you and we will be friends 0_o!"
                });
                
                context.SaveChanges();
            }
        }
    }
}
