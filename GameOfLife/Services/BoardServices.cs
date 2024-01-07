using GameOfLife.Models;
using GameOfLife.Repositories;
using Newtonsoft.Json;

namespace GameOfLife.Services
{
	public class BoardServices : IBoardServices
	{
		private IRepository _repo;
		private Dictionary<int, Board> _boards;

		public BoardServices(IRepository repo)
		{
			_repo = repo;
			_boards = _repo.GetBoardsAsync().Result;
		}

		public async Task<int> AddBoardAsync(int[,] cells)
		{
			var id = await _repo.AddBoardAsync(cells);
			_boards.Add(id, new Board(cells));

			return id;
		}

		public async Task<string> GetNextStateAsync(int id, int iterations)
		{
			var board = _boards[id];

			for (int i = 0; i < iterations; i++)
			{
				board.NextState();
			}

			await _repo.UpdateBoardAsync(id, board.Cells);

			return JsonConvert.SerializeObject(board.Cells);
		}

		public async Task<string> GetFinalStateAsync(int id, int iterations)
		{
			var board = _boards[id];
			var count = 0;

			while (!board.IsConcluded && count < iterations)
			{
				board.NextState();
				count++;
			}

			await _repo.UpdateBoardAsync(id, board.Cells);

			if (!board.IsConcluded)
			{
				throw new Exception($"Board is not concluded after {iterations} attempts");
			}

			return JsonConvert.SerializeObject(board.Cells);
		}
	}
}
