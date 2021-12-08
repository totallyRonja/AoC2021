var crabs = File.ReadAllText("input.txt").Split(",").Select(int.Parse).ToArray();

{
	//Day01
	//naive solution
	long minCost = Int64.MaxValue;
	for(int i=0;;i++) {
		var crabMeetup = i;
		var cost = crabs.Select(crab => Math.Abs(crab - crabMeetup)).Sum();
		if (cost < minCost) minCost = cost;
		else break;
	}

	Console.WriteLine(minCost);
	
	//smart solution
	var median = crabs.OrderBy(crab => crab).ElementAt(crabs.Length/2);
	var medianCost = crabs.Select(crab => Math.Abs(crab - median)).Sum();
	Console.WriteLine(medianCost);
}

{
	int minCost = Int32.MaxValue;
	for(int i=0;;i++) {
		var crabMeetup = i;
		var cost = crabs.Select(crab => Math.Abs(crab - crabMeetup)).Sum(Fuel);
		if (cost < minCost) minCost = cost;
		else break;
	}
	Console.WriteLine(minCost);
}

int Fuel(int dist) => dist * (dist + 1) / 2;