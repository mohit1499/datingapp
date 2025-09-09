using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API.Controllers
{
    public class MembersController(AppDbContext context) : BaseApiController
    {
        [HttpGet]     
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await context.User.ToListAsync();
            return members;
        }


        [HttpGet("{id}")] // api/members/{id}             here id is route parameter not fixed path segemnt
        [Authorize]
        public async Task<ActionResult<AppUser>> GetMember(string id)
        {
            var member = await context.User.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }
            return member;
        }
        
    }
}
