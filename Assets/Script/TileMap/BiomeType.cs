using Assets.Script.TileMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using UnityEngine;
namespace Assets.Script
{
    [System.Serializable]
    public class BiomeType
    {


        public TileEnum.BiomeEnum type;
        public float movDifficulty;
        public int minSpawnTemperature { get; private set; }
        public int maxSpawnTemperature { get; private set; }
        public int minSpawnAltitude { get; private set; }
        public int maxSpawnAltitude { get; private set; }
        public int minSpawnMoisture { get; private set; }
        public int maxSpawnMoisture { get; private set; }

        private Dictionary<TileEnum.FeatureEnum, int> featuresSpawnProbabilities;
        private int maxFeatures;
        private float baseFeatureChance;


        public BiomeType(TileEnum.BiomeEnum type, float movDifficulty, int minSpawnAltitude, int maxSpawnAltitude, int minSpawnTemperature, int maxSpawnTemperature, int minSpawnMoisture, int maxSpawnMoisture)
        {
            this.type = type;
            this.movDifficulty = movDifficulty;
            this.minSpawnTemperature = minSpawnTemperature;
            this.maxSpawnTemperature = maxSpawnTemperature;
            this.minSpawnAltitude = minSpawnAltitude;
            this.maxSpawnAltitude = maxSpawnAltitude;
            this.minSpawnMoisture = minSpawnMoisture;
            this.maxSpawnMoisture = maxSpawnMoisture;
        }

        private static Dictionary<TileEnum.BiomeEnum, BiomeType> biomeTypes;

        private static void initialiseBiomeTypes()
        {
            XmlDocument xmlDoc = new XmlDocument();
            biomeTypes = new Dictionary<TileEnum.BiomeEnum, BiomeType>();
            xmlDoc.Load(XMLAsset.getPathXml("Biome.xml"));
            XmlNodeList biomesXML = xmlDoc.SelectNodes("//Biome");
            foreach (XmlNode biomeXML in biomesXML)
            {
                string typeStr = biomeXML.SelectSingleNode("Type")?.InnerText;
                if (Enum.TryParse<TileEnum.BiomeEnum>(typeStr, true, out TileEnum.BiomeEnum biomeEnum))
                {
                    XmlNodeList featuresXML=biomeXML.SelectSingleNode("./PossibleFeatures").SelectNodes("./PossibleFeature");
                    Dictionary<TileEnum.FeatureEnum, int> newFeaturesSpawnProbabilities=new Dictionary<TileEnum.FeatureEnum, int>();
                    Debug.Log(featuresXML.Count);
                    foreach (XmlNode featureXML in featuresXML)
                    {
                        if (Enum.TryParse<TileEnum.FeatureEnum>(featureXML.SelectSingleNode("Feature")?.InnerText, true, out TileEnum.FeatureEnum featureEnum)&&
                            int.TryParse(featureXML.SelectSingleNode("Rarity")?.InnerText,out int rarity))
                        {
                            newFeaturesSpawnProbabilities.Add(featureEnum, rarity);
                        }
                        else
                         throw new Exception("Rarity or feature incorrect");
                    }
                    if (int.TryParse(biomeXML.SelectSingleNode("MinSpawnTemperature")?.InnerText, out int minSpawnTemp) &&
                        int.TryParse(biomeXML.SelectSingleNode("MaxSpawnTemperature")?.InnerText, out int maxSpawnTemp) &&
                        int.TryParse(biomeXML.SelectSingleNode("MinSpawnAltitude")?.InnerText, out int minSpawnAltitude) &&
                        int.TryParse(biomeXML.SelectSingleNode("MaxSpawnAltitude")?.InnerText, out int maxSpawnAltitude) &&
                        int.TryParse(biomeXML.SelectSingleNode("MinSpawnMoisture")?.InnerText, out int minSpawnMoisture) &&
                        int.TryParse(biomeXML.SelectSingleNode("MaxSpawnMoisture")?.InnerText, out int maxSpawnMoisture) &&
                        float.TryParse(biomeXML.SelectSingleNode("MovementDifficulty")?.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out float movementDifficulty))
                    {
                        BiomeType newBiomeType = new BiomeType(biomeEnum, movementDifficulty, minSpawnAltitude, maxSpawnAltitude, minSpawnTemp, maxSpawnTemp, minSpawnMoisture, maxSpawnMoisture);
                        biomeTypes[biomeEnum] = newBiomeType;
                    }
                    else
                    {
                        throw new Exception($"Invalid numeric value in XML for biome {biomeEnum}");
                    }
                }
                else throw new Exception("Enum doesn't exist for " + typeStr);
            }
        }

        static BiomeType()
        {
            initialiseBiomeTypes();
        }

        public static BiomeType get(TileEnum.BiomeEnum key)
        {
            return biomeTypes[key];
        }

        public List<TileFeatureType> getFeatures(int seed)
        {
            double proba = baseFeatureChance;
            int featureRemaining = maxFeatures;
            System.Random random = new System.Random(seed);
            RandomListSelector randomizer = new RandomListSelector(seed);
            List<TileFeatureType> featuresList=new List<TileFeatureType>();
            while (random.NextDouble()<proba && maxFeatures>0)
            {
                maxFeatures--;
                TileEnum.FeatureEnum featureEnum=randomizer.SelectWeighted(featuresSpawnProbabilities, item => item.Value).Key;
                featuresList.Add(TileFeatureType.getType(featureEnum));
            }
            return featuresList;
        }
 
    }
}