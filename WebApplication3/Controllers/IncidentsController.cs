using WebApplication3.DB;
using WebApplication3.dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApplication3.Controllers
{
        [Authorize]
        [Route("api/[controller]")]
        [ApiController]
        public class IncidentsController : ControllerBase
        {
            private readonly ItCompany1135Context db;

            public IncidentsController(ItCompany1135Context db)
            {
                this.db = db;
            }
         
            [HttpPost("Report")]
            public  ActionResult AddIncident()
            {
                var client = GetClient();
                if(client == null)
                    return Forbid();
              //  var incident = GetIncident(client.id);
              
                db.SaveChanges();
                return Ok();
            }


        }
        }
    }

