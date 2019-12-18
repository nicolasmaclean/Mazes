using System.Collections.Generic;

public static class GenerationAlgorithms
{
    public static void BinaryTree(Cell[,] maze)
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
    
    public static void SideWinder(Cell[,] maze)
    {
        int height = maze.GetLength(0);
        int width = maze.GetLength(1);
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
}