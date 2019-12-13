using System.Collections.Generic;
using UnityEngine;

public class Distances
{
    public Cell root { get; set; }
    public Dictionary<Cell, int> cells;
    Cell[,] maze { get; set;}

    public Distances(Cell root, Cell[,] maze)
    {
        this.root = root;
        this.maze = maze;
        cells = new Dictionary<Cell, int>();
        cells[this.root] = 0;
        // distances();
    }

    public int getDistance(Cell cell)
    {
        return cells[cell];
    }

    public void setDistance(Cell cell, int dist)
    {
        cells[cell] = dist;
    }

    public Dictionary<Cell, int>.KeyCollection getCells()
    {
        return cells.Keys;
    }

    public Dictionary<Cell, int> distances()
    {
        List<Cell> frontier = new List<Cell>();
        frontier.Add(root);

        while(frontier.Count > 0) {
            List<Cell> newFrontier = new List<Cell>();

            for(int i = 0; i < frontier.Count; i++) {
                Dictionary<Cell, bool> links = frontier[i].getLinks();
                Debug.Log(frontier[i].gameObject.name);
                foreach(var item in links) {
                    if(item.Value) {
                        cells[item.Key] = cells[frontier[i]] + 1;
                        newFrontier.Add(item.Key);
                    }
                }
            }
            frontier = newFrontier;
        }

        return cells;
    }

    public string toString()
    {
        string output = "";

        for(int y = maze.GetLength(0)-1; y >= 0; y--) {
            for(int x = 0; x < maze.GetLength(1); x++) {
                if(cells.ContainsKey(maze[y, x]))
                    output += cells[maze[y, x]];
                else
                    output += "-1";
            }
            output += "\n";
        }
        return output;
    }
}