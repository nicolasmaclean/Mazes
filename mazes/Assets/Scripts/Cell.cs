using UnityEngine;
using System.Collections.Generic;

public class Cell : MonoBehaviour
{
    public int x { get; set; }
    public int y { get; set; }
    // public Cell north, east, south, west;
    public Cell north { get; set;}
    public Cell east { get; set;}
    public Cell south { get; set;}
    public Cell west { get; set;}

    private Dictionary<Cell, bool> links;

    public void initialize(int x, int y)
    {
        this.x = x; this.y = y;
        links = new Dictionary<Cell, bool>();
    }

    public void link(Cell cell, bool bidi) // links cell neighbors
    {
        links[cell] = true;
        if(bidi)
            cell.link(this, false);
    }

    public void unlink(Cell cell, bool bidi) // unlinks cell neighbors
    {
        links[cell] = false;
        if(bidi)
            cell.unlink(gameObject.GetComponent<Cell>(), false);
    }

    public Dictionary<Cell, bool> getLinks() // returns the cells that are linked
    {
        return links;
    }

    public int getLinkCount()
    {
        int num = 0;
        foreach(var item in links)
            if(item.Value) num++;
        return num;
    }

    public bool hasLinks()
    {
        bool anyLinks = false;
        foreach(var item in links)
            if(item.Value)
                anyLinks = true;
        return anyLinks;
    }

    public bool isLinked(Cell cell) // returns if the given cell is linked to the current cell
    {
        bool val = false;
        if(cell != null)
            links.TryGetValue(cell, out val);
        return val;
    }

    public List<Cell> getNeighbors()
    {
        List<Cell> neighbors = new List<Cell>();
        if(east != null) neighbors.Add(east);
        if(north != null) neighbors.Add(north);
        if(south != null) neighbors.Add(south);
        if(west != null) neighbors.Add(west);
        return neighbors;
    }

    public List<Cell> getNeighborsLink(bool link)
    {
        List<Cell> neighbors = new List<Cell>();
        if(east != null && east.hasLinks() == link) neighbors.Add(east);
        if(north != null && north.hasLinks() == link) neighbors.Add(north);
        if(south != null && south.hasLinks() == link) neighbors.Add(south);
        if(west != null && west.hasLinks() == link) neighbors.Add(west);
        return neighbors;
    }

    public Cell getRandomNeighbor()
    {
        List<Cell> neighbors = getNeighbors();
        return neighbors[MazeGenerator.random.Next(neighbors.Count)];
    }

    public void makeWall(int side, GameObject go)
    {
        float x = this.x; float y = this.y; Quaternion q = new Quaternion(0, 0, 0, 1);
        float offset = MazeGenerator.wallLength/2; float turn = 1f;
        string name = "null";
        switch(side) {
            case 0 : y += offset; q = new Quaternion(0, 0, turn, 1); name = "north"; break; // north
            case 1 : x += offset; name = "east"; break; // east
            case 2 : y -= offset; q = new Quaternion(0, 0, turn, 1); name = "south"; break; // south
            case 3 : x -= offset; name = "west"; break; // west
        }
        Instantiate(go, new Vector2(x, y), q, transform).name = name;
    }
}