using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrelloClone.Core.Settings;
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
        private readonly IMapper _mapper;
        private readonly Random _random;
        private readonly ColorSchemesSettings _colorSchemes;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IConfiguration configuration, 
            IMapper mapper, 
            Random random,
            IOptions<ColorSchemesSettings> colorSchemes
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
            _random = random;
            _colorSchemes = colorSchemes.Value;
        }

        /// <summary>
        /// Get the random color code of the avatar for the user.
        /// </summary>
        /// <returns></returns>
        private string GetRandomAvatarColorCode()
        {
            var randomIndex = _random.Next(0, _colorSchemes.SchemesSetting.Count);
            var colorCode = _colorSchemes.SchemesSetting[randomIndex];
            return colorCode;
        }

        public async Task<ApplicationUserDTO> CreateNewUserAsync(ApplicationUserDTO newUser)
        {
            newUser.AvatarColorCode = GetRandomAvatarColorCode();
            var applicationUser = _mapper.Map<ApplicationUser>(newUser);
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
                var dto = _mapper.Map<ApplicationUserDTO>(result);
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
                _mapper.Map(unauthorizedUser, user);
                user.Password = string.Empty;
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
