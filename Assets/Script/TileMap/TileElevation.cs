using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
using UnityEngine;

public class TileElevation : Tile
{
    public ElevationType elevationType { get; private set; }
    public ElevationType northNeighboor;
    public ElevationType southNeighboor;
    public ElevationType westNeighboor;
    public ElevationType eastNeighboor;

    public TileElevation(BiomeType biome, int coordX, int coordY, AltitudeType altitude) : base(biome, coordX, coordY, altitude)
    {
        this.elevationType = elevationType;
    }
    public void setElevationType(TileEnum.ElevEnum elevEnum) 
    {
        elevationType=TileAsset.getElevationType(elevEnum);
    }
}
