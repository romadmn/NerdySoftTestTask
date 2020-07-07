using System;
using System.Collections.Generic;
using System.Text;
using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Application.MapperProfilers
{
    public class AnnouncementProfiler : Profile
    {
        public AnnouncementProfiler()
        {
            CreateMap<AnnouncementGetDto, Announcement>().ReverseMap()
                .ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0));
            CreateMap<AnnouncementPostDto, Announcement>().ReverseMap()
                .ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0));
            CreateMap<AnnouncementPutDto, Announcement>().ReverseMap()
                .ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0));
            CreateMap<AnnouncementDetailsDto, Announcement>().ReverseMap()
                .ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0));
        }
    }
}
