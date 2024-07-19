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
    public int coordX, coordY;
    public AltitudeType altitude;
    public int movementDifficulty { get; private set; }
    
    public Tile()
    {

    }
    public Tile(BiomeType biome,int coordX,int coordY,AltitudeType altitude)
    {

        //Biome
        this.biome= biome;

        this.coordX = coordX;
        this.coordY = coordY;

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

