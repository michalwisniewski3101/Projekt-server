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
        [HttpGet("paginated")]
        public async Task<ActionResult<PagedResult<AppDTO>>> GetApps(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Apps.CountAsync();
            var apps = await _context.Apps
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var servers = await _context.Servers.ToListAsync();

            var appDTOs = apps.Select(app =>
            {
                var server = servers.FirstOrDefault(s => s.Id == app.ServerId);
                return _mapper.Map<AppDTO>((app, server));
            }).ToList();

            var result = new PagedResult<AppDTO>
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = appDTOs
            };

            return Ok(result);
        }





        [HttpGet("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var servers = await _context.Servers.ToListAsync();
            var apps = await _context.Apps.ToListAsync();

            var appDTOs = apps.Select(app =>
            {
                var server = servers.FirstOrDefault(s => s.Id == app.ServerId);
                return _mapper.Map<AppDTO>((app, server));
            }).ToList();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // Dodaj nagłówki
            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "ServerName";
            worksheet.Cells[1, 3].Value = "Creation Date";
            worksheet.Cells[1, 4].Value = "Modification Date";



            // Dodaj dane
            for (int i = 0; i < appDTOs.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = appDTOs[i].Name;
                worksheet.Cells[i + 2, 2].Value = appDTOs[i].ServerName;
                worksheet.Cells[i + 2, 3].Value = appDTOs[i].CreationDate;
                worksheet.Cells[i + 2, 3].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                worksheet.Cells[i + 2, 4].Value = appDTOs[i].ModificationDate;
                worksheet.Cells[i + 2, 4].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            var fileName = $"Export_Apps_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }





        [HttpGet("GetAppFilterData")]
        public async Task<ActionResult<List<AppFilter>>> GetAppFilterData()
        {
            var AfilterData = await _context.AppFilterView.Select(f => new { f.Id, f.Name }).ToListAsync();
            return Ok(AfilterData);
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
