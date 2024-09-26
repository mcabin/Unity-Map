using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;

public struct EdgeStruct
{
    public bool isUp;
    public bool isRising;
    public bool isPraticable;
    public EdgeStruct(bool isUp,bool isRising,bool isPraticable)
    {
        this.isUp = isUp;
        this.isRising = isRising;
        this.isPraticable = isPraticable;
    }

    public static bool operator ==(EdgeStruct a, EdgeStruct b)
    {
        
        return a.isUp == b.isUp && a.isRising== b.isRising;
    }
    public static bool operator !=(EdgeStruct a, EdgeStruct b)
    {

        return a.isUp != b.isUp || a.isRising != b.isRising;
    }
    public bool compare(EdgeStruct other,TileEnum.AltitudeEnum altitudeA, TileEnum.AltitudeEnum altitudeB) {
        if (altitudeA == altitudeB)
        {
            return (other.isUp == this.isUp && isUp) || (isRising && other.isRising == this.isRising);
        }
        else return !other.isUp && !other.isRising && this.isUp && (int)altitudeA > (int)altitudeB;
    }

    public override bool Equals(object obj)
    {
        return obj is EdgeStruct @struct &&
               isUp == @struct.isUp &&
               isRising == @struct.isRising;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(isUp, isRising);
    }
}
public class ElevationType
{
    public bool isTempory => elev == TileEnum.ElevEnum.UNKNOW;
    public TileEnum.ElevEnum elev { get; private set; }
    public int rarity { get; private set; }
    public bool isDefault { get; private set; }

    private int _rotation;

 
    public int rotation
    {
        get { return _rotation; }
        set { _rotation = value % 360; }
    }
    private Dictionary<GlobalEnum.Direction,EdgeStruct> edgesDic;
    public TileEnum.AltitudeEnum altitude{get;private set;} 

    public ElevationType(TileEnum.ElevEnum elev,EdgeStruct northEdge, EdgeStruct eastEdge, EdgeStruct southEdge, EdgeStruct westEdge, int rotation,int rarity,TileEnum.AltitudeEnum altitude,bool isDefault)
    {
        edgesDic = new Dictionary<GlobalEnum.Direction, EdgeStruct>();
        this.elev = elev;
        edgesDic[GlobalEnum.Direction.NORTH] = northEdge;
        edgesDic[GlobalEnum.Direction.EAST] = eastEdge;
        edgesDic[GlobalEnum.Direction.SOUTH] = southEdge;
        edgesDic[GlobalEnum.Direction.WEST] = westEdge;
        this.rotation = rotation;
        this.rarity = rarity;
        this.altitude = altitude;
        this.isDefault = isDefault;
    }

    public ElevationType clone(int rotation = 0)
    {
        if(rotation == 0)
        {
            return new ElevationType(elev, edgesDic[GlobalEnum.Direction.NORTH], edgesDic[GlobalEnum.Direction.EAST], edgesDic[GlobalEnum.Direction.SOUTH], edgesDic[GlobalEnum.Direction.WEST], rotation,rarity,altitude, isDefault);
        }
        if (rotation == 90)
        {
            return new ElevationType(elev, edgesDic[GlobalEnum.Direction.WEST], edgesDic[GlobalEnum.Direction.NORTH], edgesDic[GlobalEnum.Direction.EAST], edgesDic[GlobalEnum.Direction.SOUTH], rotation, rarity, altitude, isDefault);
        }
        if (rotation == 180)
        {
            return new ElevationType(elev, edgesDic[GlobalEnum.Direction.SOUTH], edgesDic[GlobalEnum.Direction.WEST], edgesDic[GlobalEnum.Direction.NORTH], edgesDic[GlobalEnum.Direction.EAST], rotation, rarity, altitude, isDefault);

        }
        if (rotation == 270)
        {
            return new ElevationType(elev, edgesDic[GlobalEnum.Direction.EAST], edgesDic[GlobalEnum.Direction.SOUTH], edgesDic[GlobalEnum.Direction.WEST], edgesDic[GlobalEnum.Direction.NORTH], rotation, rarity, altitude, isDefault);
        }
        throw new Exception("Invalid rotation");
    }

