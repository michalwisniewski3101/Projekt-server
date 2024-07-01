namespace Projekt_server.Models
{
    public class AppDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public required string ServerName { get; set; }
    }
}
