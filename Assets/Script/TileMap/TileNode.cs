using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TileNode 
{
    public Tile tile{get;private set;}
    //A star
    public int gCost;
    public int hCost;
    public int fCost => gCost + hCost;
    public List<TileNode> tilesNeighbors { get; private set; }
    public TileNode cameFrom;
    public TileNode(Tile tile)
    {
        this.tile= tile;
        tilesNeighbors = new List<TileNode>();
    }
    public void addNeighbor(TileNode neighbor)
    {
        tilesNeighbors.Add(neighbor);
    }
}
