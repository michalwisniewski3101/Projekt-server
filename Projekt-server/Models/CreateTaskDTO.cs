namespace Projekt_server.Models
{
    public class CreateTaskDTO
    {
        public required string Name { get; set; }
        public required int ServerId { get; set; }
        public int? AppId { get; set; }
    }
}
