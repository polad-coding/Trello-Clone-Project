using System.Collections.Generic;

namespace TrelloClone.Core.Models
{
    public class ListModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int BoardId { get; set; }
        public BoardModel Board { get; set; }
        public List<ListCardModel> ListCardModels { get; set; }

    }
}
