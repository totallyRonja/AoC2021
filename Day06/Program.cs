using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

//BenchmarkRunner.Run<Stuff>();

var stuff = new Stuff();
stuff.Iterations = 80;
var first = stuff.Recursion();

stuff.Iterations = 256;
var second = stuff.Recursion();

Console.WriteLine($"There are {first} fish after 80 iterations and {second} fish after 256 iterations");

public class Stuff { 
	int[] input = File.ReadAllText("input.txt").Split(",").Select(int.Parse).ToArray();
	
	[Params(18, 80, 256)]
	public int Iterations { get; set; }
	
	[Benchmark]
	public long Recursion() {
		var fishCache = new Dictionary<(int Time, int Age), long>();
		long fishCount = 0;
		foreach (var fish in input) {
			fishCount += CountFish(fish, Iterations);
		}
		return fishCount;
		
		long CountFish(int age, int time) {
			if (age >= time) return 1;
			if (fishCache.TryGetValue((time, age), out var cachedCount)) return cachedCount;
			var spawnAge = time - age - 1;
			long count = CountFish(6, spawnAge) + CountFish(8, spawnAge);
			fishCache.Add((time, age), count);
			return count;
		}
	}

	[Benchmark]
	public long Buckets() {
		long[] fishBuckets = new long[9];
		foreach(var fish in input) {
			fishBuckets[fish]++;
		}

		for(int ii = 0; ii<256; ii++) {
			long first = fishBuckets[0];
			for(int i = 1; i < 9;i++) {
				fishBuckets[i - 1] = fishBuckets[i];
			}
			fishBuckets[6] += first;
			fishBuckets[8] = first;
		}

		return fishBuckets.Sum();
	}
}