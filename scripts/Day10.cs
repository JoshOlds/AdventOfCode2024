using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Day10 : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Day 10 Part 1: ");
		ExecutePart1();
		GD.Print("\nDay 10 Part 2: ");
		ExecutePart2();
	}

	private void ExecutePart1()
	{
		string puzzleInput = GetPuzzleExample();
		int[,] trailMap = GetInputAs2dArray(puzzleInput);
		int mapX = trailMap.GetLength(0);
		int mapY = trailMap.GetLength(1);
		
		int scoreSum = 0;
		int[,] scoreMap = new int[mapX, mapY];
		for (int y = 0; y < mapY; y++)
		{
			for (int x = 0; x < mapX; x++)
			{
				if (trailMap[x, y] == 0)
				{
					int score = EvaluateTrail(new Coord(x, y), trailMap, mapX, mapY);
					scoreSum += score;
					scoreMap[x, y] = score;
				}
			}
		}
		GD.Print(scoreSum);
	}

	private void ExecutePart2()
	{
		
	}

	private int EvaluateTrail(Coord startPosition, int[,] trailMap, int mapWidth, int mapHeight)
	{
		int peaksDiscovered = 0;
		int currentHeight = trailMap[startPosition.X, startPosition.Y];
		if (currentHeight == 9) return 0;

		// Evaluate all 9 directions and recurse
		List<Coord> adjacentTiles = new List<Coord>();
		//Coord upLeft = new Coord(startPosition.X - 1, startPosition.Y - 1);
		//adjacentTiles.Add(upLeft);
		Coord upCenter = new Coord(startPosition.X, startPosition.Y - 1);
		adjacentTiles.Add(upCenter);
		//Coord upRight = new Coord(startPosition.X + 1, startPosition.Y - 1);
		//adjacentTiles.Add(upRight);
		Coord midLeft = new Coord(startPosition.X - 1, startPosition.Y);
		adjacentTiles.Add(midLeft);
		Coord midRight = new Coord(startPosition.X + 1, startPosition.Y);
		adjacentTiles.Add(midRight);
		//Coord downLeft = new Coord(startPosition.X - 1, startPosition.Y + 1);
		//adjacentTiles.Add(downLeft);
		Coord downCenter = new Coord(startPosition.X, startPosition.Y + 1);
		adjacentTiles.Add(downCenter);
		//Coord downRight = new Coord(startPosition.X + 1, startPosition.Y + 1);
		//adjacentTiles.Add(downRight);

		foreach (Coord coord in adjacentTiles)
		{
			if (coord.X >= 0 && coord.X < mapWidth && coord.Y >= 0 && coord.Y < mapHeight)
			{
				if (trailMap[coord.X, coord.Y] == currentHeight + 1)
				{
					if (currentHeight == 8) peaksDiscovered++;
					else peaksDiscovered += EvaluateTrail(coord, trailMap, mapWidth, mapHeight);
				}
			}
		}

		return peaksDiscovered;
	}

	private int[,] GetInputAs2dArray(string input)
	{
		List<string> lines = input.Split("\n").ToList();
		int arrayHeight = lines.Count;
		int arrayWidth = lines[0].Length;
		int[,] outArray = new int[arrayWidth, arrayHeight];
		int xIdx = 0;
		int yIdx = 0;
		foreach (string line in lines)
		{
			foreach (char c in line)
			{
				outArray[xIdx, yIdx] = c - '0'; // Converts ascii to decimal
				xIdx++;
			}
			yIdx++;
			xIdx = 0;
		}

		return outArray;
	}

	private string GetPuzzleExample()
	{
		return @"89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732";
	}

	private string GetPuzzleInput()
	{
		return @"12109832101432101234107652158943210178765892
03078456210145696701218943067654396549456701
54562364345436789874327832107810387630345210
65401875696925210765498017656901234521254321
78078956787814321544567328943217890012189450
69101045698701055432123410812206921983098765
43232132509652566785010569701105435674549056
58943001419143478994321678983210304105678143
67653214328012988765690787894321213289437212
45654301037001089650787096765010034576524301
56789890156121072341256101896654123678915498
43276765243234561212345234987783210569206789
54109854312789870109012345676898765454106543
45610123203650105438721056765609674323287012
54781010154543216521635489832014589210398013
67898543269854107610544376541023008101296323
54987656576765678923455210458782112010387456
23122189983454989012966904349698103465456567
12033078012763210101877813234521098578956798
03944565430887654012109320121034787632347897
87856556021991047121238458945695698961036016
96587432110872338930347567232780087654105125
01498983321265427945656089101091109803234934
32327465456766016859890176232892256712107843
21012334569854105766763245001743343893256765
30503129678945234897854632122654872894349854
45614068798234012656906543213458961783210703
21765878907178723765417891008965450654125612
30854965416069654894328982567872342103054503
48903010325450560761237813450561003276543678
56012321210341981230106504341540214789432189
67329630121212870341012415432634345695321012
78478742198903965494543326998723456786540765
89569653087654654987696547889010567847830874
21052104676501723898587032378765676956921923
32343015685432810767698121459034982349650010
10478723794354903456567030760121061078744567
21569654891263212347450177898267877101233498
32108765430678903038321789783454978715012399
47899834320545676129012876012543269856101087
56938723011230983543903965987650156747801256
40127619654321012652874854107890349832954343
30034508763018723761765543236501212721096501
21065619012349654890101234565432301430787432";
	}
}
