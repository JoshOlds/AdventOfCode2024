// High level concept for today was to represent Antennae as objects with coordinate data. Then create a dictionary
// with coordinates as keys, and antinode count as values. The rest is simply working through the math to solve
// antinode locations based on two antenna/
// I did a bit of extra work to make sure we only counted unique antenna pairs, as I thought part 2 would end up wanting
// counts of overlapping nodes, but alas, this work was not needed.

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

class Antenna
{
    public Antenna(Coord coordinate, char typeChar, int id)
    {
        Coordinate = coordinate;
        TypeChar = typeChar;
        ID = id;
        PairedAntennae = new List<Antenna>();
    }

    public Coord Coordinate;
    public char TypeChar;
    public int ID;
    public List<Antenna> PairedAntennae;
}

public partial class day8 : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Execute();
    }

    public void Execute()
    {
        // Set up puzzle input and dimensions
        string input = GetPuzzleInput();
        var inputArr = input.Split("\n");
        int inputWidth = inputArr[0].Length;
        int inputHeight = inputArr.Length;

        // Create antenna objects and pairs based on puzzle input
        List<Antenna> antennae = GetAntennae(input);
        List<(Antenna, Antenna)> uniqueAntennaPairs = GetUniqueAntennaPairs(antennae);

        // Dictionary of each Coordinate that contains an Antinode. Dict lets us count unique locations as well as overlap quantity
        Dictionary<Coord, int> antinodes = new Dictionary<Coord, int>();

        // Iterate through antenna pairs and populate the dictionary
        foreach ((Antenna, Antenna) antennaPair in uniqueAntennaPairs)
        {
            (Coord, Coord) antinodePair = GetAntinodes(antennaPair.Item1, antennaPair.Item2);
            if (antinodes.ContainsKey(antinodePair.Item1))
                antinodes[antinodePair.Item1] = antinodes[antinodePair.Item1]++;
            else antinodes[antinodePair.Item1] = 1;
            if (antinodes.ContainsKey(antinodePair.Item2))
                antinodes[antinodePair.Item2] = antinodes[antinodePair.Item2]++;
            else antinodes[antinodePair.Item2] = 1;
        }

        // Count unique nodes that are inside the map
        int uniqueNodes = 0;
        foreach (KeyValuePair<Coord, int> node in antinodes)
        {
            if (node.Key.Y >= 0 && node.Key.Y < inputHeight && node.Key.X >= 0 &&
                node.Key.X < inputWidth)
            {
                uniqueNodes++;
            }
        }
        GD.Print("Day 8 Part 1: ");
        GD.Print(uniqueNodes);
        
        // Reset the antinodes dictionary
        antinodes = new Dictionary<Coord, int>();
        // Iterate through antenna pairs and populate the dictionary (using line function this time)
        foreach ((Antenna, Antenna) antennaPair in uniqueAntennaPairs)
        {
            List<Coord> lineNodes = GetAntinodeLines(antennaPair.Item1, antennaPair.Item2, inputWidth, inputHeight);
            foreach (Coord coord in lineNodes)
            {
                if (antinodes.ContainsKey(coord)) antinodes[coord] = antinodes[coord]++;
                else antinodes[coord] = 1;
            }
        }
        // Count unique nodes that are inside the map
        uniqueNodes = 0;
        foreach (KeyValuePair<Coord, int> node in antinodes)
        {
            if (node.Key.Y >= 0 && node.Key.Y < inputHeight && node.Key.X >= 0 &&
                node.Key.X < inputWidth)
            {
                uniqueNodes++;
            }
        }
        GD.Print("Day 8 Part 2: ");
        GD.Print(uniqueNodes);
    }
    
    // Parses out Antennae from the map input. 
    private List<Antenna> GetAntennae(string input)
    {
        List<Antenna> antennae = new List<Antenna>();
        List<string> lines = input.Split("\n").ToList();
        int idIncrement = 0;
        for (int y = 0; y < lines.Count; y++)
        {
            string line = lines[y];
            for (int x = 0; x < lines[0].Length; x++)
            {
                if (line[x] != '.')
                {
                    Antenna ant = new Antenna(new Coord(x, y), line[x], idIncrement);
                    idIncrement++;
                    antennae.Add(ant);
                }
            }
        }

        return antennae;
    }

    // Calculates the two antinodes for a given pair of antennae (according to part1 rules)
    private (Coord, Coord) GetAntinodes(Antenna firstAnt, Antenna secondAnt)
    {
        int xDist = firstAnt.Coordinate.X - secondAnt.Coordinate.X;
        int yDist = firstAnt.Coordinate.Y - secondAnt.Coordinate.Y;
        Coord firstAntinode = new Coord(firstAnt.Coordinate.X + xDist, firstAnt.Coordinate.Y + yDist);
        Coord secondAntinode = new Coord(secondAnt.Coordinate.X - xDist, secondAnt.Coordinate.Y - yDist);
        return (firstAntinode, secondAntinode);
    }

    // Calculates all Antinodes in a line for a given pari of antennae (according to part2 rules)
    private List<Coord> GetAntinodeLines(Antenna firstAnt, Antenna secondAnt, int xBound, int yBound)
    {
        List<Coord> nodes = new List<Coord>();
        nodes.Add(firstAnt.Coordinate);
        nodes.Add(secondAnt.Coordinate);
        int xDist = firstAnt.Coordinate.X - secondAnt.Coordinate.X;
        int yDist = firstAnt.Coordinate.Y - secondAnt.Coordinate.Y;
        
        bool withinBounds = true;
        int multiplier = 1;
        while (withinBounds)
        {
            Coord newNode = new Coord(firstAnt.Coordinate.X + (xDist * multiplier), firstAnt.Coordinate.Y + (yDist * multiplier));
            if (newNode.X < 0 || newNode.X >= xBound || newNode.Y < 0 || newNode.Y >= yBound) withinBounds = false;
            else nodes.Add(newNode);
            multiplier++;
        }

        withinBounds = true;
        multiplier = 1;
        while (withinBounds)
        {
            Coord newNode = new Coord(secondAnt.Coordinate.X - (xDist * multiplier), secondAnt.Coordinate.Y - (yDist * multiplier));
            if (newNode.X < 0 || newNode.X >= xBound || newNode.Y < 0 || newNode.Y >= yBound) withinBounds = false;
            else nodes.Add(newNode);
            multiplier++;
        }

        return nodes;
    }

    // Creates a list of unique antenna pairs. 
    // This ended up not being needed as we only need to know if a tile has a node - not how many overlap on each tile.
    private List<(Antenna, Antenna)> GetUniqueAntennaPairs(List<Antenna> antennae)
    {
        List<(Antenna, Antenna)> antennapairs = new List<(Antenna, Antenna)>();
        foreach (Antenna antenna in antennae)
        {
            foreach (Antenna other in antennae)
            {
                if (antenna.ID != other.ID && antenna.TypeChar == other.TypeChar)
                {
                    // Only add this pair if it is unique
                    if (other.PairedAntennae.Contains(antenna) == false)
                    {
                        other.PairedAntennae.Add(antenna);
                        antenna.PairedAntennae.Add(other);
                        antennapairs.Add((antenna, other));
                    }
                }
            }
        }

        return antennapairs;
    }

    private string GetPuzzleInput()
    {
        return @"......................D....B...h..................
..............................h...................
.............D...3.....X..................9.......
...........C........X....2.hB......v........b.....
....................................O.............
......u.....3.........p...........................
....u......................v....6.................
......................y..D.....Ov.2..............b
.....u..........X...........o........y............
.........................y...B.f...........s......
.7....................C.2.....Bsyp..........t...q.
.u.7...........X............................Oe..t.
...........V........3......6v.s........o....h....t
..E........L.................6..........o......9..
........E......m.2.P.......O...9...8....b.........
..m..........3.......p..........M8................
..1.....................K.p....................b.e
5...............L...........s.6..........S.M......
....5..1.......E.........k.f.........M............
.E..Y..V......l.......T...D.......9....Q..........
..............................M...................
.....5....P................m...x..q......F......e.
................f...c......................x..F...
..V.C...........7.......a....o....8.........F.....
.......4....L.a..g..P.....8......Q....7d..........
...1......4..a............k......t...d............
..........V..........L....m........K....Q........S
..................1....k.....T....................
..........l......a...............F................
...........P...4.......l......x...................
.............c....g........T......................
.....g............c...Q.......................S...
...............l..................A.d.T.U.........
..........................f...0.............d.....
..........G..................A............e.S...x.
.........Y.......q........g....K..................
.....................q.H4...0.................j...
....................HA..............J.............
..Y..........................0...J.......j........
.......................G.JA...................U...
.......5..........................................
...........c..............G.........K.............
...............................G..................
...........................0.j....................
............................H.......k..........U..
.........................H........................
...................................Y....J.........
..................................j...............
..................................................
..................................................";
    }
}