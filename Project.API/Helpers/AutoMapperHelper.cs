using AutoMapper;
using Project.API.Dtos;
using Project.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Project.API.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {

            CreateMap<Users, UserListDto>()
                .ForMember(dest => dest.photourl,opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url);
                })
                .ForMember(dest => dest.Age,opt =>
                {
                    opt.MapFrom(src => src.DateofBirth != null ? ((int.Parse(DateTime.Now.ToString("yyyyMMdd")) - int.Parse(src.DateofBirth.Value.ToString("yyyyMMdd")))/10000) : 0);
                });

            CreateMap<Users, UserLoginResponseDto>()
               .ForMember(dest => dest.photourl, opt =>
               {
                   opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url);
               });
              
            CreateMap<Photos, PhotoDto>().ReverseMap();
            
            CreateMap<UserUpdateDto,Users>();
            
            CreateMap<UserRegisterDto, Users>();
            
            CreateMap<MessageCreateDto, Messages>().ReverseMap();

            CreateMap<Messages, MessageReturnDto>()
                .ForMember(dest => dest.SenderUrl, opt =>
                   {
                       opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(x => x.IsMain).Url);
                   })
                .ForMember(dest => dest.ReceiverUrl, opt =>
                    {
                        opt.MapFrom(u => u.Receiver.Photos.FirstOrDefault(x => x.IsMain).Url);
                    }); 

        }
    }
}
