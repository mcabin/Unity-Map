using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using UnityEngine;

public class PathFinding
{
   private TileNode[,] tileMap;
    private int height,width;

    private const int BASE_DIAGONAL_COST = 14;
    private const int BASE_STRAIGHT_COST = 10;

    PathFinding(TileNode[,] map,int height,int width)
    {
        this.tileMap = map;
        this.height = height;
        this.width = width;
    }

    private void resetPathfindingMap()
    {
        for (int w = 0; w < width; w++)
        {
            for(int h = 0; h < height; h++)
            {
                TileNode tile = tileMap[w, h];
                tile.gCost=int.MaxValue;
                tile.hCost=int.MaxValue;
                tile.cameFrom = null;
            }
        }
    }
    //Calcul le cout du mouvement en considérant que les deux cases sont adjacentes
    //Retourne -1 si on ne peut pas voyager entre les 2 cases a cause de l'altitude
    private int calculteNeighborTravelCost(TileNode start,TileNode target)
    {
        int baseCost = BASE_STRAIGHT_COST;
        if (start.tile.coordX - target.tile.coordX != 0 && start.tile.coordY - target.tile.coordY != 0)
            baseCost = BASE_DIAGONAL_COST;

        int cost = target.tile.movementDifficulty;
        int altitudeDifference = start.tile.altitude.level - target.tile.altitude.level;
        if (Mathf.Abs(altitudeDifference) >1)
        {
            return -1;
        }
        else 
            return (int)(baseCost*cost);
    }
    private int calculateDistanceCost(TileNode start,TileNode target)
    {
        int distanceHori=Mathf.Abs(start.tile.coordX-target.tile.coordX);
        int distanceVerti=Mathf.Abs(start.tile.coordY-target.tile.coordX);
        int remainingDistance=Mathf.Abs(distanceVerti-distanceHori);
        return Mathf.Min(distanceHori, distanceVerti)*BASE_DIAGONAL_COST+BASE_STRAIGHT_COST*remainingDistance;

    }
    public List<TileNode> findPath(TileNode startNode, TileNode endNode)
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
            foreach(TileNode neighborNode in currentNode.tilesNeighbors)
            {
                if (closeList.Contains(neighborNode)) continue;
                int calculatePath = calculteNeighborTravelCost(currentNode, neighborNode);
                if (calculatePath < 0) continue;

                int potentialGCost=currentNode.gCost+ calculatePath;
                if (neighborNode.gCost > potentialGCost)
                {
                    neighborNode.gCost = potentialGCost;
                    neighborNode.cameFrom = currentNode;
                    neighborNode.hCost=calculateDistanceCost(neighborNode,endNode);
                    if (!openList.Contains(neighborNode)) {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
        //Aucun chemin trouvé
        return null;
    }
    public List<TileNode> CalculatePath(TileNode endNode)
    {
        List<TileNode> path= new List<TileNode>();
        path.Add(endNode);
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
