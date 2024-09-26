using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
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

    public static BiomeType get(TileEnum.BiomeEnum key) {
        return biomeTypes[key];
    }

}
