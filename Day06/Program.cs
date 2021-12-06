using System.Diagnostics;
using RonjasToolbox;

var input = File.ReadAllText("input.txt").Split(",")
	.Select(int.Parse).ToArray();

//ronja solution
long fishCount = 0;
var fishCache = new Dictionary<(int Time, int Age), long>();

//Part 1
foreach (var fish in input) {
	fishCount += CountFish(fish, 80);
}
Console.WriteLine(fishCount);

//Part 2
var watch = Stopwatch.StartNew();
fishCount = 0;
foreach (var fish in input) {
	fishCount += CountFish(fish, 256);
}
Console.WriteLine($"Ronjas solution took: {watch.ElapsedTicks} ticks!");
Console.WriteLine(fishCount);

long CountFish(int age, int time) {
	if (age >= time) return 1;
	if (fishCache.TryGetValue((time, age), out var cachedCount)) return cachedCount;
	var spawnAge = time - age - 1;
	long count = CountFish(6, spawnAge) + CountFish(8, spawnAge);
	fishCache.Add((time, age), count);
	return count;
}

//bucket solution
watch.Restart();
long[] fishBuckets = new long[9];
foreach(var fish in input) {
	fishBuckets[fish]++;
}

foreach (int _ in 256) {
	long first = fishBuckets[0];
	foreach (long i in 1..9) {
		fishBuckets[i - 1] = fishBuckets[i];
	}
	fishBuckets[6] += first;
	fishBuckets[8] = first;
}

var sum = fishBuckets.Sum();
Console.WriteLine($"The bucket solution took: {watch.ElapsedTicks} ticks!"); //slower??
Console.WriteLine(sum);