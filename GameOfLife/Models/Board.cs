namespace GameOfLife.Models
{
	public class Board
	{
		public int[,] Cells { get; set; }
		public bool IsConcluded { get; set; }

		public Board(int[,] cells)
		{
			Cells = cells;
		}

		private bool IsAlive(int x, int y, int state)
		{
			int neighbors = 0;

			for (int i = x - 1; i <= x + 1; i++)
			{
				for (int j = y - 1; j <= y + 1; j++)
				{
					if ((i == x && j == y) ||
						(i < 0 || i >= Cells.GetLength(0) ||
						j < 0 || j >= Cells.GetLength(1)))
					{
						continue;
					}
					if (Cells[i, j] == 1)
					{
						neighbors++;
					}
				}
			}

			return (neighbors == 3) || (state == 1 && neighbors == 2);
		}

		public void NextState()
		{
			var newCells = new int[Cells.GetLength(0), Cells.GetLength(1)];
			var countAlive = 0;

			for (int i = 0; i < Cells.GetLength(0); i++)
			{
				for (int j = 0; j < Cells.GetLength(1); j++)
				{
					if (IsAlive(i, j, Cells[i, j]))
					{
						newCells[i, j] = 1;
						countAlive++;
					}
				}
			}

			IsConcluded = countAlive == 0;
			Cells = newCells;
		}
	}
}
