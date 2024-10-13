using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using UnityEngine;
namespace Assets.Script
{

    public class PathFinding : MonoBehaviour
    {
        private TileNode[,] tileMap;
        private int height, width;

        private const int BASE_STRAIGHT_COST = 10;

        private static PathFinding _instance;
        public static PathFinding Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(_instance);
            }
            else
            {
                _instance = this;
            }
        }

        public static void Initialize(int width, int height, TileNode[,] listNodes)
        {
            _instance.tileMap = listNodes;
            _instance.height = height;
            _instance.width = width;
        }

        private void resetPathfindingMap()
        {
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    TileNode tile = tileMap[w, h];
                    tile.gCost = int.MaxValue;
                    tile.hCost = int.MaxValue;
                    tile.cameFrom = null;
                    tile.isClosed = false;
                    tile.isInList = false;
                }
            }
        }
        //Calcul le cout du mouvement en considérant que les deux cases sont adjacentes
        //Retourne -1 si on ne peut pas voyager entre les 2 cases a cause de l'altitude
        private int calculteNeighborTravelCost(TileNode start, TileNode target, GlobalEnum.Direction direction,Unit unit)
        {
            EdgeStruct edgeToCross = start.tile.altitude.elevationType.getEdge(direction);
            EdgeStruct edgeTarget = target.tile.altitude.elevationType.getEdge(GlobalEnum.inverseDirection(direction));

            //Corniche infranchisable
            if (!edgeToCross.isPraticable || !edgeTarget.isPraticable)
            {
                return -1;
            }
            float cost =target.tile.calculateMovementCost(unit);
            return (int)(BASE_STRAIGHT_COST * cost);
        }
        private int calculateDistanceCost(TileNode start, TileNode target)
        {
            int distanceHori = Mathf.Abs(start.tile.coordW - target.tile.coordW);
            int distanceVerti = Mathf.Abs(start.tile.coordH - target.tile.coordW);
            int remainingDistance = distanceVerti + distanceHori;
            return BASE_STRAIGHT_COST * remainingDistance;

        }

        public FoundPath findPath(Unit startUnit, int endWCoord, int endHCoord)
        {
            
            return new FoundPath(findPathWithNode(tileMap[startUnit.getPosition().w, startUnit.getPosition().h], tileMap[endWCoord, endHCoord],startUnit));
        }
        private List<TileNode> findPathWithNode(TileNode startNode, TileNode endNode,Unit unit)
        {
            resetPathfindingMap();
            BinaryHeap openList = new BinaryHeap(width * height);
            startNode.gCost = 0;
            startNode.hCost = calculateDistanceCost(startNode, endNode);
            openList.Add(startNode);
            while (openList.Count > 0)
            {
                TileNode currentNode = openList.ExtractMin();
                currentNode.isClosed = true;
                if (currentNode == endNode)
                {
                    return CalculatePath(currentNode);
                }
                foreach (GlobalEnum.Direction direction in Enum.GetValues(typeof(GlobalEnum.Direction)))
                {
                    TileNode neighborNode = currentNode.getNeighbor(direction);
                    if (neighborNode != null)
                    {
                        if (neighborNode.isClosed) continue;
                        int calculatePath = calculteNeighborTravelCost(currentNode, neighborNode, direction,unit);
                        if (calculatePath < 0)
                        {
                            neighborNode.isClosed = true;
                            continue;
                        };
                        int oldFCost = neighborNode.fCost;
                        int potentialGCost = currentNode.gCost + calculatePath;
                        if (neighborNode.gCost > potentialGCost)
                        {
                            neighborNode.gCost = potentialGCost;
                            neighborNode.cameFrom = currentNode;
                            neighborNode.hCost = calculateDistanceCost(neighborNode, endNode);
                            if (!neighborNode.isInList)
                            {
                                neighborNode.isInList = true;
                                openList.Add(neighborNode);
                            }
                            else
                            {
                                if (oldFCost > neighborNode.fCost)
                                {
                                    openList.DecreasePriority(neighborNode);
                                }
                            }
                        }
                    }
                }
            }
            //Aucun chemin trouvé
            return null;
        }
        private List<TileNode> CalculatePath(TileNode endNode)
        {
            List<TileNode> path = new List<TileNode>
            {
                endNode
            };
            TileNode currentNode = endNode;
            while (currentNode.cameFrom != null)
            {
                path.Add(currentNode.cameFrom);
                currentNode = currentNode.cameFrom;
            }
            path.Reverse();
            return path;
        }
        private TileNode GetLowestFCostNode(List<TileNode> nodes)
        {
            TileNode minTile = nodes[0];
            int nodesLenght = nodes.Count;
            for (int i = 1; i < nodesLenght; i++)
            {
                if (nodes[i].fCost < minTile.fCost)
                {
                    minTile = nodes[i];
                }
            }
            return minTile;
        }
    }
}