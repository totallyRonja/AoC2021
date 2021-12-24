
var input = File.ReadAllLines("input.txt");

var caves = new Dictionary<string, List<string>>();
foreach (var (from, to) in input.Select(line => {
             var split = line.IndexOf('-');
             return (From: line[..split], To: line[(split+1)..]);
         })) {
    Console.WriteLine($"from {from} to {to}");
    if (!caves.TryGetValue(from, out var fromConnections)) {
        fromConnections = new List<string>();
        caves.Add(from, fromConnections);
    }
    fromConnections.Add(to);
    if (!caves.TryGetValue(to, out var toConnections)) {
        toConnections = new List<string>();
        caves.Add(to, toConnections);
    }
    toConnections.Add(from);
}


var pathCount1 = WalkPaths1("start", new List<string>());
Console.WriteLine($"there are {pathCount1} paths through the caves");

var pathCount2 = WalkPaths2("start", new List<string>(), false);
Console.WriteLine($"there are {pathCount2} paths through the caves if we can visit once cave twice");

int WalkPaths1(string node, List<string> visitedNodes) {
    if (visitedNodes.Contains(node)) return 0;
    if (node == "end") return 1;
    if (node.ToLower() == node) visitedNodes.Add(node);
    var paths = 0;
    foreach (var connection in caves[node]) {
        paths += WalkPaths1(connection, new List<string>(visitedNodes));
    }
    return paths;
}

int WalkPaths2(string node, List<string> visitedNodes, bool visitedTwice) {
    if (visitedNodes.Contains(node)) {
        if (node == "start") return 0;
        if (visitedTwice) return 0;
        visitedTwice = true;
    }
    if (node == "end") return 1;
    if (node.ToLower() == node) visitedNodes.Add(node);
    var paths = 0;
    foreach (var connection in caves[node]) {
        paths += WalkPaths2(connection, new List<string>(visitedNodes), visitedTwice);
    }
    return paths;
}
