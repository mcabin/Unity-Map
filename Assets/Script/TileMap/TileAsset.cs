using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static TileEnum;

public static class TileAsset
{
    private readonly static BiomeType[] biomeTypes;
    private readonly static int biomeNumber;

    private readonly static AltitudeType[] altitudeTypes;
    private readonly static int altitudeNumber;

    private readonly static TileFeature[] featureTypes;
    private readonly static int featureNumber;

    private readonly static ElevationType[] elevationTypes;
    private readonly static int elevationNumber;
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

        //
        elevationNumber = Enum.GetNames(typeof(TileEnum.ElevEnum)).Length;
        elevationTypes = new ElevationType[elevationNumber];
        intitialiseElevationType();
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
            if (Enum.TryParse(typeStr, true, out TileEnum.AltitudeEnum altitudeEnum))
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
    //ELEVATION
    private static void intitialiseElevationType()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(getPathXml("Elevation.xml"));
        XmlNodeList elevationsXml = xmlDoc.SelectNodes("//Elevation");
        foreach (XmlNode elevationXml in elevationsXml)
        {
            string typeStr = elevationXml.SelectSingleNode("Type")?.InnerText;
            if (Enum.TryParse(typeStr, true, out TileEnum.ElevEnum elevEnum))
            {
                if (int.TryParse(elevationXml.SelectSingleNode("Rotation")?.InnerText, out int rotation) &&
                    int.TryParse(elevationXml.SelectSingleNode("Rarity")?.InnerText, out int rarity))
                {

                    //North
                    EdgeStruct northEdge;
                    if (int.TryParse(elevationXml.SelectSingleNode("NorthEdge").SelectSingleNode("isElev")?.InnerText, out int northIsElev) &&
                        int.TryParse(elevationXml.SelectSingleNode("NorthEdge").SelectSingleNode("isPlateau")?.InnerText, out int northIsPlateau)
                        )
                    {
                        northEdge= new EdgeStruct(northIsPlateau>0,northIsElev>0);
                    }
                    else
                        throw new Exception("Invalid north for "+elevEnum);

                    //East
                    EdgeStruct eastEdge;
                    if (int.TryParse(elevationXml.SelectSingleNode("EastEdge").SelectSingleNode("isElev")?.InnerText, out int eastIsElev) &&
                        int.TryParse(elevationXml.SelectSingleNode("EastEdge").SelectSingleNode("isPlateau")?.InnerText, out int eastIsPlateau)
                        )
                    {
                        eastEdge = new EdgeStruct(eastIsPlateau > 0, eastIsElev > 0);
                    }
                    else
                        throw new Exception("Invalid east for " + elevEnum);

                    //south
                    EdgeStruct southEdge;
                    if (int.TryParse(elevationXml.SelectSingleNode("SouthEdge").SelectSingleNode("isElev")?.InnerText, out int southIsElev) &&
                        int.TryParse(elevationXml.SelectSingleNode("SouthEdge").SelectSingleNode("isPlateau")?.InnerText, out int southIsPlateau)
                        )
                    {
                        southEdge = new EdgeStruct(southIsPlateau > 0, southIsElev > 0);
                    }
                    else
                        throw new Exception("Invalid south for " + elevEnum);

                    //west
                    EdgeStruct westEdge;
                    if (int.TryParse(elevationXml.SelectSingleNode("WestEdge").SelectSingleNode("isElev")?.InnerText, out int westIsElev) &&
                        int.TryParse(elevationXml.SelectSingleNode("WestEdge").SelectSingleNode("isPlateau")?.InnerText, out int westIsPlateau)
                        )
                    {
                        westEdge = new EdgeStruct(westIsPlateau > 0, westIsElev > 0);
                    }
                    else
                        throw new Exception("Invalid west for " + elevEnum);
                    elevationTypes[(int)elevEnum] = new ElevationType(elevEnum, northEdge, eastEdge, southEdge, westEdge, 0, rarity);
                }
                else throw new Exception("incorrect rotation");

            }
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

    public static ElevationType[] getElevationTypes()
    {
        return elevationTypes;
    }

    public static ElevationType getElevationType(TileEnum.ElevEnum key)
    {
        return elevationTypes[(int)key];
    }
}