using RonjasToolbox;

var input = File.ReadAllLines("input.txt");
var pairs = new Dictionary<char, char>(){{'<','>'}, {'(',')'},  {'[',']'}, {'{','}'}};

var analyzedLines = input.Select(line => {
    var stack = new Stack<char>();
    foreach (var character in line) {
        if (stack.TryPeek(out var stackTop) && pairs[stackTop] == character) {
            stack.Pop();
            continue;
        }

        if (pairs.ContainsKey(character)) {
            stack.Push(character);
            continue;
        }

        return (stack, character, State.Failure);
    }
    if (stack.Count > 0) return (stack, default, State.Incomplete);
    return (null, default, State.Success)!;
}).ToList();

var errorScore = analyzedLines.Where(line => line.Failure == State.Failure)
    .Select(line => line.character switch {'>' => 25137, ')' => 3, ']' => 57, '}' => 1197})
    .Sum();

Console.WriteLine($"Error score is " + errorScore);

analyzedLines = analyzedLines.Where(line => line.Failure == State.Incomplete).ToList();
var completeScores = analyzedLines.Where(line => line.Failure == State.Incomplete)
    .Select(line => line.stack.Select(c => c switch {'<' => 4, '(' => 1, '[' => 2, '{' => 3})
        .Aggregate(0L, (sum, c) => sum * 5 + c)).OrderBy(v => v).ToList();
Console.WriteLine(completeScores.Join("\n"));
var completeScore = completeScores[completeScores.Count / 2];

Console.WriteLine($"Autocomplete score is " + completeScore);

enum State {
    Success,
    Failure,
    Incomplete
}