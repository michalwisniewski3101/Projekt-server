namespace Projekt_server.Entities
{
    public class Taskk
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public required int ServerId { get; set; }
        public int? AppId { get; set; }



        // Właściwość nawigacyjna do klasy Server
        public virtual required Server Server { get; set; }

        // Właściwość nawigacyjna do klasy App
        public virtual App? App { get; set; }


        //public virtual required Server Server { get; set; }
        //public virtual App? App { get; set; }
    }
}
