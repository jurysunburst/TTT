using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.SessionState;
using TTT.Web.Enums;

namespace TTT.Web.Services
{
	public class BoardService
	{
		private List<int> _board;
		private List<int> _empties;
		private List<int> _crosses;
		private List<int> _noughts;
		private List<int> _cellsToCheck;

		private List<int[]> _winningRows;

		private HttpSessionState _session;

		public BoardService(HttpSessionState session)
		{
			_session = session;
			_board = LoadBoardSession().ToList();
			_cellsToCheck = new List<int> { 0, 1, 2, 3, 6 };
			_winningRows = new List<int[]> {
				new int[] { 0, 1, 2 }, new int[] { 0, 4, 8 }, new int[] { 0, 3, 6 }, new int[] { 1, 4, 7 },
				new int[] { 2, 4, 6 }, new int[] { 2, 5, 8 }, new int[] { 3, 4, 5 }, new int[] { 6, 7, 8 }
			};
			RefreshEmpty();
		}

		#region SessionMethods
		private IEnumerable<int> LoadBoardSession()
		{
			var moves = _session["boardMoves"];
			if (moves == null)
			{
				moves = Enumerable.Repeat(0, 9).ToList();
				_session["boardMoves"] = moves;
			}
			return (IEnumerable<int>)moves;
		}

		public int GetSatusSession()
		{
			var status = _session["boardStatus"];

			if (status == null)
			{
				status = (int)Statuses.None;
				_session["boardStatus"] = status;
			}
			return (int)status;
		}

		private void SaveBoard()
		{
			_session["boardMoves"] = _board;
		}
		private void SaveStatus()
		{
			_session["boardStatus"] = CheckCurrentStatus();
		}

		public void SaveSessionValues()
		{
			SaveBoard();
			SaveStatus();
		}

		public void ClearBoard()
		{
			_session["boardMoves"] = null;
			_session["boardStatus"] = null;
		}
		#endregion
		#region BoardMethods

		public IEnumerable<int> BoardValues
		{
			get
			{
				return _board;
			}
			set
			{
				if (value != null)
				{
					_board = value.ToList();
					RefreshEmpty();
				}
			}
		}

		public int CheckCurrentStatus()
		{
			if (CheckForMark(Marks.X))
				return (int)Statuses.XWin;
			if (CheckForMark(Marks.O))
				return (int)Statuses.OWin;
			return _empties.Count == 0 ? (int)Statuses.Ended : (int)Statuses.None;
		}

		public void MakeMove(int cell, Marks mark)
		{
			if (cell != -1)
			{
				_board[cell] = (int)mark;
				_empties.Remove(cell);
				if (mark == Marks.X)
					_crosses.Add(cell);
				if (mark == Marks.O)
					_noughts.Add(cell);
			}
		}

		public void MakeAIMove(Marks mark, bool fullRandom = false)
		{

			int cellNumber = -1;
			var cellsToMove = new List<int>();
			if (fullRandom)
				cellsToMove = _empties;
			else
				foreach (var checkedRow in _winningRows)
				{
					var filledCrosses = checkedRow.Intersect(_crosses);
					var crossesCount = filledCrosses.Count();
					if (crossesCount == 1)
					{
						cellsToMove.AddRange(checkedRow.Union(_crosses).Except(filledCrosses));
					}
					if (crossesCount == 2)
					{
						cellNumber = checkedRow.Except(filledCrosses).Single();
						if (!_empties.Contains(cellNumber))
							cellNumber = -1;
						else
							break;
					}
				}
			if (cellNumber == -1)
			{
				var r = new Random();
				cellsToMove = cellsToMove.Intersect(_empties).ToList();
				if (cellsToMove.Count != 0)
				{
					var cell = r.Next(0, cellsToMove.Count);
					cellNumber = cellsToMove[cell];
				}
			}
			MakeMove(cellNumber, mark);
		}

		private bool CheckForMark(Marks mark)
		{
			List<int[]> rowsToCheck = new List<int[]>();
			int currentMark = (int)mark;
			var rowFilled = false;
			foreach (var cell in _cellsToCheck)
			{
				if (_board[cell] != currentMark && !rowFilled)
					continue;
				rowsToCheck = _winningRows.Where(x => x[0] == cell).ToList();

				foreach (var row in rowsToCheck)
				{
					if (_board[row[0]] == currentMark && _board[row[1]] == currentMark && _board[row[2]] == currentMark)
					{
						rowFilled = true;
						break;
					}
				}
			}
			return rowFilled;
		}

		private void RefreshEmpty()
		{
			_empties = new List<int>();
			_crosses = new List<int>();
			_noughts = new List<int>();
			for (var i = 0; i < _board.Count; i++)
			{
				if (_board[i] == (int)Marks.Empty)
					_empties.Add(i);
				if (_board[i] == (int)Marks.X)
					_crosses.Add(i);
				if (_board[i] == (int)Marks.O)
					_noughts.Add(i);
			}
		}
		#endregion
	}
}