using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Script
{
    public struct SimpleNode
    {
        public Vector2Int coord;
        public float costFromStart;
        public float cost;

        public SimpleNode(Vector2Int coord, float costFromStart, float cost)
        {
            this.coord = coord;
            this.costFromStart = costFromStart;
            this.cost = cost;
        }
    }

    public class FoundPath
    {
        public List<SimpleNode> path { get; private set; }
        public int Count => path.Count;
        public float finalCost { get; private set; }

        public FoundPath(List<TileNode> tileNodes)
        {
            path = new List<SimpleNode>();
            transformTileNodeInSimpleNode(tileNodes);
        }

        private void transformTileNodeInSimpleNode(List<TileNode> tileNodes)
        {
            if (tileNodes == null || tileNodes.Count == 0) return;

            float totalCost = 0;

            for (int i = 0; i < tileNodes.Count; i++)
            {
                TileNode tileNode = tileNodes[i];
                float cost = (i > 0) ? tileNode.gCost - tileNodes[i - 1].gCost : 0;

                SimpleNode newSimpleNode = new SimpleNode(
                    tileNode.tile.coord,
                    tileNode.gCost,
                    cost
                );

                path.Add(newSimpleNode);
                totalCost = tileNode.gCost; // Le dernier gCost sera le coût final
            }

            finalCost = totalCost;
        }

        public SimpleNode getNode(int index) => path[index];
    }
}
