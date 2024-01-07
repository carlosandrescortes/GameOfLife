using GameOfLife.Models;
using GameOfLife.Repositories;
using GameOfLife.Services;
using Moq;
using Newtonsoft.Json;

namespace GameOfLife.Tests
{
	public class BoardServicesTests
	{
		private readonly Mock<IRepository> _mockRepo;
		private readonly BoardServices _boardServices;

		public BoardServicesTests()
		{
			_mockRepo = new Mock<IRepository>();
			_mockRepo.Setup(repo => repo.GetBoardsAsync()).ReturnsAsync(new Dictionary<int, Board>());
			_boardServices = new BoardServices(_mockRepo.Object);
		}

		[Fact]
		public async Task AddBoardAsync_ShouldReturnExpectedId()
		{
			// Arrange
			var cells = new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };
			var expectedId = 1;

			_mockRepo.Setup(repo => repo.AddBoardAsync(cells)).ReturnsAsync(expectedId);

			// Act
			var result = await _boardServices.AddBoardAsync(cells);

			// Assert
			Assert.Equal(expectedId, result);
		}

		[Fact]
		public async Task GetNextStateAsync_ShouldReturnExpectedJson()
		{
			// Arrange
			var cells = new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };

			var expectedId = 1;
			var iterations = 1;

			_mockRepo.Setup(repo => repo.AddBoardAsync(cells)).ReturnsAsync(expectedId);
			var id = await _boardServices.AddBoardAsync(cells);

			var expectedCells = new int[,] { { 0, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
			var expectedJson = JsonConvert.SerializeObject(expectedCells);

			// Act
			var result = await _boardServices.GetNextStateAsync(id, iterations);

			// Assert
			Assert.Equal(expectedJson, result);
		}

		[Fact]
		public async Task GetNextStateAsync_ShouldReturnExpectedJsonAfterMultipleIterations()
		{
			// Arrange
			var cells = new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };

			var expectedId = 1;
			var iterations = 10;

			_mockRepo.Setup(repo => repo.AddBoardAsync(cells)).ReturnsAsync(expectedId);
			var id = await _boardServices.AddBoardAsync(cells);

			var expectedJson = JsonConvert.SerializeObject(cells);

			// Act
			var result = await _boardServices.GetNextStateAsync(id, iterations);

			// Assert
			Assert.Equal(expectedJson, result);
		}

		[Fact]
		public async Task GetFinalStateAsync_ShouldThrowExceptioWhenConclusionNotReached()
		{
			// Arrange
			var cells = new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };

			var expectedId = 1;
			var iterations = 10;

			_mockRepo.Setup(repo => repo.AddBoardAsync(cells)).ReturnsAsync(expectedId);
			var id = await _boardServices.AddBoardAsync(cells);

			// Act
			// Assert
			await Assert.ThrowsAsync<Exception>(() => _boardServices.GetFinalStateAsync(id, iterations));
		}

		[Fact]
		public async Task GetFinalStateAsync_ShouldReturnExpectedJsonWhenConclusionReached()
		{
			// Arrange
			var cells = new int[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };

			var expectedId = 1;
			var iterations = 10;

			_mockRepo.Setup(repo => repo.AddBoardAsync(cells)).ReturnsAsync(expectedId);
			var id = await _boardServices.AddBoardAsync(cells);

			var expectedCells = new int[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
			var expectedJson = JsonConvert.SerializeObject(expectedCells);

			// Act
			var result = await _boardServices.GetFinalStateAsync(id, iterations);

			// Assert
			Assert.Equal(expectedJson, result);
		}

	}
}
