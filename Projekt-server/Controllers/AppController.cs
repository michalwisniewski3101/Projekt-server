using AutoMapper;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_server.Data;
using Projekt_server.Entities;
using Projekt_server.Models;
using System.Threading.Tasks;

namespace Projekt_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AppController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<App>>> GetAllApps()
        {
            var servers = await _context.Servers.ToListAsync();
            var apps = await _context.Apps.ToListAsync();

            var appDTOs = apps.Select(app =>
            {
                var server = servers.FirstOrDefault(s => s.Id == app.ServerId);
                return _mapper.Map<AppDTO>((app, server ));
            }).ToList();





            return Ok(appDTOs);


        }
        [HttpGet("{id}")]

        public async Task<ActionResult<App>> GetApp(int id)
        {
            var app = await _context.Apps.FindAsync(id);

            if (app == null)
                return NotFound("App not found");

            var server = await _context.Servers.FindAsync(app.ServerId);
            



            var appDTO = _mapper.Map<AppDTO>((app, server));
            return Ok(appDTO);


        }

        [HttpPost]
        public async Task<ActionResult<List<App>>> AddApp(CreateAppDTO appDTO)
        {
            var app = _mapper.Map<App>(appDTO);

            app.CreationDate = DateTime.Now;
            app.ModificationDate = null;


            _context.Apps.Add(app);

            await _context.SaveChangesAsync();

            return Ok(await _context.Apps.ToListAsync());

        }

        [HttpPut]
       
        public async Task<ActionResult<List<App>>> UpdateApp(int id, CreateAppDTO appDTO)
        {
            var dbApp = await _context.Apps.FindAsync(id);
            if (dbApp == null)
                return NotFound("App not found");

            dbApp.ModificationDate = DateTime.Now;

            dbApp.Name = appDTO.Name;
            dbApp.ServerId = appDTO.ServerId;


            await _context.SaveChangesAsync();

            return Ok(await _context.Apps.ToListAsync());


        }


        [HttpDelete]
        public async Task<ActionResult<App>> DeleteApp(int id)
        {
            var dbApp = await _context.Apps.FindAsync(id);
            if (dbApp == null)
                return NotFound("App not found");

            _context.Apps.Remove(dbApp);

            await _context.SaveChangesAsync();

            return Ok(await _context.Apps.ToListAsync());


        }
    }
}
