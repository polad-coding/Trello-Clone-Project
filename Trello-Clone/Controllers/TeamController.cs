using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TrelloClone.Core.Interfaces;
using TrelloClone.Core.Models;

namespace TrelloClone.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeamController : ControllerBase
    {

        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost]
        [Route("CreateTeamAsync")]
        public async Task<ActionResult> CreateTeamAsync([FromBody]TeamModel teamModel)
        {
            var isCreated = await _teamService.CreateTeamAsync(teamModel);
            if (isCreated)
            {
                return Ok(teamModel);
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("AssociateUserWithTeamAsync")]
        public async Task<ActionResult> AssociateUserWithTeamAsync([FromBody]int teamId)
        {
            var isAssociated = await _teamService.AssociateUserWithTeamAsync(teamId, User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (isAssociated)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
