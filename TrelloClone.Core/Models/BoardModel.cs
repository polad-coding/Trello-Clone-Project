using System.Collections.Generic;

namespace TrelloClone.Core.Models
{
    public class BoardModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ListModel> Lists { get; set; } = new List<ListModel>();
        public int TeamId { get; set; }
        public TeamModel Team { get; set; }
    }
}
