namespace Projekt_server.Models
{
    public class ServerDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public required string IpAddress { get; set; }
    }
}
