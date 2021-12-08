using RonjasToolbox;

var input = File.ReadAllLines("input.txt").Select(line => {
	var parts = line.Split(" | ");
	var inputs = parts[0].Split(" ").Select(StringToNumbersDisplay).ToArray();
	var display = parts[1].Split(" ").Select(StringToNumbersDisplay).ToArray();
	return (Inputs: inputs, Display: display);
}).ToArray();

bool[][] displayConfigurations = {
	new[] { true, true, true, false, true, true, true },
	new[] { false, false, true, false, false, true, false },
	new[] { true, false, true, true, true, false, true },
	new[] { true, false, true, true, false, true, true },
	new[] { false, true, true, true, false, true, false },
	new[] { true, true, false, true, false, true, true },
	new[] { true, true, false, true, true, true, true },
	new[] { true, false, true, false, false, true, false },
	new[] { true, true, true, true, true, true, true },
	new[] { true, true, true, true, false, true, true },
};


//part 1
var simpleNumbers = new[] { 1, 4, 7, 8 };
int[] segCount1478 = simpleNumbers.Select(GetNumberDisplay).Select(disp => disp.Count(on => on)).ToArray();
int count1478 = input
	.Select(line => line.Display
		.Count(digit => segCount1478.Contains(digit.Length) )).Sum();

Console.WriteLine($"1, 4, 7 and 8 appears {count1478} times in the displays");

//part 2
int sum = 0;
foreach ((int[][] inputs, int[][] display) in input) {
	var connections = new HashSet<int>[7];
	foreach (var i in 7) {
		connections[i] = new HashSet<int>(7.Iter());
	}

	
	foreach (int[] inp in inputs) {
		var index1478 = segCount1478.IndexOf(inp.Length);
		if (index1478 >= 0) {
			var displaySegments = GetNumberDisplay(simpleNumbers[index1478]);
			foreach ((var connection, int index) in connections.Zip(7.Iter())) {
				var isOn = displaySegments[index];
				connection.RemoveWhere(con => inp.Contains(con) != isOn);
			}
		}
	}
	
	var digit0 = GetDisplayNumberFuzzy(connections, display[0]);
	var digit1 = GetDisplayNumberFuzzy(connections, display[1]);
	var digit2 = GetDisplayNumberFuzzy(connections, display[2]);
	var digit3 = GetDisplayNumberFuzzy(connections, display[3]);

	sum += 1000 * digit0 + 100 * digit1 + 10 * digit2 + digit3;
}

Console.WriteLine(sum);

int GetDisplayNumberFuzzy(HashSet<int>[] connections, int[] inputs) {
	foreach (int conn0 in connections[0]) {
		foreach (int conn1 in connections[1]) {
			foreach (int conn2 in connections[2]) {
				foreach (int conn3 in connections[3]) {
					foreach (int conn4 in connections[4]) {
						foreach (int conn5 in connections[5]) {
							foreach (int conn6 in connections[6]) {
								//figure out one valid setup
								var possibleConnections = new []{ conn0, conn1, conn2, conn3, conn4, conn5, conn6};
								//dont repeat connections
								if(possibleConnections.Distinct().Count() != possibleConnections.Length) continue;
								var possibleDisplay = possibleConnections.Select(inputs.Contains).ToArray();
								var num = GetDisplayNumber(possibleDisplay);
								if (num >= 0) return num;
							}
						}
					}
				}
			}
		}
	}
	return -1;
}

int GetDisplayNumber(bool[] display) {
	return displayConfigurations.IndexOf(display.SequenceEqual);
}

bool[] GetNumberDisplay(int number) {
	return displayConfigurations[number];
}

int[] StringToNumbersDisplay(string display) {
	return display.Select(character => character - 'a').ToArray();
}