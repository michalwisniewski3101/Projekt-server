using AutoMapper;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_server.Data;
using Projekt_server.Entities;
using Projekt_server.Models;
using System.Threading.Tasks;

namespace Projekt_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ServerController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<ServerDTO>>> GetAllServers()
        {
            var servers = await _context.Servers.ToListAsync();
            var serversDTO = _mapper.Map<List<ServerDTO>>(servers);

            return Ok(serversDTO);
        }
    
    [HttpGet("{id}")]

        public async Task<ActionResult<Server>> Getserver(int id)
        {
            var server = await _context.Servers.FindAsync(id);
            if (server == null)
                return NotFound("Server not found");
            var serverDTO = _mapper.Map<ServerDTO>(server);



            return Ok(serverDTO);


        }

        [HttpPost]
        
        public async Task<ActionResult<List<Server>>> Addserver(CreateServerDTO serverDTO)
        {
            var server = _mapper.Map<Server>(serverDTO);

            server.CreationDate= DateTime.Now;
            server.ModificationDate = null;
          
            
            _context.Servers.Add(server);

            await _context.SaveChangesAsync();

            return Ok(await _context.Servers.ToListAsync());

        }

        [HttpPut]
        
        public async Task<ActionResult<List<Server>>> Updateserver(int id, CreateServerDTO serverDTO)
        {
            var dbServer = await _context.Servers.FindAsync(id);
            if (dbServer == null)
                return NotFound("server not found");

            dbServer.ModificationDate = DateTime.Now;

            dbServer.Name = serverDTO.Name;
            dbServer.IpAddress = serverDTO.IpAddress;
            



            await _context.SaveChangesAsync();

            return Ok(await _context.Servers.ToListAsync());


        }


        [HttpDelete]
       
        public async Task<ActionResult<Server>> Deleteserver(int id)
        {
            var dbServer = await _context.Servers.FindAsync(id);
            if (dbServer == null)
                return NotFound("Server not found");

            _context.Servers.Remove(dbServer);

            await _context.SaveChangesAsync();

            return Ok(await _context.Servers.ToListAsync());


        }
    }
}
