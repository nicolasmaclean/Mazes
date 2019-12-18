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
                    Debug.Log(item.Key);
                    if(item.Value) {
                        distances[item.Key.y, item.Key.x] = distances[cur.y, cur.x] + 1;
                        newFrontier.Add(item.Key);
                    }
                }
            }
        }

        return distances;
    }

    public static List<Cell> getPath(int[,] distances, Cell goal)
    {
        List<Cell> path = new List<Cell>();
        Cell cur = goal;

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
}
