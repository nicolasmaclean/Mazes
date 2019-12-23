using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public static System.Random random = new System.Random();
    public static float wallWidth { get; set; }
    public static float wallLength { get; set; }

    public bool autoUpdate;
    public int generationNum;
    public int solutionNum;
    public GameObject cellPrefab;
    public GameObject wallPrefab;
    public Material material;
    public int width; public int height;

    public Cell[,] maze;
    public int[,] distances;
    public Cell root; public Cell goal;

    void Start()
    {
        generateMaze();    
    }

    public void deleteMaze()
    {
        for(int i = transform.childCount-1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);
    }

    public void generateMaze()
    {
        deleteMaze();

        // width = cameraZoom.width;
        // height = cameraZoom.height;
        maze = new Cell[height, width];
        wallWidth = wallPrefab.GetComponent<Renderer>().bounds.size.x;
        wallLength = wallPrefab.GetComponent<Renderer>().bounds.size.y; // + wallWidth; do this and make a corner to fill gaps

        prepareGrid();
        configureCells();

        switch(generationNum) {
            case 0 : GenerationAlgorithms.BinaryTree(maze); break;
            case 1 : GenerationAlgorithms.SideWinder(maze); break;
            case 2 : GenerationAlgorithms.AldousBroder(maze, getRandomCell(), getMazeSize()); break;
            case 3 : GenerationAlgorithms.Wilson(maze); break;
            case 4 : GenerationAlgorithms.HuntAndKill(maze, getRandomCell()); break;
        }

        root = maze[0, 0];
        goal = maze[maze.GetLength(0)-1, maze.GetLength(1)-1];

        to3d();
    }

    public void solveMaze()
    {
        switch(solutionNum) {
            case 0 : distances = SolutionAlgorithms.Dijkstra(maze, root); colorMaze(); break;
            case 1 : distances = SolutionAlgorithms.Tremaux(maze, root, goal); showPath(); break;
        }
    }

    public String toString() // an ascii art version of the maze
    { // the font in Unity's console has uneven widths of character, so this doesn't work
        String body = "   "; String corner = "+";
        String wall = "|"; String floor ="---";

        String output = "+";
        for(int i = 0; i < width; i++)
            output += "---+";
        output += "\n";

        for(int y = height - 1; y >= 0; y--) {
            String top = wall; // The middle row text of a cell
            String bottom ="+"; // the bottom row text of a cell
            for(int x = 0; x < width; x++) {
                Cell cur = maze[y, x];

                String east = cur.isLinked(cur.east) ? " " : wall;
                top = top + body + east;

                String south = cur.isLinked(cur.south) ? body : floor;
                bottom += south + corner;
            }
            output += top + "\n" + bottom + "\n";
        }
        return output;
    }

    void prepareGrid()
    {
        for(int y = 0; y < height; y++)
            for(int x = 0; x < width; x++) {
                
                maze[y, x] = Instantiate(cellPrefab, new Vector2(x * wallLength, y * wallLength), new Quaternion(-1f, 0, 0, 1), gameObject.transform).GetComponent<Cell>();
                maze[y, x].initialize(x, y);
                maze[y, x].gameObject.name = "Cell " + (y * width + x);
            }
    }

    void configureCells()
    {
        for(int y = 0; y < height; y++)
            for(int x = 0; x < width; x++) {
                if(x + 1 < width)
                    maze[y, x].east = maze[y, x+1];
                else
                    maze[y, x].east = null;
                if(x - 1 >= 0)
                    maze[y, x].west = maze[y, x-1];
                else
                    maze[y, x].west = null;
                if(y + 1 < height)
                    maze[y, x].north = maze[y+1, x];
                else
                    maze[y, x].north = null;
                if(y - 1 >= 0)
                    maze[y, x].south = maze[y-1, x];
                else
                    maze[y, x].south = null;
            }     
    }

    public List<Cell> deadends()
    {
        List<Cell> list = new List<Cell>();

        for(int y = 0; y < height; y++)
            for(int x = 0; x < width; x++)
                if(maze[y, x].getLinkCount() == 1)
                    list.Add(maze[y, x]);

        return list;
    }

    Cell getRandomCell()
    {
        return maze[random.Next(height), random.Next(width)];
    }

    int getMazeSize()
    {
        return width*height;
    }

    void to3d()
    {
        for(int y = height-1; y >= 0; y--)
            for(int x = 0; x < width; x++) {
                Cell cur = maze[y, x];

                if(cur.north == null) //north walls
                    cur.makeWall(0, wallPrefab);
                if(cur.west == null) // west walls
                    cur.makeWall(3, wallPrefab);

                if(!cur.isLinked(cur.east)) // east walls
                    cur.makeWall(1, wallPrefab);
                if(!cur.isLinked(cur.south)) // south walls
                    cur.makeWall(2, wallPrefab);
            }
    }

    void showPath()
    {
        List<Cell> path = SolutionAlgorithms.getPath(distances, goal);

        foreach(Cell cur in path) {
            cur.transform.Find("Floor").GetComponent<Renderer>().material = material;
        }
    } 
    
    void colorMaze()
    {
        List<Cell> path = SolutionAlgorithms.getLongestPath(maze, distances);
        goal = path[0]; root = path[path.Count-1];

        Cell furthestCell = SolutionAlgorithms.getMax(maze, distances);
        int furthestDist = distances[(int)furthestCell.y, (int)furthestCell.x];

        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                maze[y, x].transform.Find("Floor").GetComponent<Renderer>().material.color = Color.Lerp(Color.white, material.color, (float)distances[y, x] / furthestDist);
            }
        }
    }
}
