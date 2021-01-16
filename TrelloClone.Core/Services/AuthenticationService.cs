using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using TrelloClone.Core.Constants;
using TrelloClone.Core.DTOs;
using TrelloClone.Core.Interfaces;
using TrelloClone.Core.Models;

namespace TrelloClone.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Get the random color code of the avatar for the user.
        /// </summary>
        /// <returns></returns>
        private string GetRandomAvatarColorCode()
        {
            var random = new Random();
            string colorCode = UserDefaultAvatarColorSchemes.colorSchemes[random.Next(0, UserDefaultAvatarColorSchemes.colorSchemes.Count)];
            return colorCode;
        }

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="newUser">DTO of the user to be created.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Check if user exists by email address.
        /// </summary>
        /// <param name="mail">Email address to check.</param>
        /// <returns></returns>
        public async Task<bool> ExistsByEmailAsync(string mail)
        {
            var result = await _userManager.FindByEmailAsync(mail);

            if (result != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if user exists by user name.
        /// </summary>
        /// <param name="username">User name to check.</param>
        /// <returns></returns>
        public async Task<bool> ExistsByUserNameAsync(string username)
        {
            var result = await _userManager.FindByNameAsync(username);

            if (result != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get user by then given id.
        /// </summary>
        /// <param name="id">Id of the user to get.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Log in the given user.
        /// </summary>
        /// <param name="user">User to log in.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Log out the current user.
        /// </summary>
        /// <returns></returns>
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
    }
}
