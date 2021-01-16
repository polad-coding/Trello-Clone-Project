using System.Threading.Tasks;
using TrelloClone.Core.Models;

namespace TrelloClone.Core.Interfaces
{
    public interface ITeamService
    {
        Task<bool> CreateTeamAsync(TeamModel teamModel);
        Task<bool> AssociateUserWithTeamAsync(int teamId, string userId);
    }
}
