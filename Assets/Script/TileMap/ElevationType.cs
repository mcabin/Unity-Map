using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;

public struct EdgeStruct
{
    public bool isPlateau;
    public bool isElevation;
    public EdgeStruct(bool isPlateau,bool isElevation)
    {
        this.isPlateau = isPlateau;
        this.isElevation = isElevation;
    }

    public static bool operator ==(EdgeStruct a, EdgeStruct b)
    {
        
        return a.isPlateau == b.isPlateau && a.isElevation== b.isElevation;
    }
    public static bool operator !=(EdgeStruct a, EdgeStruct b)
    {

        return a.isPlateau != b.isPlateau || a.isElevation != b.isElevation;
    }
    public bool compare(EdgeStruct other) {
             return (other.isPlateau == this.isPlateau && isPlateau) || (isElevation && other.isElevation == this.isElevation) ||other==this;
    }

    public override bool Equals(object obj)
    {
        return obj is EdgeStruct @struct &&
               isPlateau == @struct.isPlateau &&
               isElevation == @struct.isElevation;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(isPlateau, isElevation);
    }
}
public class ElevationType
{
    public bool isTempory => elev == TileEnum.ElevEnum.UNKNOW;
    public TileEnum.ElevEnum elev { get; private set; }
    public int rarity { get; private set; }
    private int _rotation;
    public int rotation
    {
        get { return _rotation; }
        set { _rotation = value % 360; }
    }
    public EdgeStruct northEdge { get; private set; }
    public EdgeStruct eastEdge { get; private set; }
    public EdgeStruct southEdge { get; private set; }
    public EdgeStruct westEdge { get; private set; }

    public ElevationType(TileEnum.ElevEnum elev,EdgeStruct northEdge, EdgeStruct eastEdge, EdgeStruct southEdge, EdgeStruct westEdge, int rotation,int rarity)
    {
        this.elev = elev;
        this.northEdge = northEdge;
        this.eastEdge = eastEdge;
        this.southEdge = southEdge;
        this.westEdge = westEdge;
        this.rotation = rotation;
        this.rarity = rarity;
    }

    public ElevationType clone(int rotation = 0)
    {
        if(rotation == 0)
        {
            return new ElevationType(elev, northEdge, eastEdge, southEdge, westEdge,rotation,rarity);
        }
        if (rotation == 90)
        {
            return new ElevationType(elev, westEdge, northEdge, eastEdge, southEdge, rotation, rarity);
        }
        if (rotation == 180)
        {
            return new ElevationType(elev, southEdge, westEdge, northEdge, eastEdge , rotation, rarity);
        }
        if (rotation == 270)
        {
            return new ElevationType(elev,eastEdge , southEdge , westEdge , northEdge , rotation, rarity);
        }
        throw new Exception("Invalid rotation");
    }

    public override string ToString()
    {
        string str=elev.ToString()+" "+rotation;

        return str;
    }



}
