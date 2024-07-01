namespace Projekt_server.Entities
{
    public class Server
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; } 
        public required string IpAddress { get; set; }



        //public virtual List<Taskk>? Tasks { get; set; }

        //public virtual List<App>? Apps { get; set; }
    }
}
