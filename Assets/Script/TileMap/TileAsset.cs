using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using UnityEngine;

public static class TileAsset
{
    private readonly static BiomeType[] biomeTypes;
    private readonly static int biomeNumber;

    private readonly static AltitudeType[] altitudeTypes;
    private readonly static int altitudeNumber;

    private readonly static TileFeature[] featureTypes;
    private readonly static int featureNumber;

    static TileAsset()
    {
        //ALTITUDE
        altitudeNumber=Enum.GetNames(typeof(TileEnum.AltitudeEnum)).Length;
        altitudeTypes=new AltitudeType[altitudeNumber];
        initialseAltitudeTypes();
        //BIOME
        biomeNumber = Enum.GetNames(typeof(TileEnum.BiomeEnum)).Length;
        biomeTypes = new BiomeType[biomeNumber];
        initialseBiomeTypes();
        //FEATURE
        featureNumber = 2;
        featureTypes = new TileFeature[featureNumber];
        initialiseFeatureTypes();
    }
    private static string getPathXml(string pathName)
    {
        string cheminRelatif = Path.Combine("Script/TileMap/Xml", pathName);

        #if UNITY_EDITOR
        // Chemin pour l'éditeur Unity
        return Path.Combine(Application.dataPath, cheminRelatif);
        #else
            return Path.Combine(Application.persistentDataPath, cheminRelatif);
        #endif
    }
    private static void initialseBiomeTypes()
    {
        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.Load(getPathXml("Biome.xml"));
        XmlNodeList biomesXML = xmlDoc.SelectNodes("//Biome");
        foreach (XmlNode biomeXML in biomesXML)
        {
            string typeStr = biomeXML.SelectSingleNode("Type")?.InnerText;
            if (Enum.TryParse<TileEnum.BiomeEnum>(typeStr, true, out TileEnum.BiomeEnum biomeEnum))
            {
                if (int.TryParse(biomeXML.SelectSingleNode("MinSpawnTemperature")?.InnerText, out int minSpawnTemp) &&
                    int.TryParse(biomeXML.SelectSingleNode("MaxSpawnTemperature")?.InnerText, out int maxSpawnTemp) &&
                    int.TryParse(biomeXML.SelectSingleNode("MinSpawnAltitude")?.InnerText, out int minSpawnAltitude) &&
                    int.TryParse(biomeXML.SelectSingleNode("MaxSpawnAltitude")?.InnerText, out int maxSpawnAltitude) &&
                    int.TryParse(biomeXML.SelectSingleNode("MinSpawnMoisture")?.InnerText, out int minSpawnMoisture) &&
                    int.TryParse(biomeXML.SelectSingleNode("MaxSpawnMoisture")?.InnerText, out int maxSpawnMoisture) &&
                    float.TryParse(biomeXML.SelectSingleNode("MovementDifficulty")?.InnerText,NumberStyles.Float, CultureInfo.InvariantCulture, out float movementDifficulty))
                {
                    BiomeType newBiomeType = new BiomeType(biomeEnum, movementDifficulty, minSpawnAltitude, maxSpawnAltitude,minSpawnTemp,maxSpawnTemp,minSpawnMoisture,maxSpawnMoisture);
                    biomeTypes[(int)biomeEnum] = newBiomeType;
                }
                else
                {
                    throw new Exception($"Invalid numeric value in XML for biome {biomeEnum}");
                }
            }
            else throw new Exception("Enum doesn't exist for " + typeStr);
        }
    }

    private static void initialseAltitudeTypes()
    {
        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.Load(getPathXml("Altitude.xml"));
        XmlNodeList altitudesXml = xmlDoc.SelectNodes("//Altitude");
        foreach (XmlNode altitudeXml in altitudesXml)
        {
            string typeStr = altitudeXml.SelectSingleNode("Type")?.InnerText;
            if (Enum.TryParse<TileEnum.AltitudeEnum>(typeStr, true, out TileEnum.AltitudeEnum altitudeEnum))
            {
                if (int.TryParse(altitudeXml.SelectSingleNode("MininimumAltitude")?.InnerText, out int minAltitude) &&
                    int.TryParse(altitudeXml.SelectSingleNode("MaximumAltitude")?.InnerText, out int maxAltitude))
                {
                    AltitudeType newAltitudeType = new AltitudeType(altitudeEnum, minAltitude,maxAltitude);
                    altitudeTypes[(int)altitudeEnum] = newAltitudeType;
                }
                else
                {
                    throw new Exception($"Invalid numeric value in XML for biome {altitudeEnum}");
                }
            }
            else throw new Exception("Enum doesn't exist for " + typeStr);
        }
    }

    //FEATURE
    private static void initialiseFeatureTypes()
    {
 
        //FOREST
        featureTypes[(int)TileEnum.FeatureEnum.FOREST] = new TileFeature(TileEnum.FeatureEnum.FOREST, x => x * 1.5f) ;
        //MOUNTAIN
        featureTypes[(int)TileEnum.FeatureEnum.MOUNTAIN] = new TileFeature(TileEnum.FeatureEnum.MOUNTAIN, x => x * 3);
    }
    //GETTER
    public static TileFeature getFeatureType(TileEnum.FeatureEnum key)
    {
        return featureTypes[(int)key];
    }

    public static AltitudeType GetAltitudeType(TileEnum.AltitudeEnum key)
    {
        return altitudeTypes[(int)key];
    }
    //GETER
    public static BiomeType getBiomeType(TileEnum.BiomeEnum key)
    {
        return biomeTypes[(int)key];
    }
}