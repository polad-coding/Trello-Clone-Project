using System.Text.Json;

namespace TrelloClone.Core.Models
{
    public class ErrorDetailsModel
    {
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}