using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrelloClone.Core.Interfaces;
using TrelloClone.Core.Models;

namespace TrelloClone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class BoardController : ControllerBase
    {

        private readonly IBoardService _boardService;

        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpPost]
        [Route("CreateBoardAsync")]
        public async Task<ActionResult> CreateBoardAsync([FromBody]BoardModel boardModel)
        {
            var isCreated = await _boardService.CreateBoardAsync(boardModel);

            if (isCreated)
            {
                return Ok(boardModel);
            }

            return StatusCode(500);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<List<string>> GetBackgroundColorSchemes()
        {
            var colorSchemes = _boardService.GetBackgroundColorSchemes();

            if (colorSchemes != null)
            {
                return Ok(colorSchemes);
            }

            return StatusCode(500);
        }

    }
}
