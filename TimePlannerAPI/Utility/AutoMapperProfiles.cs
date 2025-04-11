using AutoMapper;
using TimePlannerAPI.DTOs;
using TimePlannerAPI.Models;

namespace TimePlannerAPI.Utility
{
    public class AutoMapperProfiles : Profile
    {
        // This is the constructor of the class
        public AutoMapperProfiles()
        {

            // Create mappings here

            CreateMap<CreateScheduleDto, Schedule>()
                .ForMember(dest => dest.Id, opt=> opt.Ignore())// Ignore Id when creating a new schedule
            .ForMember(dest => dest.CreatedDate, opt=> opt.Ignore()) // Ignore CreatedDate when creating a new schedule
            .ForMember(dest => dest.Users, opt => opt.Ignore()) // Ignore Users when creating a new schedule
                .ForMember(dest => dest.TimeBlocks, opt => opt.Ignore());// Ignore TimeBlocks when creating a new schedule


            CreateMap<Schedule, ScheduleDto>()
                .ForMember(dest => dest.TimeBlocks,
                opt => opt.MapFrom(src => src.TimeBlocks.Count));


            CreateMap<TimeBlockDto, TimeBlock>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Schedule, opt => opt.Ignore());
                


                CreateMap<TimeBlock, TimeBlockDto>()
                .ForMember(dest => dest.DurationMinutes,
                opt => opt.MapFrom(src => (src.EndTime - src.StartTime).TotalMinutes));





            //CreateMap<User, UserDTO>(); //Create map from Genre to CreateGenreDTO

            //CreateMap<createUserDTO, User>(); //Create map from CreateUserDTO to User




            // In your AutoMapper profile configuration
            CreateMap<TimeBlock, TimeBlockDto>()
                .ForMember(dest => dest.DurationMinutes,
                    opt => opt.MapFrom(src => (src.EndTime - src.StartTime).TotalMinutes));

            CreateMap<CreateTimeBlockDto, TimeBlock>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.EndTime, opt => opt.Ignore());
               // .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<UpdateTimeBlockDto, TimeBlock>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StartTime, opt => opt.Ignore())
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(_ => DateTime.UtcNow))
                //.ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.ScheduleId, opt => opt.Ignore());

        }
    }
}
