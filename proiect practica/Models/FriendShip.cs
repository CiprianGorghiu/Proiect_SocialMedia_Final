namespace proiect_practica.Models
{
    public class FriendShip
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public User User1 { get; set; }
        public User User2 { get; set; }
    }
}
