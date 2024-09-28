using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using UnityEngine;

public class PathFinding:MonoBehaviour
{
   private TileNode[,] tileMap;
    private int height,width;

    private const int BASE_STRAIGHT_COST = 10;

    
    public void Initialize(int width,int height, TileNode[,] listNodes)
    {
        tileMap = listNodes;
        this.height=height;
        this.width=width;
    }

    private void resetPathfindingMap()
    {
        for (int w = 0; w < width; w++)
        {
            for(int h = 0; h < height; h++)
            {
                TileNode tile = tileMap[w,h];
                tile.gCost=int.MaxValue;
                tile.hCost=int.MaxValue;
                tile.cameFrom = null;
            }
        }
    }
    //Calcul le cout du mouvement en considérant que les deux cases sont adjacentes
    //Retourne -1 si on ne peut pas voyager entre les 2 cases a cause de l'altitude
    private int calculteNeighborTravelCost(TileNode start,TileNode target,GlobalEnum.Direction direction)
    {
        EdgeStruct edgeToCross= start.tile.altitude.elevationType.getEdge(direction);
        EdgeStruct edgeTarget = target.tile.altitude.elevationType.getEdge(GlobalEnum.inverseDirection(direction));

        //Corniche infranchisable
        if (!edgeToCross.isPraticable||!edgeTarget.isPraticable) { 
            return -1;
        }
        int cost = target.tile.movementDifficulty;
        return (int)(BASE_STRAIGHT_COST * cost);
    }
    private int calculateDistanceCost(TileNode start,TileNode target)
    {
        int distanceHori=Mathf.Abs(start.tile.coordW-target.tile.coordW);
        int distanceVerti=Mathf.Abs(start.tile.coordH-target.tile.coordW);
        int remainingDistance=distanceVerti+distanceHori;
        return BASE_STRAIGHT_COST*remainingDistance;

    }

    public List<TileNode> findPath(int startWCoord,int startHCoord,int endWCoord,int endHCoord) {
        return findPathWithNode(tileMap[startWCoord, startHCoord], tileMap[endWCoord, endHCoord]);
    }
    private List<TileNode> findPathWithNode(TileNode startNode, TileNode endNode)
    {
        resetPathfindingMap();
        List<TileNode> openList = new List<TileNode> { startNode };
        List<TileNode> closeList = new List<TileNode>();
        startNode.gCost = 0;
        startNode.hCost = calculateDistanceCost(startNode, endNode);
        while (openList.Count > 0)
        {
            TileNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(currentNode);
            }
            openList.Remove(currentNode);
            closeList.Add(currentNode);
            foreach (GlobalEnum.Direction direction in Enum.GetValues(typeof(GlobalEnum.Direction)))
            {
                TileNode neighborNode =currentNode.getNeighbor(direction);
                if (neighborNode != null)
                {
                    if (closeList.Contains(neighborNode)) continue;
                    int calculatePath = calculteNeighborTravelCost(currentNode, neighborNode,direction);
                    if (calculatePath < 0) continue;

                    int potentialGCost = currentNode.gCost + calculatePath;
                    if (neighborNode.gCost > potentialGCost)
                    {
                        neighborNode.gCost = potentialGCost;
                        neighborNode.cameFrom = currentNode;
                        neighborNode.hCost = calculateDistanceCost(neighborNode, endNode);
                        if (!openList.Contains(neighborNode))
                        {
                            openList.Add(neighborNode);
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
        List<TileNode> path= new List<TileNode>
        {
            endNode
        };
        TileNode currentNode = endNode;
        while (currentNode.cameFrom != null) {
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
             if(nodes[i].fCost < minTile.fCost)
            {
                minTile = nodes[i];
            }
        }
        return minTile;
    }
}
