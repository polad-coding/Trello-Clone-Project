using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TrelloClone.Core.Interfaces;
using TrelloClone.Core.Models;

namespace TrelloClone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeamController : ControllerBase
    {

        private ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        /// <summary>
        /// Create new team.
        /// </summary>
        /// <param name="teamModel">New team to create.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Associate current user with the given team.
        /// </summary>
        /// <param name="teamId">Team id to associate with.</param>
        /// <returns></returns>
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
