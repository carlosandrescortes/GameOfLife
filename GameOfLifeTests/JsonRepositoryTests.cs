using GameOfLife.Models;
using GameOfLife.Repositories;
using Newtonsoft.Json;

namespace GameOfLife.Tests
{
	public class JsonRepositoryTests
	{
		private static readonly object lockObject = new object();
		private readonly string _path = "boards.json";
		private readonly JsonRepository _repo;

		public JsonRepositoryTests()
		{
			lock (lockObject)
			{
				if (File.Exists(_path))
				{
					File.Delete(_path);
				}
			}

			_repo = new JsonRepository();
		}

		[Fact]
		public async Task AddBoardAsync_ShouldCreateFile()
		{
			// Arrange
			var cells = new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };

			// Act
			await _repo.AddBoardAsync(cells);

			// Assert
			Assert.True(File.Exists(_path));
		}

		[Fact]
		public async Task AddBoardAsync_ShouldReturnExpectedId()
		{
			// Arrange
			var cells = new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };
			var expectedId = 1;

			// Act
			var result = await _repo.AddBoardAsync(cells);

			// Assert
			Assert.Equal(expectedId, result);
		}

		[Fact]
		public async Task UpdateBoardAsync_ShouldUpdateFile()
		{
			// Arrange
			var cells = new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };

			var id = await _repo.AddBoardAsync(cells);

			var expectedCells = new int[,] { { 0, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };

			// Act
			await _repo.UpdateBoardAsync(id, expectedCells);

			// Assert
			var json = await File.ReadAllTextAsync(_path);
			var boards = JsonConvert.DeserializeObject<Dictionary<int, Board>>(json);

			Assert.Equal(expectedCells, boards[id].Cells);
		}

		[Fact]
		public async Task GetBoardsAsync_ShouldReturnExpectedBoards()
		{
			// Arrange
			var cells1 = new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };
			var cells2 = new int[,] { { 0, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };

			var id1 = await _repo.AddBoardAsync(cells1);
			var id2 = await _repo.AddBoardAsync(cells2);

			// Act
			var result = await _repo.GetBoardsAsync();

			// Assert
			Assert.Equal(cells1, result[id1].Cells);
			Assert.Equal(cells2, result[id2].Cells);
		}
	}
}
