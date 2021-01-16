using System.Threading.Tasks;
using TrelloClone.Core.Interfaces;
using TrelloClone.Core.Models;

namespace TrelloClone.Core.Services
{
    public class TeamService : ITeamService
    {
        private IApplicationDbContext _applicationDbContext;

        public TeamService(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Associate current user with the given team.
        /// </summary>
        /// <param name="teamId">Team id to associate with.</param>
        /// <param name="userId">Id of the current user.</param>
        /// <returns></returns>
        public async Task<bool> AssociateUserWithTeamAsync(int teamId, string userId)
        {
            var team = await _applicationDbContext.Teams.FindAsync(teamId);
            team.TeamUserModels.Add(new TeamUserModel { TeamId = team.Id, UserId = userId });

            var rowsAffected = await _applicationDbContext.SaveAsync();

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Create new team.
        /// </summary>
        /// <param name="teamModel">New team to create.</param>
        /// <returns></returns>
        public async Task<bool> CreateTeamAsync(TeamModel teamModel)
        {
            await _applicationDbContext.Teams.AddAsync(teamModel);
            var affected = await _applicationDbContext.SaveAsync();

            if (affected > 0)
            {
                return true;
            }

            return false;
        }
    }
}
