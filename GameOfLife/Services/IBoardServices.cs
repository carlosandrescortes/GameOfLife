namespace GameOfLife.Services
{
	public interface IBoardServices
	{
		Task<int> AddBoardAsync(int[,] cells);
		Task<string> GetNextStateAsync(int id, int iterations);
		Task<string> GetFinalStateAsync(int id, int iterations);
	}
}
