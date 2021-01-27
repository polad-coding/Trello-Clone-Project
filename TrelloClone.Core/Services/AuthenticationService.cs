using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrelloClone.Core.Constants;
using TrelloClone.Core.DTOs;
using TrelloClone.Core.Interfaces;
using TrelloClone.Core.Models;

namespace TrelloClone.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Get the random color code of the avatar for the user.
        /// </summary>
        /// <returns></returns>
        private string GetRandomAvatarColorCode()
        {
            var random = new Random();
            var colorCode = UserDefaultAvatarColorSchemes.colorSchemes[random.Next(0, UserDefaultAvatarColorSchemes.colorSchemes.Count)];
            return colorCode;
        }

        public async Task<ApplicationUserDTO> CreateNewUserAsync(ApplicationUserDTO newUser)
        {
            var applicationUser = new ApplicationUser();
            newUser.AvatarColorCode = GetRandomAvatarColorCode();
            TransferDTOdataToApplicationUserEntity(newUser, applicationUser);
            var result = await _userManager.CreateAsync(applicationUser, newUser.Password);

            if (result.Succeeded)
            {
                newUser.Id = applicationUser.Id;
                return newUser;
            }

            return null;
        }

        public async Task<bool> ExistsByEmailAsync(string mail)
        {
            var result = await _userManager.FindByEmailAsync(mail);

            if (result != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ExistsByUserNameAsync(string username)
        {
            var result = await _userManager.FindByNameAsync(username);

            if (result != null)
            {
                return true;
            }

            return false;
        }

        public async Task<ApplicationUserDTO> GetUserByIdAsync(string id)
        {
            var result = await _userManager.FindByIdAsync(id);

            if (result != null)
            {
                var dto = new ApplicationUserDTO();
                TransferApplicationUserEntityDataToDTO(dto, result);
                return dto;
            }

            return null;
        }

        public async Task<bool> LogInUserAsync(ApplicationUserDTO user)
        {
            var unauthorizedUser = await _userManager.FindByEmailAsync(user.Email);
            var logInSucceeded = _signInManager.PasswordSignInAsync(unauthorizedUser, user.Password, false, false).Result.Succeeded;

            if (logInSucceeded)
            {
                TransferApplicationUserEntityDataToDTO(user, unauthorizedUser);
                user.Password = String.Empty;
                return true;
            }

            return false;
        }

        public bool LogOutUser()
        {
            var logOutSucceeded = _signInManager.SignOutAsync().IsCompleted;

            return logOutSucceeded;
        }

        /// <summary>
        /// Transfer data from ApplicationUser entity to the respective DTO.
        /// </summary>
        /// <param name="userDTO"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private ApplicationUserDTO TransferApplicationUserEntityDataToDTO(ApplicationUserDTO userDTO, ApplicationUser user)
        {
            userDTO.Id = user.Id;
            userDTO.AvatarColorCode = user.AvatarColorCode;
            userDTO.Bio = user.Bio;
            userDTO.Email = user.Email;
            userDTO.FullName = user.FullName;
            userDTO.Username = user.UserName;

            return userDTO;
        }

        /// <summary>
        /// Transfer data from DTO to the respective ApplicationUser entity.
        /// </summary>
        /// <param name="userDTO"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private ApplicationUser TransferDTOdataToApplicationUserEntity(ApplicationUserDTO userDTO, ApplicationUser user)
        {
            user.AvatarColorCode = userDTO.AvatarColorCode;
            user.Bio = userDTO.Bio;
            user.Email = userDTO.Email;
            user.FullName = userDTO.FullName;
            user.UserName = userDTO.Username;

            return user;
        }

        /// <summary>
        /// Create Jwt token for the given user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetJwtToken(ApplicationUserDTO user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToInt32(_configuration.GetSection("JwtExpireDays").Value));

            var tokenTemplate = new JwtSecurityToken(
                "MySite",
                "MySite",
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenTemplate);
            var jsonFormatedToken = JsonSerializer.Serialize(token);

            return jsonFormatedToken != null ? jsonFormatedToken : null;
        }
    }
}
