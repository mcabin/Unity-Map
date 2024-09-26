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
    private Dictionary<GlobalEnum.Direction, TileNode> neighborsDic;
    public TileNode cameFrom;
    public TileNode(Tile tile)
    {
        this.tile= tile;
        neighborsDic = new Dictionary<GlobalEnum.Direction,TileNode>();
    }
    public void addNeighbor(GlobalEnum.Direction dir,TileNode node)
    {
        neighborsDic[dir] = node;
    }

    public TileNode getNeighbor(GlobalEnum.Direction dir)
    {
        if (neighborsDic.TryGetValue(dir, out TileNode value))
        {
            return value;
        }
        return null;
    }
}
