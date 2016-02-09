using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TTT.Web.Tests.Session;

namespace TTT.Web.Controllers.Tests
{
	[TestClass()]
	public class BoardMoveControllerTests
	{
		[TestMethod()]
		public void GetTest()
		{
			HttpContext.Current = FakeContext.FakeHttpContext(
				new Dictionary<string, object>
				{
				}, "http://localhost:65516/api/");
			BoardMoveController controller = new BoardMoveController();

			// Act
			var result = controller.Get();

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Status);
			Assert.AreEqual(0, result.Moves.ElementAt(0));
			Assert.AreEqual(9, result.Moves.Count());
		}

		[TestMethod()]
		public void GetPredefinedTest()
		{
			HttpContext.Current = FakeContext.FakeHttpContext(
				new Dictionary<string, object> {
					{ "boardStatus", 0 },
					{ "boardMoves", new List<int> { 0, 1, 0, 0, 2, 0, 0, 2, 0} }
				}, "http://localhost:65516/api/");
			BoardMoveController controller = new BoardMoveController();

			// Act
			var result = controller.Get();

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Status);
			Assert.AreEqual(1, result.Moves.ElementAt(1));
			Assert.AreEqual(2, result.Moves.ElementAt(7));
			Assert.AreEqual(9, result.Moves.Count());
		}

		[TestMethod()]
		public void PostTest()
		{
			HttpContext.Current = FakeContext.FakeHttpContext(
				new Dictionary<string, object> {
					{ "boardStatus", 0 },
					{ "boardMoves", new List<int> { 0, 1, 0, 0, 2, 0, 0, 2, 0} }
				}, "http://localhost:65516/api/");
			BoardMoveController controller = new BoardMoveController();

			// Act
			controller.Post("0");
			var result = controller.Get();

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Status);
			Assert.AreEqual(1, result.Moves.ElementAt(0));
			Assert.AreEqual(1, result.Moves.ElementAt(1));
			Assert.AreEqual(2, result.Moves.ElementAt(7));
			Assert.AreEqual(9, result.Moves.Count());
		}

		[TestMethod()]
		public void DeleteTest()
		{
			HttpContext.Current = FakeContext.FakeHttpContext(
				new Dictionary<string, object> {
					{ "boardStatus", 1 },
					{ "boardMoves", new List<int> { 1, 1, 1, 0, 2, 0, 0, 2, 0} }
				}, "http://localhost:65516/api/");
			BoardMoveController controller = new BoardMoveController();

			controller.Delete();

			var result = controller.Get();

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Status);
			Assert.AreEqual(0, result.Moves.ElementAt(0));
			Assert.AreEqual(9, result.Moves.Count());
		}
	}
}