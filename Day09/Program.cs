using RonjasToolbox;

var input = File.ReadAllLines("input.txt");

var data = new ListView2d<int>(input
	.SelectMany(line => line.Select(c => c - '0')).ToArray(), input[0].Length, input.Length);


(int X, int Y)[] orthoNeighborDirections = new[] {(-1, 0), (0, 1), (1, 0), (0, -1)};

var minima = new List<(int Height, (int X , int Y) Position)>();
foreach (int x in data.Width) {
	foreach (int y in data.Height) {
		var self = data[x, y];
		foreach (var dir in orthoNeighborDirections) {
			var neighborPos = (X: x + dir.X, Y: y + dir.Y);
			if(neighborPos.X < 0 || neighborPos.X >= data.Width || neighborPos.Y < 0 || neighborPos.Y >= data.Height) continue;
			var neighbor = data[neighborPos.X, neighborPos.Y];
			if (neighbor <= self) goto continue_outer;
		}
		minima.Add((self, (x, y)));
		continue_outer: ;
	}
}

var dangerLevels = minima.Sum(min => min.Height+1);
Console.WriteLine(dangerLevels);

var basins = new List<(int X, int Y)>[minima.Count];
foreach (var i in basins.Length) {
	var border = new Queue<(int X, int Y)>();
	border.Enqueue(minima[i].Position);
	var visited = new HashSet<(int X, int Y)>();
	visited.Add(minima[i].Position);

	var basin = new List<(int X, int Y)>();
	basin.Add(minima[i].Position);
	basins[i] = basin;
	while (border.TryDequeue(out var point)) {
		foreach (var dir in orthoNeighborDirections) {
			var neighborPos = (X: point.X + dir.X, Y: point.Y + dir.Y);
			if(neighborPos.X < 0 || neighborPos.X >= data.Width || neighborPos.Y < 0 || neighborPos.Y >= data.Height) continue;
			if(visited.Contains(neighborPos)) continue;
			var neighbor = data[neighborPos.X, neighborPos.Y];
			if(neighbor == 9) continue;
			//possible height diff check?
			border.Enqueue(neighborPos);
			visited.Add(neighborPos);
			basin.Add(neighborPos);
		}
	}
}

foreach (int y in data.Height) {
	foreach (int x in data.Width) {
		var num = data[x, y];
		if (num == 9) Console.ForegroundColor = ConsoleColor.Black;
		else if(minima.Exists(min => min.Position == (x, y))) Console.ForegroundColor = ConsoleColor.Yellow;
		else {
			var basinIndex = basins.IndexOf(basin => basin.Contains((x, y)));
			var basinColors = new[]{ConsoleColor.DarkBlue, ConsoleColor.DarkCyan, ConsoleColor.DarkMagenta, ConsoleColor.DarkRed, ConsoleColor.DarkYellow, ConsoleColor.DarkGreen};
			Console.ForegroundColor = basinColors[basinIndex % basinColors.Length];
		}
		Console.Write(num);
	}
	Console.WriteLine();
}
Console.ResetColor();

Console.WriteLine(basins.Select(bas => bas.Count).OrderBy(c => c).Join(", "));
Console.WriteLine(basins.Select(bas => bas.Count).OrderBy(c => c).TakeLast(3).Aggregate(1, (i, i1) => i * i1));