using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Script
{
    [System.Serializable]
    public class TileNode : IComparable<TileNode>
    {
        public Tile tile { get; private set; }
        //A star
        public int gCost;
        public int hCost;

        public bool isInList;
        public bool isClosed;

        public int positionInHeap;
        public int fCost => gCost + hCost;
        private Dictionary<GlobalEnum.Direction, TileNode> neighborsDic;
        public TileNode cameFrom;
        public TileNode(Tile tile)
        {
            this.tile = tile;
            neighborsDic = new Dictionary<GlobalEnum.Direction, TileNode>();
        }
        public void addNeighbor(GlobalEnum.Direction dir, TileNode node)
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

        public int CompareTo(TileNode other)
        {
            return fCost.CompareTo(other.fCost);
        }
    }
}
