using AutoMapper;
using Projekt_server.Entities;
using Projekt_server.Models;

namespace Projekt_server.MappingProfiles
{
    public class ServerMappingProfile: Profile
    {

       
        
            public ServerMappingProfile()
            {
                // CreateMap<Taskk, TaskDTO>();

                // CreateMap<Server, TaskDTO>()
                //   .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.Name));
                //CreateMap<App, TaskDTO>()
                // .ForMember(dest => dest.AppName, opt => opt.MapFrom(src => src.Name));



                CreateMap<Server, ServerDTO>();
                CreateMap<CreateServerDTO, Server>();


            }
        
    }
}
