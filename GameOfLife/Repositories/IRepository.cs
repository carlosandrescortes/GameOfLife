using GameOfLife.Models;

namespace GameOfLife.Repositories
{
	public interface IRepository
	{
		Task<int> AddBoardAsync(int[,] cells);
		Task UpdateBoardAsync(int id, int[,] cells);
		Task<Dictionary<int, Board>> GetBoardsAsync();
	}
}
