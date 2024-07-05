using AutoMapper;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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

        [HttpGet("paginated")]
        public async Task<ActionResult<PagedResult<ServerDTO>>> GetServers(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Servers.CountAsync();
            var servers = await _context.Servers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var serversDTO = _mapper.Map<List<ServerDTO>>(servers);

            var result = new PagedResult<ServerDTO>
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = serversDTO
            };

            return Ok(result);
        }


        [HttpGet("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            var servers = await _context.Servers.ToListAsync();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // Dodaj nagłówki
            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "IP Address";
            worksheet.Cells[1, 3].Value = "Creation Date";
            worksheet.Cells[1, 4].Value = "Modification Date";



            // Dodaj dane
            for (int i = 0; i < servers.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = servers[i].Name;
                worksheet.Cells[i + 2, 2].Value = servers[i].IpAddress;
                worksheet.Cells[i + 2, 3].Value = servers[i].CreationDate;
                worksheet.Cells[i + 2, 3].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                worksheet.Cells[i + 2, 4].Value = servers[i].ModificationDate;
                worksheet.Cells[i + 2, 4].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            var fileName = $"Export_Servers_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        




        [HttpGet("GetServerFilterData")]

         public async Task<ActionResult<List<ServerFilter>>> GetServerFilterData()
         {
             var SfilterData = await _context.ServerFilterView.Select(f => new { f.Id, f.Name }).ToListAsync();
             return Ok(SfilterData);
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

            // Check if any applications or tasks are associated with this server
            var associatedApps = await _context.Apps.Where(app => app.ServerId == id).ToListAsync();
            var associatedTasks = await _context.Tasks.Where(task => task.ServerId == id).ToListAsync();

            if (associatedApps.Any())
                _context.Apps.RemoveRange(associatedApps);

            if (associatedTasks.Any())
                _context.Tasks.RemoveRange(associatedTasks);

            _context.Servers.Remove(dbServer);

            await _context.SaveChangesAsync();

            return Ok(await _context.Servers.ToListAsync());


        }

    }
}
