using AutoMapper;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
