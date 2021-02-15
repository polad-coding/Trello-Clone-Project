using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrelloClone.Core.Interfaces;
using TrelloClone.Core.Models;
using TrelloClone.Core.Settings;

namespace TrelloClone.Core.Services
{
    public class BoardService : IBoardService
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ColorSchemesSettings _colorSchemesSettings;

        public BoardService(IApplicationDbContext applicationDbContext, IOptions<ColorSchemesSettings> colorSchemes)
        {
            _applicationDbContext = applicationDbContext;
            _colorSchemesSettings = colorSchemes.Value;
        }

        public async Task<bool> CreateBoardAsync(BoardModel boardModel)
        {
            await _applicationDbContext.Boards.AddAsync(boardModel);
            var rowsAffected = await _applicationDbContext.SaveAsync();

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public List<string> GetBackgroundColorSchemes()
        {
            return _colorSchemesSettings.SchemesSetting;
        }
    }
}
