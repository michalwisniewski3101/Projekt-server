using AutoMapper;
using Projekt_server.Entities;
using Projekt_server.Models;

namespace Projekt_server.MappingProfiles
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile() 
        {
           // CreateMap<Taskk, TaskDTO>();

           // CreateMap<Server, TaskDTO>()
             //   .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.Name));
           //CreateMap<App, TaskDTO>()
               // .ForMember(dest => dest.AppName, opt => opt.MapFrom(src => src.Name));



            CreateMap<(Taskk task, Server server, App app), TaskDTO>()
                .ForMember(sss => sss.ServerName, m => m.MapFrom(source => source.server.Name))
                .ForMember(sss => sss.AppName, m => m.MapFrom(source => source.app.Name))
                .ForMember(sss => sss.Name, m => m.MapFrom(source => source.task.Name))
                .ForMember(sss => sss.Id, m => m.MapFrom(source => source.task.Id))
                .ForMember(sss => sss.CreationDate, m => m.MapFrom(source => source.task.CreationDate))
                .ForMember(sss => sss.ModificationDate, m => m.MapFrom(source => source.task.ModificationDate));

            CreateMap<CreateTaskDTO, Taskk>();


        }
    }

}
