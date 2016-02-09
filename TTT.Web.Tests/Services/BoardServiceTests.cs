using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TTT.Web.Tests.Session;
using TTT.Web.Enums;

namespace TTT.Web.Services.Tests
{
	[TestClass()]
	public class BoardServiceTests
	{
		[TestMethod()]
		public void GetSatusSessionTest()
		{
			var session = FakeContext.FakeHttpContext(
				new Dictionary<string, object> {
					{ "boardStatus", 3 },
					{ "boardMoves", new List<int> { 0, 1, 0, 0, 2, 0, 0, 2, 0} }
				}, "http://localhost:65516/api/").Session;
			BoardService servise = new BoardService(session);
			Assert.AreEqual(3, servise.GetSatusSession());
		}

		[TestMethod()]
		public void SaveSessionValuesTest()
		{
			var session = FakeContext.FakeHttpContext(
				new Dictionary<string, object> {
					{ "boardStatus", 3 },
					{ "boardMoves", new List<int> { 0, 1, 0, 0, 2, 0, 0, 2, 0} }
				}, "http://localhost:65516/api/").Session;
			BoardService servise = new BoardService(session);
			servise.BoardValues = Enumerable.Repeat(0, 9);
			servise.SaveSessionValues();
			Assert.AreEqual(0, ((IEnumerable<int>)session["boardMoves"]).ElementAt(1));
		}

		[TestMethod()]
		public void ClearBoardTest()
		{
			var session = FakeContext.FakeHttpContext(
				new Dictionary<string, object> {
					{ "boardStatus", 3 },
					{ "boardMoves", new List<int> { 0, 1, 0, 0, 2, 0, 0, 2, 0} }
				}, "http://localhost:65516/api/").Session;
			BoardService servise = new BoardService(session);
			servise.ClearBoard();
			Assert.IsNull(session["boardMoves"]);
			Assert.IsNull(session["boardStatus"]);
		}

		[TestMethod()]
		public void CheckCurrentStatusTest_XWin()
		{
			var session = FakeContext.FakeHttpContext(
				new Dictionary<string, object> {
					{ "boardStatus", 0 },
					{ "boardMoves", new List<int> { 0, 1, 0, 0, 1, 0, 0, 1, 0} }
				}, "http://localhost:65516/api/").Session;
			BoardService servise = new BoardService(session);
			Assert.AreEqual((int)Statuses.XWin, servise.CheckCurrentStatus());
		}

		[TestMethod()]
		public void CheckCurrentStatusTest_OWin()
		{
			var session = FakeContext.FakeHttpContext(
				new Dictionary<string, object> {
					{ "boardStatus", 0 },
					{ "boardMoves", new List<int> { 2, 1, 0, 0, 2, 0, 0, 1, 2} }
				}, "http://localhost:65516/api/").Session;
			BoardService servise = new BoardService(session);
			Assert.AreEqual((int)Statuses.OWin, servise.CheckCurrentStatus());
		}

		[TestMethod()]
		public void CheckCurrentStatusTest_None()
		{
			var session = FakeContext.FakeHttpContext(
				new Dictionary<string, object> {
					{ "boardStatus", 0 },
					{ "boardMoves", new List<int> { 0, 0, 0, 0, 2, 0, 0, 1, 2} }
				}, "http://localhost:65516/api/").Session;
			BoardService servise = new BoardService(session);
			Assert.AreEqual((int)Statuses.None, servise.CheckCurrentStatus());
		}

		[TestMethod()]
		public void CheckCurrentStatusTest_Ended()
		{
			var session = FakeContext.FakeHttpContext(
				new Dictionary<string, object> {
					{ "boardStatus", 0 },
					{ "boardMoves", new List<int> { 2, 1, 1, 1, 2, 2, 1, 2, 1} }
				}, "http://localhost:65516/api/").Session;
			BoardService servise = new BoardService(session);
			Assert.AreEqual((int)Statuses.Ended, servise.CheckCurrentStatus());
		}

		[TestMethod()]
		public void MakeMoveTest()
		{
			var session = FakeContext.FakeHttpContext(
				new Dictionary<string, object> {
					{ "boardStatus", 0 },
					{ "boardMoves", new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0} }
				}, "http://localhost:65516/api/").Session;
			BoardService servise = new BoardService(session);
			var cellNumber = 2;
			servise.MakeMove(cellNumber, Marks.X);

			Assert.AreEqual(1, servise.BoardValues.Where(x=>x==1).Count());
			Assert.AreEqual((int)Marks.X, servise.BoardValues.ElementAt(cellNumber));
		}

		[TestMethod()]
		public void MakeAIMoveTest()
		{
			var session = FakeContext.FakeHttpContext(
				new Dictionary<string, object> {
					{ "boardStatus", 0 },
					{ "boardMoves", new List<int> { 1, 0, 0, 0, 0, 0, 0, 0, 0} }
				}, "http://localhost:65516/api/").Session;
			BoardService servise = new BoardService(session);
			servise.MakeAIMove(Marks.O);

			Assert.AreEqual(1, servise.BoardValues.Where(x => x == (int)Marks.O).Count());
		}
	}
}