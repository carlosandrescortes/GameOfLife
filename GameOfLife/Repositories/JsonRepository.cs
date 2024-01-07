using GameOfLife.Models;
using Newtonsoft.Json;

namespace GameOfLife.Repositories
{
	public class JsonRepository : IRepository
	{
		private static readonly object lockObject = new object();
		private readonly string _path = "boards.json";

		public async Task<int> AddBoardAsync(int[,] cells)
		{
			var boards = await GetBoardsAsync();
			var id = boards.Count + 1;

			boards.Add(id, new Board(cells));

			var json = JsonConvert.SerializeObject(boards);

			lock (lockObject)
			{
				using (StreamWriter writer = new StreamWriter(_path, false))
				{
					writer.Write(json);
				}
			}

			return id;
		}

		public async Task UpdateBoardAsync(int id, int[,] cells)
		{
			var boards = await GetBoardsAsync();
			boards[id].Cells = cells;

			var json = JsonConvert.SerializeObject(boards);

			lock (lockObject)
			{
				using (StreamWriter writer = new StreamWriter(_path, false))
				{
					writer.Write(json);
				}
			}
		}

		public async Task<Dictionary<int, Board>> GetBoardsAsync()
		{
			string json;

			lock (lockObject)
			{
				if (!File.Exists(_path))
				{
					return new Dictionary<int, Board>();
				}

				using (StreamReader reader = new StreamReader(_path))
				{
					json = reader.ReadToEnd();
				}
			}

			return JsonConvert.DeserializeObject<Dictionary<int, Board>>(json);
		}
	}
}
