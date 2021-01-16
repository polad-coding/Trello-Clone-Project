namespace TrelloClone.Core.Models
{
    public class TeamUserModel
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int TeamId { get; set; }
        public TeamModel Team { get; set; }
    }
}
