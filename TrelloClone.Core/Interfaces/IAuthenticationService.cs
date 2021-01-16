using System.Threading.Tasks;
using TrelloClone.Core.DTOs;

namespace TrelloClone.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> ExistsByEmailAsync(string mail);
        Task<bool> ExistsByUserNameAsync(string username);
        Task<ApplicationUserDTO> CreateNewUserAsync(ApplicationUserDTO newUser);
        Task<bool> LogInUserAsync(ApplicationUserDTO user);
        bool LogOutUser();
        Task<ApplicationUserDTO> GetUserByIdAsync(string id);
    }
}
