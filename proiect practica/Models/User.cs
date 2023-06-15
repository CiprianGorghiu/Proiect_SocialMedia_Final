using System.ComponentModel.DataAnnotations;

namespace proiect_practica.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Name { get; set; }

        public ICollection<FriendRequest>? FriendRequestsSend { get; set; }
        public ICollection<FriendRequest>? FriendRequestsRecived { get; set; }

        public ICollection<FriendShip>? FriendsOf { get; set; }
        public ICollection<FriendShip>? Friends { get; set; }

    }
}
