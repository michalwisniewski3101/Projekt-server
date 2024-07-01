namespace Projekt_server.Entities
{
    public class App
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public required int ServerId { get; set; }
        
        //public virtual required Server Server { get; set; }

        //public virtual List<Taskk>? Tasks { get; set; }
    }
}
