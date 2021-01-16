using System.Collections.Generic;

namespace TrelloClone.Core.Models
{
    public class CardModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NumberOfCaptions { get; set; }
        public int NumberOfAttachements { get; set; }
        public List<ListCardModel> ListCardModels { get; set; }
    }
}
