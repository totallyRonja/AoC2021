using System.Text.RegularExpressions;
using RonjasToolbox;

var input = File.ReadAllLines("input.txt");

byte[]? basePolymer = null;
var insertRules = new Dictionary<(byte left, byte right), byte>();
//parse
foreach(var line in input) {
    var ruleMatch = Regex.Match(line, "^([A-Z])([A-Z]) -> ([A-Z])$");
    if (ruleMatch.Success){
        insertRules.Add(((byte)(ruleMatch.Groups[1].Value[0] - 'A'), (byte)(ruleMatch.Groups[2].Value[0] - 'A')), (byte)(ruleMatch.Groups[3].Value[0] - 'A'));
    } else if (!line.IsNullOrWhiteSpace()) {
        basePolymer = line.Select(c => (byte)(c-'A')).ToArray();
    }
}
if (basePolymer is null) return;
//condense
var elements = new List<byte>();
foreach (var ((left, right), value) in insertRules) {
    elements.Add(left);
    elements.Add(right);
    elements.Add(value);
}
elements = elements.Distinct().ToList();

basePolymer = basePolymer.Select(e => (byte)elements.IndexOf(e)).ToArray();
insertRules = insertRules.ToDictionary(
    kv => ((byte)elements.IndexOf(kv.Key.left), (byte)elements.IndexOf(kv.Key.right)), 
    kv => (byte)elements.IndexOf(kv.Value));

var elementCount = elements.Count;


{
    var cache = new Dictionary<(byte left, byte right, int depth), long[]>();
    var counts = new long[elementCount];
    
    var polymer = basePolymer.ToArray();
    var iterations = 40;

    foreach (var p in polymer) {
        counts[p]++;
    }
    foreach (var (first, second) in polymer.Pair()) {
        var subAmounts = CalculateAmounts(first, second, 1);
        foreach (var (i, c) in elementCount.Iter().Zip(subAmounts)) {
            counts[i] += c;
        }
    }
    
    long[] CalculateAmounts(byte first, byte second, int depth) {
        if (!cache.TryGetValue((first, second, depth), out var amounts)) {
            if (depth == iterations) {
                amounts = new long[elementCount];
                amounts[insertRules[(first, second)]]++;
            } else {
                var center = insertRules[(first, second)];
                amounts = CalculateAmounts(first, center, depth + 1)
                    .Zip(CalculateAmounts(center, second, depth + 1), (left, right) => left + right).ToArray();
                amounts[center]++;
            }
            cache.Add((first, second, depth), amounts);
        }
        return amounts;
    }

    Console.WriteLine(counts.Max() - counts.Min());
}

{
    var ruleArr = insertRules.ToArray();
    var insertLookup = ruleArr.Select(kv => 
        (ruleArr.IndexOf(rule => rule.Key.left == kv.Key.left && rule.Key.right == kv.Value), 
        ruleArr.IndexOf(rule => rule.Key.left == kv.Value && rule.Key.right == kv.Key.right)))
        .ToArray();
    
    var buckets = new long[insertRules.Count];
    long[] backBuffer;

    foreach (var (left, right) in basePolymer.Pair()) {
        var i = insertRules.IndexOf(rule => rule.Key.left == left && rule.Key.right == right);
        buckets[i]++;
    }

    foreach (var _ in 40) {
        backBuffer = new long[insertRules.Count];
        foreach (var i in buckets.Length) {
            var insert = insertLookup[i];
            backBuffer[insert.Item1] += buckets[i];
            backBuffer[insert.Item2] += buckets[i];
        }
        (buckets, backBuffer) = (backBuffer, buckets);
    }
    
    var counts = new long[elementCount];
    foreach (var i in buckets.Length) {
        counts[ruleArr[i].Key.left] += buckets[i];
    }
    counts[basePolymer.Last()]++;
    
    Console.WriteLine(counts.Max() - counts.Min());
}
