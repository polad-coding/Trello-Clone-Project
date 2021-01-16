namespace TrelloClone.Core.Models
{
    public class ListCardModel
    {
        public int ListId { get; set; }
        public ListModel List { get; set; }
        public int CardId { get; set; }
        public CardModel Card { get; set; }
    }
}
