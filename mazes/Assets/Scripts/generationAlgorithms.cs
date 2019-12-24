using System.Collections.Generic;
using System.Linq;
using System;
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

    public static void RecursiveBacktracker(Cell[,] maze, Cell root)
    {
        Stack<Cell> stack = new Stack<Cell>();
        stack.Push(root);

        while(stack.Count > 0) {
            Cell cur = stack.Peek();
            List<Cell> neighbors = cur.getNeighborsLink(false);

            if(neighbors.Count == 0) {
                stack.Pop();
            } else {
                Cell neighbor = neighbors[MazeGenerator.random.Next(neighbors.Count)];
                cur.link(neighbor, true);
                stack.Push(neighbor);
            }
        }
    }

    public static void SimplePrim(Cell[,] maze, Cell root, Func<List<Cell>, Cell> selection)
    {
        List<Cell> active = new List<Cell>();
        active.Add(root);

        while(active.Count > 0) {
            Cell cur = selection(active);
            List<Cell> availableNeighbors = cur.getNeighborsLink(false);

            if(availableNeighbors.Count > 0) {
                Cell neighbor = availableNeighbors[MazeGenerator.random.Next(availableNeighbors.Count)];
                cur.link(neighbor, true);
                active.Add(neighbor);
            } else {
                active.Remove(cur);
            }
        }
    }

    public static void TruePrim(Cell[,] maze, Cell root)
    {
        List<Cell> active = new List<Cell>();
        active.Add(root);

        int[,] costs = new int[maze.GetLength(0), maze.GetLength(1)];
        for(int y = 0; y < maze.GetLength(0); y++)
            for(int x = 0; x < maze.GetLength(1); x++)
                costs[y, x] = MazeGenerator.random.Next(100);

        while(active.Count > 0) {
            Cell cur = active.Aggregate( (a, b) => costs[a.y, a.x] < costs[b.y, b.x] ? a : b);
            List<Cell> availableNeighbors = cur.getNeighborsLink(false);

            if(availableNeighbors.Count > 0) {
                Cell neighbor = availableNeighbors.Aggregate( (a, b) => costs[a.y, a.x] < costs[b.y, b.x] ? a : b);
                cur.link(neighbor, true);
                active.Add(neighbor);
            } else {
                active.Remove(cur);
            }
        }
    }

    public static void Eller(Cell[,] maze)
    {

    }

    public static void RecursiveDivision(Cell[,] maze)
    {
        for(int y = 0; y < maze.GetLength(0); y++)
            for(int x = 0; x < maze.GetLength(1); x++)
                foreach(Cell cell in maze[y, x].getNeighbors())
                    maze[y, x].link(cell, false);

        divide(maze, 0, 0, maze.GetLength(0), maze.GetLength(1));
    }

    static void divide(Cell[,] maze, int row, int col, int height, int width)
    {
        if(height <= 1 || width <=1) return;

        if(height > width)
            divideHorizontally(maze, row, col, height, width);
        if(height < width)
            divideVertically(maze, row, col, height, width);
    }

    static void divideHorizontally(Cell[,] maze, int row, int col, int height, int width)
    {
        int divideSouthOf = MazeGenerator.random.Next(height-1);
        int passageAt = MazeGenerator.random.Next(width);

        for(int i = 0; i < width; i++)
            if(passageAt != i) {
                Cell cur = maze[row+divideSouthOf, col+i];
                cur.unlink(cur.south, true);
            }
        divide(maze, row, col, divideSouthOf+1, width);
        divide(maze, row+divideSouthOf+1, col, height-divideSouthOf-1, width);
    }

    static void divideVertically(Cell[,] maze, int row, int col, int height, int width)
    {
        int divideEastOf = MazeGenerator.random.Next(width-1);
        int passageAt = MazeGenerator.random.Next(height);

        for(int i = 0; i < height; i++)
            if(passageAt != i) {
                Cell cur = maze[row+i, col+divideEastOf];
                cur.unlink(cur.east, true);
            }
        divide(maze, row, col, height, divideEastOf+1);
        divide(maze, row, col+divideEastOf+1, height, width-divideEastOf-1);
    }
}