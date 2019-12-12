using System.Collections.Generic;

public static class BinaryTree
{
    public static void generate(Cell[,] maze)
    {
        int height = maze.GetLength(0);
        int width = maze.GetLength(1);
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
}