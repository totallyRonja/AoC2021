using System.Text.RegularExpressions;

var input = File.ReadAllLines("./input.txt");

var program = input.Select(line => {
	var match = Regex.Match(line, @"^(\w+) (\d)$");
	var amount = int.Parse(match.Groups[2].Value);
	Enum.TryParse(match.Groups[1].Value, true, out Command cmd);
	return (CmdType: cmd, Amount: amount);
}).ToArray();

{
	var pos = (X: 0, Y: 0);
	foreach (var cmd in program) {
		switch (cmd.CmdType) {
			case Command.Down:
				pos.Y += cmd.Amount;
				break;
			case Command.Up:
				pos.Y -= cmd.Amount;
				break;
			case Command.Forward:
				pos.X += cmd.Amount;
				break;
		}
	}
	Console.WriteLine($"first sub: goal position: {pos} | product: {pos.X * pos.Y}");
}

{
	var pos = (X: 0, Y: 0, Aim: 0);
	foreach (var cmd in program) {
		switch (cmd.CmdType) {
			case Command.Down:
				pos.Aim += cmd.Amount;
				break;
			case Command.Up:
				pos.Aim -= cmd.Amount;
				break;
			case Command.Forward:
				pos.X += cmd.Amount;
				pos.Y += cmd.Amount * pos.Aim;
				break;
		}
	}
	Console.WriteLine($"second sub: goal position: {pos} | product: {pos.X * pos.Y}");
}

{
	//the whole thing in a single line/semicolon FOR SOME REASON
	Console.WriteLine(File.ReadAllLines("./input.txt")
		.Select(input => (Type: input.Split(" ")[0], Amount: int.Parse(input.Split(" ")[1])))
		.Aggregate((X:0, Y:0, Aim: 0, Result: 0), (source, instruction) => instruction.Type switch{
			"forward" => (source.X+instruction.Amount, 
				source.Y + source.Aim * instruction.Amount, 
				source.Aim, source.X+instruction.Amount * (source.Y + source.Aim * instruction.Amount)),
			"down" => (source.X, source.Y, 
				source.Aim + instruction.Amount, source.Result),
			"up" => (source.X, source.Y, 
				source.Aim - instruction.Amount, source.Result),
			_ => throw new ArgumentOutOfRangeException(),
		}).Result);
}

enum Command {
	Forward,
	Up,
	Down,
}
