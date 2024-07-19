using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevationTile : Tile
{
    public AltitudeType neighboorNorth { get; private set; }
    public AltitudeType neighboorSouth { get; private set; }
    public AltitudeType neighboorEast { get; private set;}
    public AltitudeType neighboorWest { get; private set; }

    public int tileType;

    public int direction;
   public ElevationTile(BiomeType biome, int coordX, int coordY, AltitudeType altitude) : base(biome, coordX, coordY, altitude)
    {
        
    }

}
