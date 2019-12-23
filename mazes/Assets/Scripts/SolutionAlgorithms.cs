using System.Collections.Generic;
using UnityEngine;

public static class SolutionAlgorithms
{
    public static int[,] Dijkstra(Cell[,] maze, Cell root)
    {
        int[,] distances = new int[maze.GetLength(0), maze.GetLength(1)];
        List<Cell> frontier = new List<Cell>(); frontier.Add(root);

        while(frontier.Count > 0) {
            List<Cell> newFrontier = new List<Cell>();

            foreach(Cell cur in frontier) { 
                Dictionary<Cell, bool> links = cur.getLinks();
                foreach(var item in links) {
                    if(item.Value && distances[item.Key.y, item.Key.x] == 0 && item.Key != root) {
                        distances[item.Key.y, item.Key.x] = distances[cur.y, cur.x] + 1;
                        newFrontier.Add(item.Key);
                    }
                }
                frontier = newFrontier;
            }
        }

        return distances;
    }

    public static List<Cell> Tremaux(Cell[,] maze, Cell root, Cell goal)
    {
        List<Cell> path = new List<Cell>();
        Cell cur = root; path.Add(cur);

        while(cur != goal) {
            cur = goal;            
        }

        return path;
    }

    public static List<Cell> getPath(int[,] distances, Cell goal)
    {
        List<Cell> path = new List<Cell>();
        Cell cur = goal; path.Add(cur);

        while(distances[cur.y, cur.x] != 0) {
            Dictionary<Cell, bool> links = cur.getLinks();
            foreach(var item in links) {
                if(distances[item.Key.y, item.Key.x] < distances[cur.y, cur.x]) {
                    path.Add(item.Key);
                    cur = item.Key;
                }
            }
        }

        return path;
    }

    public static Cell getMax(Cell[,] maze, int[,] distances)
    {
        Cell maxCell = maze[0,0]; int maxDist = 0;

        for(int y = 0; y < distances.GetLength(0); y++)
            for(int x = 0; x < distances.GetLength(1); x++)
                if(maxDist < distances[y, x]) {
                    maxCell = maze[y, x]; maxDist = distances[y, x];
                }
                
        return maxCell;
    }

    public static List<Cell> getLongestPath(Cell[,] maze, int[,] distances)
    {
        List<Cell> path = new List<Cell>();

        Cell root = getMax(maze, distances);

        distances = Dijkstra(maze, root);
        Cell goal = getMax(maze, distances);
        
        return getPath(distances, goal);
    }
}
