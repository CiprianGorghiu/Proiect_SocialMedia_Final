using Microsoft.EntityFrameworkCore;
using proiect_practica.Models;

namespace proiect_practica.Data
{
    public class DBcontext:DbContext
    {
        public DBcontext(DbContextOptions<DBcontext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FriendRequest>(f => 
            {
                f.HasKey(x => new { x.SenderId, x.ReciverId });

                f.HasOne(x => x.Sender)
                .WithMany(x => x.FriendRequestsSend)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

                f.HasOne(x => x.Reciver)
              .WithMany(x => x.FriendRequestsRecived)
              .HasForeignKey(x => x.ReciverId)
              .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<FriendShip>(f =>
            {
                f.HasKey(x => new { x.User1Id, x.User2Id });

                f.HasOne(x => x.User1)
                .WithMany(x => x.Friends)
                .HasForeignKey(x => x.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

                f.HasOne(x => x.User2)
              .WithMany(x => x.FriendsOf)
              .HasForeignKey(x => x.User2Id)
              .OnDelete(DeleteBehavior.Restrict);
            });
        }
      

        public DbSet<User> Users { get; set; }

        public DbSet<FriendRequest> FriendRequests { get; set; }

        public DbSet<FriendShip> FriendShips { get; set; }



    }
}
