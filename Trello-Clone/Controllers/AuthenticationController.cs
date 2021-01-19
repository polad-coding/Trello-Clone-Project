using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TrelloClone.Core.DTOs;
using TrelloClone.Core.Interfaces;

namespace TrelloClone.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService authenticationService { get; set; }

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// Check if user exists by email address.
        /// </summary>
        /// <param name="mail">Email address to check.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{mail}")]
        public async Task<ActionResult<bool>> ExistsByEmailAsync(string mail)
        {
            var result = await authenticationService.ExistsByEmailAsync(mail);
            return new JsonResult(result);
        }

        /// <summary>
        /// Check if user exists by user name.
        /// </summary>
        /// <param name="username">User name to check.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{username}")]
        public async Task<ActionResult<bool>> ExistsByUserNameAsync(string username)
        {
            var result = await authenticationService.ExistsByUserNameAsync(username);

            return new JsonResult(result);
        }

        /// <summary>
        /// Create and log in new user.
        /// </summary>
        /// <param name="newUser">New user to create.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ApplicationUserDTO>> CreateNewUserAsync([FromBody] ApplicationUserDTO newUser)
        {
            var createdUser = await authenticationService.CreateNewUserAsync(newUser);

            if (createdUser != null)
            {
                return Ok(createdUser);
            }

            return StatusCode(500);
        }

        /// <summary>
        /// Log in the given user.
        /// </summary>
        /// <param name="newUser">User to log in.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ApplicationUserDTO>> LogInUserAsync([FromBody] ApplicationUserDTO user)
        {
            var logInSucceeded = await authenticationService.LogInUserAsync(user);

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
            var token = authenticationService.GetJwtToken(user);

            if (token != null)
            {
                return Ok(token);
            }

            return BadRequest();
        }

        ///// <summary>
        ///// Verify that user is authentucated.
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("[action]")]
        //public ActionResult UserIsAuthenticated()
        //{
        //    var isAuthenticated = User.Identity.IsAuthenticated;
        //    return new JsonResult(isAuthenticated);
        //}

        /// <summary>
        /// Get user by then given id.
        /// </summary>
        /// <param name="id">Id of the user to get.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ApplicationUserDTO>> GetUserByIdAsync(string id)
        {
            var result = await authenticationService.GetUserByIdAsync(id);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        /// <summary>
        /// Get current signed id user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult<ApplicationUserDTO>> GetCurrentUser()
        {
            var currentUser = await authenticationService.GetUserByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(currentUser);
        }
    }
}
