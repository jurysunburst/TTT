using System.Collections.Generic;

namespace TTT.Web.Model
{
	public class BoardModel
	{
		public int Status { get; set; }
		public IEnumerable<int> Moves { get; set; }
	}
}