using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Script
{
    public struct SimpleNode
    {
        public int coordW;
        public int coordH;
        public float costFromStart;
        public float cost;
        public SimpleNode(int coordW, int coordH,int costFromStart, int cost)
        {
            this.coordW = coordW;
            this.coordH = coordH;
            this.costFromStart = costFromStart;
            this.cost = cost;
        }
    }
    public class FoundPath 
    {
        public List<SimpleNode> path;
        private int _count;
        public int Count { get { return _count; } }
        public float finalCost{get;private set;}
        public FoundPath(List<TileNode> path) {
            transformTileNodeInSimpleNode(path);
            _count = path.Count;
        }

        private void transformTileNodeInSimpleNode(List<TileNode> tileNodes)
        {
            path=new List<SimpleNode>();
            int totalCost=0;
            foreach (var tileNode in tileNodes)
            {
                int cost = tileNode.gCost - tileNode.cameFrom.gCost;
                totalCost = tileNode.gCost;
                SimpleNode newSimpleNode=new SimpleNode(tileNode.tile.coordW,tileNode.tile.coordH,tileNode.gCost,cost);
            }
            finalCost = totalCost;
        }

        public SimpleNode getNode(int index) { return path[index]; }
    }
}