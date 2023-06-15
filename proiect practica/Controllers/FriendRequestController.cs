using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proiect_practica.Data;
using proiect_practica.Models;
using proiect_practica.Models.DTO;

namespace proiect_practica.Controllers
{
    public class FriendRequestController : Controller
    {
        private readonly DBcontext context;

        public FriendRequestController(DBcontext context)
        {
            this.context = context;


        }

        [HttpGet("Get")]
        public IActionResult Get(int UserId)
        {
            try
            {
                var Friends = context.FriendRequests
                .Include(f => f.Sender)
                .Where(f => f.ReciverId == UserId)
                .Select(f => new RequestDTO
                {
                    ReciverId = f.ReciverId,
                    SenderId = f.SenderId,
                    NameSender = f.Sender.Name,
                    EmailSender = f.Sender.Email,


                })
                .ToList<RequestDTO>();

                return Ok(Friends);
            }
            catch (Exception e) { return StatusCode(StatusCodes.Status500InternalServerError, "Ceva nu a mers bine!"); }


        }
        [HttpPost("Send")]
        public IActionResult Send(int UserId, int ReciverId)
        {
            try
            {
                var FriendRequest = new FriendRequest
                {
                    SenderId = UserId,
                    ReciverId = ReciverId,
                    CreatedDate = DateTime.Now,


                };
                context.FriendRequests.Add(FriendRequest);
                context.SaveChanges();
                return Ok(FriendRequest);

            }
            catch (Exception e) { return StatusCode(StatusCodes.Status500InternalServerError, "Ceva nu a mers bine!"); }
        }
    }
}
