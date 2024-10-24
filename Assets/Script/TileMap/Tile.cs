using Assets.Script.TileMap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


namespace Assets.Script {
    [System.Serializable]
    public class Tile
    {
        public BiomeType biome { get; private set; }
        public List<TileFeatureType> features;
        public int coordW, coordH;
        public AltitudeType altitude { get; private set; }
        public Tile()
        {

        }
        public Tile(BiomeType biome, int coordW, int coordH, AltitudeType altitude,List<TileFeatureType> featuresList)
        {

            //Biome
            this.biome = biome;

            this.coordW = coordW;
            this.coordH = coordH;

            this.altitude = altitude;
            this.features = featuresList;
        }

        public float calculateMovementCost(Unit unit)
        {
            float biomeCost = biome.movDifficulty * unit.movementCoastByBiome[biome.type];
            return biomeCost;
        }
    }
}
