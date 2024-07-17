using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BiomeType
{
    public TileEnum.BiomeEnum type;
    public float movDifficulty;
    public int minSpawnTemperature;
    public int maxSpawnTemperature;
    public int minSpawnAltitude;
    public int maxSpawnAltitude;
    public int minSpawnMoisture;
    public int maxSpawnMoisture;

    public BiomeType(TileEnum.BiomeEnum type,float movDifficulty,  int minSpawnAltitude,int maxSpawnAltitude, int minSpawnTemperature, int maxSpawnTemperature,int minSpawnMoisture,int maxSpawnMoisture)
    {
        this.type = type;
        this.movDifficulty = movDifficulty;
        this.minSpawnTemperature = minSpawnTemperature;
        this.maxSpawnTemperature = maxSpawnTemperature;
        this.minSpawnAltitude = minSpawnAltitude;
        this.maxSpawnAltitude = maxSpawnAltitude;
        this.minSpawnMoisture=minSpawnMoisture;
        this.maxSpawnMoisture = maxSpawnMoisture;
    }

}
