var crabs = File.ReadAllText("input.txt").Split(",").Select(int.Parse).ToArray();

long minCost = Int64.MaxValue;
for(int i=0;;i++) {
	var crabMeetup = i;
	var cost = crabs.Select(crab => Math.Abs(crab - crabMeetup)).Sum();
	if (cost < minCost) minCost = cost;
	else break;
}

Console.WriteLine(minCost);

minCost = Int64.MaxValue;
for(int i=0;;i++) {
	var crabMeetup = i;
	var cost = crabs.Select(crab => Math.Abs(crab - crabMeetup)).Sum(Fuel);
	if (cost < minCost) minCost = cost;
	else break;
}
Console.WriteLine(minCost);

int Fuel(int dist) => dist * (dist + 1) / 2;