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
        [HttpPost("ViewCart")]
        public ActionResult<IEnumerable<IncidentDTO>> ListIncident()
        {
            var client = GetClient();
            if (client == null)
                return Forbid();

            var cart = GetIncident(client.Sid);

            var result = client.Incidents.Select(s => new IncidentDTO
            {
             Id = s.Id,
             UserSid = s.UserSid,
             Title = s.Title,
             Description = s.Description,
             Priority = s.Priority,
             Status = s.Status,
             AssignedToSid = s.AssignedToSid,
             ResolvedAt = s.ResolvedAt,
             ResolutionNotes = s.ResolutionNotes,
             IsDeleted = s.IsDeleted,
             DeletedAt = s.DeletedAt,
             DeletedBy = s.DeletedBy,
             CreatedAt = s.CreatedAt,
             CreatedBy = s.CreatedBy,
             ModifiedAt = s.ModifiedAt,
             ModifiedBy = s.ModifiedBy,
             UserS = s.UserS,

            });
            return Ok(result);

        }
        [HttpPost("AddIncident")]
        public async Task<ActionResult> AddIncident(IncidentDTO incidentDTO)
        {
            var client = GetClient();
            if (client == null)
                return Forbid();

            var incident = GetIncident(client.Sid);
            client.Incidents.Add(new Incident 
            { 
            Id = incident.Id,
            UserSid = incident.UserSid,
            Title = incident.Title,
            Description = incident.Description,
            Priority = incident.Priority,
            Status = incident.Status,
            AssignedToSid = incident.AssignedToSid,
            ResolvedAt = incident.ResolvedAt,
            ResolutionNotes = incident.ResolutionNotes,
            IsDeleted = incident.IsDeleted,
            DeletedAt = incident.DeletedAt,
            DeletedBy = incident.DeletedBy,
            CreatedAt = incident.CreatedAt,
            CreatedBy = incident.CreatedBy,
            ModifiedAt = incident.ModifiedAt,
            ModifiedBy = incident.ModifiedBy,
            UserS = incident.UserS,

            });
            db.SaveChanges();
            return Ok();
        }



        Client? GetClient()
        {
            var claim = User.Claims.First();
            if (claim.Type != ClaimValueTypes.Sid)
                return null;

            var client = db.Clients.Find(claim.Value);
            if (client == null)
                return null;

            return client;
        }
        Incident? GetIncident(string clientId)
        {
            var incidents = db.Clients.
                    Include(s => s.Incidents).
                    ThenInclude(s => s.UserS).
                    FirstOrDefault(s => s.Sid == clientId &&
                        s.Incidents.Status == "Новая");

            if (incidents == null)
            {
                incidents = new Incident
                {
                    AssignedToSid = Guid.NewGuid().ToString(),
                    UserSid = clientId,
                    Status = "Новая"
                };
                db.Incidents.Add(incidents);
                db.SaveChanges();
            }
            return incidents;
        }

    }
}

    

