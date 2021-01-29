using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TrelloClone.Core.DTOs;
using TrelloClone.Core.Interfaces;

namespace TrelloClone.WebUI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [Route("[action]/{mail}")]
        public async Task<ActionResult<bool>> ExistsByEmailAsync(string mail)
        {
            var result = await _authenticationService.ExistsByEmailAsync(mail);
            return new JsonResult(result);
        }

        [HttpGet]
        [Route("[action]/{username}")]
        public async Task<ActionResult<bool>> ExistsByUserNameAsync(string username)
        {
            var result = await _authenticationService.ExistsByUserNameAsync(username);

            return new JsonResult(result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ApplicationUserDTO>> CreateNewUserAsync([FromBody] ApplicationUserDTO newUser)
        {
            var createdUser = await _authenticationService.CreateNewUserAsync(newUser);

            if (createdUser != null)
            {
                return Ok(createdUser);
            }

            return StatusCode(500);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ApplicationUserDTO>> LogInUserAsync([FromBody] ApplicationUserDTO user)
        {
            var logInSucceeded = await _authenticationService.LogInUserAsync(user);

            if (logInSucceeded)
            {
                return Ok(user);
            }

            return StatusCode(500);
        }

        /// <summary>
        /// Build and return Jwt token for the current user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public ActionResult GetJwtToken(ApplicationUserDTO user)
        {
            var token = _authenticationService.GetJwtToken(user);

            if (token != null)
            {
                return Ok(token);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ApplicationUserDTO>> GetUserByIdAsync(string id)
        {
            var result = await _authenticationService.GetUserByIdAsync(id);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult<ApplicationUserDTO>> GetCurrentUser()
        {
            var currentUser = await _authenticationService.GetUserByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(currentUser);
        }

        [HttpPost]
        [Route("LogOutUser")]
        [Authorize]
        public async Task<ActionResult> LogOutAsync()
        {
            await _authenticationService.LogOutAsync();
            return Ok();
        }

    }
}
