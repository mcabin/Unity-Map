using System;
using System.Collections.Generic;
using UnityEngine;
using static TileEnum;

public class TileMapModel
{
    public int height { get; private set; }
    public int width{get;private set;}
    private Tile[,] tiles;
    private TileNode[,] tilesNodes;
    //Constructor
    public TileMapModel(int width, int height)
    {
        this.height = height;
        this.width = width;
        tiles=new Tile[width,height];
    }


    public Tile getTile(int w,int h)
    {
        return tiles[w, h];
    }
    public TileNode getTileNode(int w,int h)
    {
        return tilesNodes[w,h];
    }
    public TileNode[,] GetTileNodes()
    {
        return tilesNodes;
    }

    public void setTile(int w, int h,Tile tile) { 
        tiles[w, h] = tile; 
    }

   
}
