using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


namespace Assets.Script {
    [System.Serializable]
    public class Tile
    {
        public BiomeType biome;
        public List<TileFeature> features;
        public int coordW, coordH;
        public AltitudeType altitude;
        public Tile()
        {

        }
        public Tile(BiomeType biome, int coordW, int coordH, AltitudeType altitude)
        {

            //Biome
            this.biome = biome;

            this.coordW = coordW;
            this.coordH = coordH;

            this.altitude = altitude;
        }

        public float calculateMovementCost(Unit unit)
        {
            float biomeCost = biome.movDifficulty * unit.movementCoastByBiome[biome.type];
            return biomeCost;
        }
    }
}
