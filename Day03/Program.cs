using RonjasToolbox;

var input = File.ReadAllLines("input.txt");

var numbers = input.Select(input => Convert.ToInt16(input, 2)).ToArray();

var gamma = 0;
foreach (int i in 0..12) {
	if (MostCommonBit(numbers, i)) gamma |= 1 << i;
}
var epsilon = (~gamma) & 0x0FFF;

Console.WriteLine($"gamma: {gamma} - epsilon: {epsilon} - power_consumption: {gamma * epsilon}");

var numberMap = new HashSet<short>(numbers);
foreach (var i in (0..12).Iter().Reverse()) {
	var mostCommonBit = MostCommonBit(numberMap, i);
	numberMap.RemoveWhere(number => (((number >> i) & 1) == 1) != mostCommonBit);
	if(numberMap.Count <= 1) break;
}
var oxygenRating = numberMap.First();

numberMap.Clear();
foreach (var i in (0..12).Iter().Reverse()) {
	var mostCommonBit = !MostCommonBit(numberMap, i);
	numberMap.RemoveWhere(number => (((number >> i) & 1) == 1) != mostCommonBit);
	if(numberMap.Count <= 1) break;
}
var co2Scrubber = numberMap.First();

Console.WriteLine($"oxygen: {oxygenRating} - scrubber: {co2Scrubber} - life support: {oxygenRating * co2Scrubber}");


bool MostCommonBit(IEnumerable<short> numbers, int position) {
	var trueBits = numbers.Count(num => ((num >> position) & 1) == 1);
	var falseBits = numbers.Count() - trueBits;
	return trueBits >= falseBits;
}