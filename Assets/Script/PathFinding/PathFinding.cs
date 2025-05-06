using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class PathFinding : MonoBehaviour
    {
        private TileNode[,] tileMap;
        private int height, width;
        private const int BASE_STRAIGHT_COST = 10;
        private static readonly GlobalEnum.Direction[] Directions =
            (GlobalEnum.Direction[])Enum.GetValues(typeof(GlobalEnum.Direction));
        private static PathFinding _instance;
        public static PathFinding Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        public static void Initialize(int width, int height, TileNode[,] listNodes)
        {
            _instance.tileMap = listNodes;
            _instance.height = height;
            _instance.width = width;
        }

        private void ResetPathfindingMap(List<TileNode> visitedNodes)
        {
            foreach (var node in visitedNodes)
            {
                node.gCost = int.MaxValue;
                node.hCost = int.MaxValue;
                node.cameFrom = null;
                node.isClosed = false;
                node.isInList = false;
            }
        }

        private int CalculateNeighborTravelCost(TileNode start, TileNode target, GlobalEnum.Direction direction, Unit unit)
        {
            EdgeStruct edgeToCross = start.tile.altitude.elevationType.getEdge(direction);
            EdgeStruct edgeTarget = target.tile.altitude.elevationType.getEdge(GlobalEnum.inverseDirection(direction));
            if (!edgeToCross.isPraticable || !edgeTarget.isPraticable) return -1;
            return (int)(BASE_STRAIGHT_COST * target.tile.calculateMovementCost(unit));
        }

        private int CalculateDistanceCost(TileNode start, TileNode target)
        {
            return BASE_STRAIGHT_COST * (Mathf.Abs(start.tile.coord.x - target.tile.coord.x) + Mathf.Abs(start.tile.coord.y - target.tile.coord.y));
        }

        public FoundPath FindPath(Unit startUnit, Vector2Int endCoord)
        {
            if (endCoord.x < 0 || endCoord.x >= width || endCoord.y < 0 || endCoord.y >= height)
            {
                Debug.LogError("Coordonnées de destination hors limites !");
                return new FoundPath(null);
            }
            return new FoundPath(FindPathWithNode(tileMap[startUnit.coord.x, startUnit.coord.y], tileMap[endCoord.x, endCoord.y], startUnit));
        }

        private List<TileNode> FindPathWithNode(TileNode startNode, TileNode endNode, Unit unit)
        {
            List<TileNode> visitedNodes = new List<TileNode>();
            ResetPathfindingMap(visitedNodes);
            BinaryHeap openList = new BinaryHeap(width * height);
            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            openList.Add(startNode);
            visitedNodes.Add(startNode);

            while (openList.Count > 0)
            {
                TileNode currentNode = openList.ExtractMin();
                currentNode.isClosed = true;
                if (currentNode == endNode)
                {
                    return ReconstructPath(currentNode);
                }
                foreach (GlobalEnum.Direction direction in Directions)
                {
                    TileNode neighborNode = currentNode.GetNeighbor(direction);
                    if (neighborNode == null || neighborNode.isClosed) continue;

                    int travelCost = CalculateNeighborTravelCost(currentNode, neighborNode, direction, unit);
                    if (travelCost < 0) continue;

                    int potentialGCost = currentNode.gCost + travelCost;
                    if (neighborNode.gCost > potentialGCost)
                    {
                        neighborNode.gCost = potentialGCost;
                        neighborNode.cameFrom = currentNode;
                        neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                        visitedNodes.Add(neighborNode);

                        if (!neighborNode.isInList)
                        {
                            neighborNode.isInList = true;
                            openList.Add(neighborNode);
                        }
                        else
                        {
                            openList.DecreasePriority(neighborNode);
                        }
                    }
                }
            }
            return null;
        }

        private List<TileNode> ReconstructPath(TileNode endNode)
        {
            List<TileNode> path = new List<TileNode>();
            for (TileNode currentNode = endNode; currentNode != null; currentNode = currentNode.cameFrom)
            {
                path.Add(currentNode);
            }
            path.Reverse();
            return path;
        }
    }
}