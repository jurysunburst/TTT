using System.Web;
using System.Web.Http;
using TTT.Web.Enums;
using TTT.Web.Model;
using TTT.Web.Services;

namespace TTT.Web.Controllers
{
	public class BoardMoveController : ApiController
	{
		// GET api/BoardMove
		public BoardModel Get()
		{
			var boardService = new BoardService(HttpContext.Current.Session);
			return new BoardModel()
			{
				Moves = boardService.BoardValues,
				Status = boardService.GetSatusSession()
			};
		}

		// POST api/BoardMove
		public void Post([FromBody]string value)
		{
			var boardService = new BoardService(HttpContext.Current.Session);
			boardService.MakeMove(int.Parse(value), Marks.X);

			if (boardService.CheckCurrentStatus() == (int)Statuses.None)
			{
				boardService.MakeAIMove(Marks.O);
			}
			boardService.SaveSessionValues();
		}

		// DELETE api/BoardMove
		public void Delete()
		{
			var boardService = new BoardService(HttpContext.Current.Session);
			boardService.ClearBoard();
		}
	}
}