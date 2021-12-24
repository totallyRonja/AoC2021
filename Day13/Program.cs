using System.Text;
using RonjasToolbox;

var lines = File.ReadAllLines("input.txt");

var dots = new List<(int X, int Y)>();
var folds = new List<(char Axis, int Pos)>();

foreach (var line in lines) {
    if (line.StartsWith("fold along "))
        folds.Add((line["fold along ".Length], int.Parse(line[("fold along ".Length + 2)..])));
    var split = line.IndexOf(',');
    if (split >= 0)
        dots.Add((int.Parse(line[..split]), int.Parse(line[(split+1)..])));
}

//Console.WriteLine(dots.Join(", "));
Console.WriteLine(folds.Join(", "));

foreach(var fold in folds)
{
    foreach (var i in dots.Count.Iter().Reverse()){
        var dot = dots[i];
        if (fold.Axis == 'x' && dot.X > fold.Pos) {
            dots.Remove(dot);
            var newDot = (fold.Pos - (dot.X - fold.Pos), dot.Y);
            if(!dots.Contains(newDot)) dots.Add(newDot);
        }

        if (fold.Axis == 'y' && dot.Y > fold.Pos) {
            dots.Remove(dot);
            var newDot = (dot.X, fold.Pos - (dot.Y - fold.Pos));
            if(!dots.Contains(newDot)) dots.Add(newDot);
        }
    }
    Console.WriteLine($"{dots.Count()} dots");
}
PrintDots(dots);

void PrintDots(List<(int X, int Y)> dots) {
    var maxX = dots.Max(d => d.X)+1;
    var maxY = dots.Max(d => d.Y)+1;
    var res = new StringBuilder();
    foreach (var y in maxY) {
        foreach (var x in maxX) {
            res.Append(dots.Contains((x, y)) ? '#' : '.');
        }
        res.Append('\n');
    }
    Console.Write(res);
}