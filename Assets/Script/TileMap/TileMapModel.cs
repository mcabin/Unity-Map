using System;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Script
{

    public class TileMapModel
    {
        public int height { get; private set; }
        public int width { get; private set; }
        private Tile[,] tiles;
        private TileNode[,] tilesNodes;
        //Constructor
        public TileMapModel(int width, int height)
        {
            this.height = height;
            this.width = width;
            tiles = new Tile[width, height];
        }


        public Tile GetTile(int w, int h)
        {
            return tiles[w, h];
        }
        public Tile GetTile(Vector2Int vector)
        {
            return this.tiles[vector.x, vector.y];
        }
        public TileNode GetTileNode(int w, int h)
        {
            return tilesNodes[w, h];
        }

        public TileNode GetTileNode(Vector2Int vector)
        {
            return tilesNodes[vector.x, vector.y];
        }
        public TileNode[,] GetTileNodes()
        {
            return tilesNodes;
        }

        public void setTile(int w, int h, Tile tile)
        {
            tiles[w, h] = tile;
        }


    }
}