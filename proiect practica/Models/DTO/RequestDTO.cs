namespace proiect_practica.Models.DTO
{
    public class RequestDTO
    {
        public int ReciverId { get; set; }
        public int SenderId { get; set; }

        public string EmailSender { get; set; }
        public string NameSender { get; set; }
    }
}
