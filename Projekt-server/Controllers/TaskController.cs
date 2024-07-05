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


namespace Projekt_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TaskController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<TaskDTO>>> GetAllTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();
            var servers = await _context.Servers.ToListAsync();
            var apps = await _context.Apps.ToListAsync();


            var taskDTOs = tasks.Select(task =>
            {
                var server = servers.FirstOrDefault(s => s.Id == task.ServerId);
                var app = apps.FirstOrDefault(a => a.Id == task.AppId);
                return _mapper.Map<TaskDTO>((task, server, app));
            }).ToList();







            return Ok(taskDTOs);


        }
        [HttpGet("paginated")]
        public async Task<ActionResult<PagedResult<TaskDTO>>> GetTasks(int pageNumber, int pageSize)
        {
            var tasks = await _context.Tasks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        
            var servers = await _context.Servers.ToListAsync();
            var apps = await _context.Apps.ToListAsync();

            var taskDTOs = tasks.Select(task =>
            {
                var server = servers.FirstOrDefault(s => s.Id == task.ServerId);
                var app = apps.FirstOrDefault(a => a.Id == task.AppId);
                return _mapper.Map<TaskDTO>((task, server, app));
            }).ToList();

            var totalItems = await _context.Tasks.CountAsync();

            var result = new PagedResult<TaskDTO>
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = taskDTOs
            };

            return Ok(result);
        }


        [HttpGet("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var tasks = await _context.Tasks.ToListAsync();
            var servers = await _context.Servers.ToListAsync();
            var apps = await _context.Apps.ToListAsync();


            var taskDTOs = tasks.Select(task =>
            {
                var server = servers.FirstOrDefault(s => s.Id == task.ServerId);
                var app = apps.FirstOrDefault(a => a.Id == task.AppId);
                return _mapper.Map<TaskDTO>((task, server, app));
            }).ToList();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // Dodaj nagłówki
            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "Server Name";
            worksheet.Cells[1, 3].Value = "App Name";
            worksheet.Cells[1, 4].Value = "Creation Date";
            worksheet.Cells[1, 5].Value = "Modification Date";



            // Dodaj dane
            for (int i = 0; i < taskDTOs.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = taskDTOs[i].Name;
                worksheet.Cells[i + 2, 2].Value = taskDTOs[i].ServerName;
                worksheet.Cells[i + 2, 3].Value = taskDTOs[i].AppName;
                worksheet.Cells[i + 2, 4].Value = taskDTOs[i].CreationDate;
                worksheet.Cells[i + 2, 4].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                worksheet.Cells[i + 2, 5].Value = taskDTOs[i].ModificationDate;
                worksheet.Cells[i + 2, 5].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            var fileName = $"Export_Tasks_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<TaskDTO>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound("Task not found");

            var server = await _context.Servers.FindAsync(task.ServerId);
            var app = await _context.Apps.FindAsync(task.AppId);

            

            var taskDTO = _mapper.Map<TaskDTO>((task, server, app));
            return Ok(taskDTO);


        }

        [HttpPost]
        //dto chat
        public async Task<ActionResult<List<Taskk>>> AddTask(CreateTaskDTO taskDTO)
        {
            var task = _mapper.Map<Taskk>(taskDTO);

            task.CreationDate = DateTime.Now;
            task.ModificationDate = null;


            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            return Ok(await _context.Tasks.ToListAsync());

        }

        [HttpPut]
        //dfo chat
        public async Task<ActionResult<List<Taskk>>> UpdateTask(int id, CreateTaskDTO taskDTO)
        {
            var dbTask = await _context.Tasks.FindAsync(id);
            if (dbTask == null)
                return NotFound("Task not found");
            
            dbTask.ModificationDate = DateTime.Now;

            dbTask.Name = taskDTO.Name;
            dbTask.AppId= taskDTO.AppId;
            dbTask.ServerId = taskDTO.ServerId;

            await _context.SaveChangesAsync();
            return Ok(await _context.Tasks.ToListAsync());
        }


        [HttpDelete]
        //dfo chat
        public async Task<ActionResult<Taskk>> DeleteTask(int id)
        {
            var dbTask = await _context.Tasks.FindAsync(id);
            if (dbTask == null)
                return NotFound("Task not found");

            _context.Tasks.Remove(dbTask);

            await _context.SaveChangesAsync();

            return Ok(await _context.Tasks.ToListAsync());


        }
    }
}
