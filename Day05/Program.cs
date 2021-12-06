using System.Text.RegularExpressions;
using RonjasToolbox;
using static Helper;

var input = File.ReadAllLines("input.txt");
var lines = input.Where(line => !string.IsNullOrWhiteSpace(line))
	.Select(line => {
		var match = Regex.Match(line, @"^(\d+),(\d+) -> (\d+),(\d+)$");
		var from = (X: int.Parse(match.Groups[1].Value), Y: int.Parse(match.Groups[2].Value));
		var to = (X: int.Parse(match.Groups[3].Value), Y: int.Parse(match.Groups[4].Value));
		return (From: from, To: to);
	}).ToArray();
	
var points = new Dictionary<(int, int), byte>();
foreach (var line in lines) {
	foreach (var point in line.Points(false)) {
		points.TryGetValue(point, out var count);
		count++;
		points[point] = count;
	}
}
Console.WriteLine(points.Values.Count(count => count > 1));

points.Clear();
foreach (var line in lines) {
	foreach (var point in line.Points(true)) {
		points.TryGetValue(point, out var count);
		count++;
		points[point] = count;
	}
}
Console.WriteLine(points.Values.Count(count => count > 1));

public static class Helper {
	public static IEnumerable<(int X, int Y)> Points(this ((int X, int Y) From, (int X, int Y) To) line, bool allowDiagonal) {
		if (line.From.X == line.To.X) {
			foreach (var y in (line.From.Y..line.To.Y).Iter(true)) {
				yield return (line.From.X, y);
			}
			yield break;
		}
		if (line.From.Y == line.To.Y) {
			foreach (var x in (line.From.X..line.To.X).Iter(true)) {
				yield return (x, line.From.Y);
			}
			yield break;
		}

		if (!allowDiagonal) yield break;
		foreach (var point in (line.From.X..line.To.X).Iter(true)
		         .Zip((line.From.Y..line.To.Y).Iter(true))) {
			yield return point;
		}
	}
}