using System.Collections.Generic;

namespace TrelloClone.Core.Models
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public string TeamDescription { get; set; }
        public List<BoardModel> Boards { get; set; } = new List<BoardModel>();
        public List<TeamUserModel> TeamUserModels { get; set; } = new List<TeamUserModel>();
    }
}
