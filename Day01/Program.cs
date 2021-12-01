using RonjasToolbox;

var input = File.ReadAllLines("./input.txt");
var values = input.Select(int.Parse).ToArray();

Console.WriteLine("Day 01");
Console.WriteLine("Part 1");
//Part 1
{
	//I'm addicted to LINQ
	var increases = values
		.Pair()
		.Count(pair => pair.Second > pair.First);
	Console.WriteLine(increases);
}

{
	//non-overcomplicated for
	var increases = 0;
	foreach(int i in 1..input.Length) {
		if (values[i] > values[i - 1]) increases++;
	}
	Console.WriteLine(increases);
}

//Part 2
Console.WriteLine("Part 2");
{
	//Ronja thinks she's smort and overcomplicates things
	var increases = 0;
	foreach(int i in 1..(input.Length-2)) {
		//compare the value that leaves the window (-1) with the value that enters the window (+2)
		if (values[i+2] > values[i - 1]) increases++;
	}
	Console.WriteLine(increases);
}

{
	//one more LINQ solution as a treat
	var increases = values
		.Triplet()
		.Select(window => window.First + window.Second + window.Third)
		.Pair()
		.Count(pair => pair.Second > pair.First);
	Console.WriteLine(increases);
}

{
	//oh wait I can do the windowless variant with Linq!!
	var increases = values
		.Zip(values.Skip(3))
		.Count(pair => pair.Second > pair.First);
	Console.WriteLine(increases);
}