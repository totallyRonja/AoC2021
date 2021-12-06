using RonjasToolbox;
using static Helper;

var input = File.ReadAllLines("input.txt");

var calledNumbers = input[0].Split(',').Select(int.Parse).ToArray();

ListView2d<sbyte>?[] boards = input.Skip(1).Chunk(6).Select(boardInput => {
	var boardArr = boardInput.Skip(1)
		.SelectMany(boardLine => boardLine.Split(' ', StringSplitOptions.RemoveEmptyEntries)
			.Select(sbyte.Parse).ToArray()).ToArray();
	return new ListView2d<sbyte>(boardArr, 5, 5);
}).ToArray();

var playingBoards = boards.Length;

//go through numbers
foreach (int number in calledNumbers) {
	//go through fields
	foreach (var boardIndex in boards.Length) {
		var board = boards[boardIndex];
		if(board == null) continue;
		//adjust field if it has current number
		foreach (var boardX in 5) {
			foreach (var boardY in 5) {
				if (board[boardX, boardY] == number) {
					board[boardX, boardY] = -1;
				}
			}
		}
		//end if change makes field a winning field!
		if (IsWinningBoard(board)) {
			//first win for Part 1
			if (playingBoards == boards.Length) { 
				Console.WriteLine("First win!");
				Console.WriteLine(boards[boardIndex]);
				Console.WriteLine(BoardScore(board, number));
				//goto afterBingo1;
			}
			playingBoards--;
			//last win for Part 2
			if (playingBoards == 0) { 
				Console.WriteLine("Last win!");
				Console.WriteLine(boards[boardIndex]);
				Console.WriteLine(BoardScore(board, number));
				goto afterBingo1;
			}
			boards[boardIndex] = null;
		}
	}
	
}
afterBingo1:
int _;

public static class Helper {
	public static bool IsWinningBoard(ListView2d<sbyte> board) {
		foreach (int row in 5) {
			if (IsWinningLine(board, row * 5, 1)) return true;
		}
		foreach (int col in 5) {
			if (IsWinningLine(board, col, 5)) return true;
		}
		return false;
	}
	
	public static bool IsWinningLine(ListView2d<sbyte> board, int start, int stride) {
		foreach (int i in 5) {
			if (board[start + stride * i] >= 0) return false;
		}
		return true;
	}

	public static int BoardScore(ListView2d<sbyte> winningBoard, int calledNumber) {
		int sum = 0;
		foreach (sbyte field in winningBoard) {
			if (field > 0) sum += field;
		}
		return sum * calledNumber;
	}
}