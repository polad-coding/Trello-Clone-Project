using System.Collections.Generic;
using System.Threading.Tasks;
using TrelloClone.Core.Models;

namespace TrelloClone.Core.Interfaces
{
    public interface IBoardService
    {
        Task<bool> CreateBoardAsync(BoardModel boardModel);
        List<string> GetBackgroundColorSchemes();
    }
}
