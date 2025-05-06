using Assets.Script;
using System;
using UnityEngine;

public class TileNode : IComparable<TileNode>
{
    public Tile tile { get; private set; }

    // A* Costs
    public int gCost;
    public int hCost;
    public bool isInList;
    public bool isClosed;
    public TileNode cameFrom;

    public int fCost => gCost + hCost;

    // Position dans le tas binaire (si utilisé)
    public int positionInHeap;

    // Tableau fixe pour les voisins (8 directions max, indexé par l'énumération)
    private TileNode[] neighbors;

    public TileNode(Tile tile)
    {
        this.tile = tile;
        neighbors = new TileNode[Enum.GetValues(typeof(GlobalEnum.Direction)).Length];
    }

    public void AddNeighbor(GlobalEnum.Direction dir, TileNode node)
    {
        neighbors[(int)dir] = node;
    }

    public TileNode GetNeighbor(GlobalEnum.Direction dir)
    {
        return neighbors[(int)dir];
    }

    public int CompareTo(TileNode other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            return hCost.CompareTo(other.hCost); // Priorité à la case la plus proche de la cible
        }
        return compare;
    }

    public void Reset()
    {
        gCost = int.MaxValue;
        hCost = int.MaxValue;
        cameFrom = null;
        isClosed = false;
        isInList = false;
    }
}
