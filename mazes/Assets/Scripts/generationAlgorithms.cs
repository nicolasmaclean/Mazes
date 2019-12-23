using System.Collections.Generic;
using UnityEngine;

public static class GenerationAlgorithms
{
    public static int width = -1;
    public static int height = -1;

    public static void BinaryTree(Cell[,] maze)
    {
        if(width == -1) width = maze.GetLength(1);
        if(height == -1) height = maze.GetLength(0);

        for(int y = 0; y < height; y++)
            for(int x = 0; x < width; x++) {
                List<Cell> neighbors = new List<Cell>();
                if(x + 1 < width)
                    neighbors.Add(maze[y, x].east);
                if(y + 1 < height)
                    neighbors.Add(maze[y, x].north);
                if(neighbors.Count > 0) {
                    Cell neighbor = neighbors[MazeGenerator.random.Next(neighbors.Count)];
                    maze[y, x].link(neighbor, true);
                }
            }
    }

    public static void BinaryTreeStep(Cell[,] maze, int y, int x) // an unused attempt for stepping through generation
    {
        if(width == -1) width = maze.GetLength(1);
        if(height == -1) height = maze.GetLength(0);

        if(y < maze.GetLength(0)) {
            List<Cell> neighbors = new List<Cell>();
            if(x + 1 < width)
                neighbors.Add(maze[y, x].east);
            if(y + 1 < height)
                neighbors.Add(maze[y, x].north);
            if(neighbors.Count > 0) {
                Cell neighbor = neighbors[MazeGenerator.random.Next(neighbors.Count)];
                maze[y, x].link(neighbor, true);
            }
        }
    }
    
    public static void SideWinder(Cell[,] maze)
    {
        if(width == -1) width = maze.GetLength(1);
        if(height == -1) height = maze.GetLength(0);

        for(int y = 0; y < height; y++) {
            List<Cell> run = new List<Cell>();

            for(int x = 0; x < width; x++) {
                Cell cur = maze[y, x];
                run.Add(cur);
                bool atEastWall = cur.east == null;
                bool atNorthWall = cur.north == null;

                if(atEastWall || (!atNorthWall && MazeGenerator.random.Next(2) == 0)) {
                    Cell member = run[MazeGenerator.random.Next(run.Count)];
                    if(member.north != null)
                        member.link(member.north, true);
                    run.Clear();
                } else {
                    cur.link(cur.east, true);
                }
            }
        }
    }

    public static void AldousBroder(Cell[,] maze, Cell start, int size)
    {
        Cell cur = start; int unvisited = size - 1;

        while(unvisited > 0) {
            Cell neighbor = cur.getRandomNeighbor();

            if(!neighbor.hasLinks()) {
                cur.link(neighbor, true);
                unvisited--;
            }

            cur = neighbor;
        }
    }

    public static void Wilson(Cell[,] maze) // doesn't quite work
    {
        List<Cell> unvisited = new List<Cell>();

        for(int y = 0; y < maze.GetLength(0); y++)
            for(int x = 0; x < maze.GetLength(1); x++)
                unvisited.Add(maze[y, x]);
        
        Cell first = unvisited[MazeGenerator.random.Next(unvisited.Count)];
        unvisited.Remove(first);

        while(unvisited.Count > 0) {
            Cell cur = unvisited[MazeGenerator.random.Next(unvisited.Count)];
            List<Cell> path = new List<Cell>(); path.Add(cur);

            while(unvisited.Contains(cur)) {
                cur = cur.getRandomNeighbor();
                int pos = path.IndexOf(cur);
                if(pos != -1) path.RemoveRange(pos, path.Count - pos);
                else path.Add(cur);
            }

            for(int i = 0; i < path.Count-1; i++) {
                path[i].link(path[i+1], true);
                unvisited.Remove(path[i]);
            }
        }
    }

    public static void HuntAndKill(Cell[,] maze, Cell root)
    {
        Cell cur = root;

        while(cur) {
            List<Cell> unvisitedNeighbors = cur.getNeighborsLink(false);

            if(unvisitedNeighbors.Count > 0) {
                Cell neighbor = unvisitedNeighbors[MazeGenerator.random.Next(unvisitedNeighbors.Count)];
                cur.link(neighbor, true);
                cur = neighbor;
            } else {
                cur = null;

                for(int y = maze.GetLength(0)-1; y >= 0; y--)
                    for(int x = 0; x < maze.GetLength(1); x++) {
                        List<Cell> visitedNeighbors = maze[y, x].getNeighborsLink(true);
                        if(!maze[y, x].hasLinks() && visitedNeighbors.Count > 0) {
                            cur = maze[y, x];

                            Cell neighbor = visitedNeighbors[MazeGenerator.random.Next(visitedNeighbors.Count)];
                            cur.link(neighbor, true);

                            break;
                        }
                    }
            }
        }
    }
}