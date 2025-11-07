using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication3.DB;
using WebApplication3.dto;

namespace WebApplication3.Controllers
{
    public class IncidentsController
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly ItCompany1135Context db;

            public AuthController(ItCompany1135Context db)
            {
                this.db = db;
            }
         
            [HttpPost("Report")]
            public  ActionResult AddIncident()
            {
               // var client = GetClient();
              //  if(client == null)
                    return Forbid();
              //  var incident = GetIncident(client.id);
              
                db.SaveChanges();
                return Ok();
            }

        }
        }
    }
}
