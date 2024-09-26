using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[System.Serializable]
public class Tile
{
    public BiomeType biome;
    public List<TileFeature> features;
    public int coordW, coordH;
    public AltitudeType altitude;
    public int movementDifficulty { get; private set; }
    
    public Tile()
    {

    }
    public Tile(BiomeType biome,int coordW,int coordH,AltitudeType altitude)
    {

        //Biome
        this.biome= biome;

        this.coordW = coordW;
        this.coordH = coordH;

        this.altitude = altitude;
        setMovementDifficulty();
    }
    
    //CALL WHEN CHANGING THE BIOME OR FEATURE
    public void setMovementDifficulty()
    {
        float movDiff = biome.movDifficulty;

        movementDifficulty =(int) Math.Round(movDiff);
    }
}

