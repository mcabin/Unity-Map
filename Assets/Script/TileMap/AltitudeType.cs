using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class AltitudeType
{
    public TileEnum.AltitudeEnum type { get; private set; }
    public int minAltitude { get; private set; }
    public int maxAltitude { get; private set; }

    public int level => (int)type;

    public bool isRising=false;

    public AltitudeType(TileEnum.AltitudeEnum type, int minAltitude, int maxAltitude)
    {
        this.type = type;
        this.minAltitude = minAltitude;
        this.maxAltitude = maxAltitude;
        neighborsElevs = new Dictionary<GlobalEnum.Direction, ElevationType>();
    }

    //Altitude
    static AltitudeType()
    {
        initialseAltitudeTypes();
    }
    private static Dictionary<TileEnum.AltitudeEnum, AltitudeType> altitudeTypes;

    private static void initialseAltitudeTypes()
    {
        altitudeTypes = new Dictionary<TileEnum.AltitudeEnum, AltitudeType>();
        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.Load(XMLAsset.getPathXml("Altitude.xml"));
        XmlNodeList altitudesXml = xmlDoc.SelectNodes("//Altitude");
        foreach (XmlNode altitudeXml in altitudesXml)
        {
            string typeStr = altitudeXml.SelectSingleNode("Type")?.InnerText;
            if (Enum.TryParse(typeStr, true, out TileEnum.AltitudeEnum altitudeEnum))
            {
                if (int.TryParse(altitudeXml.SelectSingleNode("MininimumAltitude")?.InnerText, out int minAltitude) &&
                    int.TryParse(altitudeXml.SelectSingleNode("MaximumAltitude")?.InnerText, out int maxAltitude))
                {
                    AltitudeType newAltitudeType = new AltitudeType(altitudeEnum, minAltitude, maxAltitude);
                    altitudeTypes[altitudeEnum] = newAltitudeType;
                }
                else
                {
                    throw new Exception($"Invalid numeric value in XML for biome {altitudeEnum}");
                }
            }
            else throw new Exception("Enum doesn't exist for " + typeStr);
        }
    }
    public bool isType(int altitude) {
        return altitude >= minAltitude && altitude <= maxAltitude;
    }
    public static AltitudeType get(TileEnum.AltitudeEnum key)
    {
        AltitudeType altitude = altitudeTypes[key];
        return new AltitudeType( altitude.type,altitude.minAltitude,altitude.maxAltitude );
    }
    public static List< AltitudeType> gets()
    {
        return altitudeTypes.Values.ToList();
    }

    //Elevation
    private Dictionary<GlobalEnum.Direction, ElevationType> neighborsElevs;
    public ElevationType elevationType { get; private set; }

    public void setNeighborElev(GlobalEnum.Direction direction, ElevationType elev)
    {
        neighborsElevs[direction] = elev;
    }

    public ElevationType getNeighborElev(GlobalEnum.Direction direction)
    {
        if(neighborsElevs.TryGetValue(direction, out ElevationType elev)) return elev;
        return null;
    }

    public int nbUnknow
    {
        get
        {
            int result = 0;
            foreach (GlobalEnum.Direction direction in Enum.GetValues(typeof(GlobalEnum.Direction)))
            {
                if (neighborsElevs.TryGetValue(direction, out var neighbor))
                {
                    if (neighbor.isTempory)
                    {
                        result++;
                    }
                }
            }
            return result;
        }
    }

    private bool isValid(ElevationType type)
    {
        bool result = true;
        foreach(GlobalEnum.Direction direction in Enum.GetValues(typeof(GlobalEnum.Direction)))
        {
            if(neighborsElevs.TryGetValue(direction, out var neighbors))
            {
                if(!neighbors.getEdge(GlobalEnum.inverseDirection(direction)).compare(type.getEdge(direction),type.altitude,neighbors.altitude))
                    result= false;
            }
        }
        return result;
    }
    public void setElevationType(ElevationType elev)
    {
        elevationType = elev;
    }

    
    //Renvoie true si les voisins de cette tuiles doivent etre modifié
    public bool setTypeWithNeighboor(int seed,int w,int h)
    {
        List<ElevationType> elevTypes = ElevationType.gets(type);
        List<ElevationType> possibleElevList = new List<ElevationType>();
        foreach (ElevationType baseType in elevTypes)
        {
            if (baseType.elev == TileEnum.ElevEnum.UNKNOW )
            {
                continue;
            }
            ElevationType typeNorth = baseType.clone(0);
            ElevationType typeEast = baseType.clone(90);
            ElevationType typeSouth = baseType.clone(180);
            ElevationType typeWest = baseType.clone(270);

            if (isValid(typeNorth))
            {
                possibleElevList.Add(typeNorth);
            }
            if (isValid(typeEast))
            {
                possibleElevList.Add(typeEast);
            }
            if (isValid(typeSouth))
            {
                possibleElevList.Add(typeSouth);
            }
            if (isValid(typeWest))
            {
                possibleElevList.Add(typeWest);
            }

        }
        
        if (possibleElevList.Count <= 0)
        {
            elevationType = ElevationType.getDefault(type);
            return true;
        }

        RandomListSelector randSelect = new RandomListSelector(seed);
        elevationType = randSelect.SelectWeighted(possibleElevList, obj => obj.rarity);
        return false;
    }


}