    public override string ToString()
    {
        string str=elev.ToString()+" "+rotation;

        return str;
    }
    private static Dictionary<TileEnum.ElevEnum, ElevationType> elevTypes;
    private static void intitialise()
    {
        elevTypes = new Dictionary<TileEnum.ElevEnum, ElevationType>();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(XMLAsset.getPathXml("Elevation.xml"));
        XmlNodeList elevationsXml = xmlDoc.SelectNodes("//Elevation");
        foreach (XmlNode elevationXml in elevationsXml)
        {
            string typeStr = elevationXml.SelectSingleNode("Type")?.InnerText;
            if (Enum.TryParse(typeStr, true, out TileEnum.ElevEnum elevEnum))
            {
                if (int.TryParse(elevationXml.SelectSingleNode("Rarity")?.InnerText, out int rarity) && 
                    int.TryParse(elevationXml.SelectSingleNode("isDefault")?.InnerText, out int isDefault))
                {

                    //North
                    EdgeStruct northEdge;
                    if (int.TryParse(elevationXml.SelectSingleNode("NorthEdge").SelectSingleNode("isRising")?.InnerText, out int northIsElev) &&
                        int.TryParse(elevationXml.SelectSingleNode("NorthEdge").SelectSingleNode("isUp")?.InnerText, out int northIsPlateau)
                        && int.TryParse(elevationXml.SelectSingleNode("NorthEdge").SelectSingleNode("isPraticable")?.InnerText, out int northIsPraticable)
                        )
                    {
                        northEdge = new EdgeStruct(northIsPlateau > 0, northIsElev > 0,northIsPraticable>0);
                    }
                    else
                        throw new Exception("Invalid north for " + elevEnum);

                    //East
                    EdgeStruct eastEdge;
                    if (int.TryParse(elevationXml.SelectSingleNode("EastEdge").SelectSingleNode("isRising")?.InnerText, out int eastIsElev) &&
                        int.TryParse(elevationXml.SelectSingleNode("EastEdge").SelectSingleNode("isUp")?.InnerText, out int eastIsPlateau)&&
                        int.TryParse(elevationXml.SelectSingleNode("EastEdge").SelectSingleNode("isPraticable")?.InnerText, out int eastIsPraticable)

                        )
                    {
                        eastEdge = new EdgeStruct(eastIsPlateau > 0, eastIsElev > 0,eastIsPraticable>0);
                    }
                    else
                        throw new Exception("Invalid east for " + elevEnum);

                    //south
                    EdgeStruct southEdge;
                    if (int.TryParse(elevationXml.SelectSingleNode("SouthEdge").SelectSingleNode("isRising")?.InnerText, out int southIsElev) &&
                        int.TryParse(elevationXml.SelectSingleNode("SouthEdge").SelectSingleNode("isUp")?.InnerText, out int southIsPlateau)&&
                        int.TryParse(elevationXml.SelectSingleNode("SouthEdge").SelectSingleNode("isPraticable")?.InnerText, out int southIsPraticalbe)
                        )
                    {
                        southEdge = new EdgeStruct(southIsPlateau > 0, southIsElev > 0,southIsPraticalbe>0);
                    }
                    else
                        throw new Exception("Invalid south for " + elevEnum);

                    //west
                    EdgeStruct westEdge;
                    if (int.TryParse(elevationXml.SelectSingleNode("WestEdge").SelectSingleNode("isRising")?.InnerText, out int westIsElev) &&
                        int.TryParse(elevationXml.SelectSingleNode("WestEdge").SelectSingleNode("isUp")?.InnerText, out int westIsPlateau)&&
                        int.TryParse(elevationXml.SelectSingleNode("WestEdge").SelectSingleNode("isPraticable")?.InnerText, out int westIsPraticable)

                        )
                    {
                        westEdge = new EdgeStruct(westIsPlateau > 0, westIsElev > 0,westIsPraticable>0);
                    }
                    else
                        throw new Exception("Invalid west for " + elevEnum);
                    string altitudeStr= elevationXml.SelectSingleNode("Altitude")?.InnerText;
                    if (Enum.TryParse(altitudeStr, true, out TileEnum.AltitudeEnum altitudeEnum)){
                        elevTypes[elevEnum] = new ElevationType(elevEnum, northEdge, eastEdge, southEdge, westEdge, 0, rarity, altitudeEnum,isDefault==1);
                    }
                    else throw new Exception("Incorrect altitude "+ altitudeStr);
                }
                else throw new Exception("incorrect rarity or default for " + typeStr);

            }
        }
    }

    static ElevationType()
    {
        intitialise();
    }

    public EdgeStruct getEdge(GlobalEnum.Direction direction)
    {
        if(edgesDic.TryGetValue(direction, out EdgeStruct edge)) 
            return edge;
        return new EdgeStruct(true,true);
    }

    public static ElevationType get(TileEnum.ElevEnum key) {
        return elevTypes[key];
    }
    public static List<ElevationType> gets(TileEnum.AltitudeEnum neededAltitude)
    {
        return elevTypes.Values.Where(e=>e.altitude==neededAltitude).ToList();
    }

    public static ElevationType getDefault(TileEnum.AltitudeEnum neededAltitude)
    {
        return elevTypes.Values.First(e => e.altitude== neededAltitude &&e.isDefault);
    }

    public static ElevationType getUnknow(TileEnum.AltitudeEnum altitude)
    {
        EdgeStruct edge=new EdgeStruct(true, true);
        return new ElevationType(TileEnum.ElevEnum.UNKNOW, edge, edge, edge, edge, 0, 1,altitude, false);
    }
}
