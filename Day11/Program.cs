using RonjasToolbox;

var octoGrid = File.ReadAllLines("input.txt")
    .Select(line => line.Select(c => c - '0').ToArray())
    .ToArray();

{
    var gridCopy = octoGrid.Select(l=>l.Select(o=>o).ToArray()).ToArray(); //clone
    var flashes = 0;
    foreach (var _ in 100) {
        var stepFlashes = new Queue<(int X, int Y)>();
        {
            foreach (var y in 10) {
                foreach (var x in 10) {
                    gridCopy[x][y]++;
                    if (gridCopy[x][y] == 10) {
                        stepFlashes.Enqueue((x, y));
                    }
                }
            }
        }
        {
            while (stepFlashes.TryDequeue(out var flash)) {
                foreach(var dy in (^1..1).Iter(true)) {
                    foreach(var dx in (^1..1).Iter(true)) {
                        var (x, y) = (X:flash.X + dx, Y:flash.Y + dy);
                        if(x < 0 || x >= 10) continue;
                        if(y < 0 || y >= 10) continue;
                        gridCopy[x][y]++;
                        if (gridCopy[x][y] == 10) {
                            stepFlashes.Enqueue((x,y));
                        }
                    }
                }
            }
        }
        {
            foreach (var y in 10) {
                foreach (var x in 10) {
                    if (gridCopy[x][y] >= 10) {
                        gridCopy[x][y] = 0;
                        flashes++;
                    }
                }
            }
        }
    }
    Console.WriteLine($"there have been {flashes} flashes after 100 iterations");
}

{
    var gridCopy = octoGrid.Select(l=>l.Select(o=>o).ToArray()).ToArray(); //clone
    var iterations = 0;
    while(true) {
        var stepFlashes = new Queue<(int X, int Y)>();
        {
            foreach (var y in 10) {
                foreach (var x in 10) {
                    gridCopy[x][y]++;
                    if (gridCopy[x][y] == 10) {
                        stepFlashes.Enqueue((x, y));
                    }
                }
            }
        }
        {
            while (stepFlashes.TryDequeue(out var flash)) {
                foreach(var dy in (^1..1).Iter(true)) {
                    foreach(var dx in (^1..1).Iter(true)) {
                        var (x, y) = (X:flash.X + dx, Y:flash.Y + dy);
                        if(x < 0 || x >= 10) continue;
                        if(y < 0 || y >= 10) continue;
                        gridCopy[x][y]++;
                        if (gridCopy[x][y] == 10) {
                            stepFlashes.Enqueue((x,y));
                        }
                    }
                }
            }
        }
        {
            foreach (var y in 10) {
                foreach (var x in 10) {
                    if (gridCopy[x][y] >= 10) {
                        gridCopy[x][y] = 0;
                    }
                }
            }
        }
        iterations++;
        if (gridCopy.All(line => line.All(o => o == 0))) break;
    }
    Console.WriteLine($"the octopi first flash simultaneously after {iterations} iterations");
}