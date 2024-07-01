using AutoMapper;
using Projekt_server.Entities;
using Projekt_server.Models;

namespace Projekt_server.MappingProfiles
{

        public class AppMappingProfile : Profile
        {
            public AppMappingProfile()
            {
                // CreateMap<Taskk, TaskDTO>();

                // CreateMap<Server, TaskDTO>()
                //   .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.Name));
                //CreateMap<App, TaskDTO>()
                // .ForMember(dest => dest.AppName, opt => opt.MapFrom(src => src.Name));



                CreateMap<(App app, Server server ), AppDTO>()
                    .ForMember(sss => sss.ServerName, m => m.MapFrom(source => source.server.Name))
                    .ForMember(sss => sss.Name, m => m.MapFrom(source => source.app.Name))
                    .ForMember(sss => sss.Id, m => m.MapFrom(source => source.app.Id))
                    .ForMember(sss => sss.CreationDate, m => m.MapFrom(source => source.app.CreationDate))
                    .ForMember(sss => sss.ModificationDate, m => m.MapFrom(source => source.app.ModificationDate));

            CreateMap<CreateAppDTO, App>();


        }
        }

    
}
