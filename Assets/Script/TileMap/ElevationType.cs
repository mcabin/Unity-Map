using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;

public struct ElevationStruct:IEquatable<ElevationStruct>
{
    private int _rotation;
    public int rotation
    {
        get { return _rotation; }
        set {
            _rotation = value % 360;
            }
    }
    public TileEnum.ElevEnum enumElev;
    public ElevationStruct( TileEnum.ElevEnum enumElev,int rotation)
    {
        _rotation=rotation%360;
        this.enumElev = enumElev;
    }

    public bool Equals(ElevationStruct other)
    {
        return this.rotation==other.rotation && this.enumElev==other.enumElev;
    }
    public override string ToString()
    {
        return "Type: "+enumElev.ToString()+" Rotation :"+rotation+" ";
    }
}
public class ElevationType
{
    public ElevationStruct elev { get; private set; }
    public List<ElevationStruct> neighboorNorth { get; private set; }
    public List<ElevationStruct> neighboorEast { get; private set; }
    public List<ElevationStruct> neighboorSouth { get; private set; }
    public List<ElevationStruct> neighboorWest { get; private set; }

    public ElevationType(ElevationStruct elev, List<ElevationStruct> neighboorNorth, List<ElevationStruct> neighboorEast,
                        List<ElevationStruct> neighboorSouth, List<ElevationStruct> neighboorWest)
    {
        this.elev = elev;
        this.neighboorNorth = neighboorNorth;
        this.neighboorEast = neighboorEast;
        this.neighboorSouth = neighboorSouth;
        this.neighboorWest = neighboorWest;
    }

    public ElevationType clone(int rotation = 0)
    {

        List<ElevationStruct> newNorth=new List<ElevationStruct>();
        foreach(ElevationStruct e in neighboorNorth)
        {
            int newRotation=e.rotation+ rotation;
            newNorth.Add(new ElevationStruct(e.enumElev,newRotation));
        }
        List<ElevationStruct> newEast = new List<ElevationStruct>(neighboorEast);
        foreach (ElevationStruct e in neighboorEast)
        {
            int newRotation = e.rotation + rotation;
            newEast.Add(new ElevationStruct(e.enumElev, newRotation));
        }
        List<ElevationStruct> newSouth = new List<ElevationStruct>(neighboorSouth);
        foreach (ElevationStruct e in neighboorSouth)
        {
            int newRotation = e.rotation + rotation;
            newSouth.Add(new ElevationStruct(e.enumElev, newRotation));
        }
        List<ElevationStruct> newWest=new List<ElevationStruct>(neighboorWest);
        foreach (ElevationStruct e in neighboorWest)
        {
            int newRotation = e.rotation + rotation;
            newWest.Add(new ElevationStruct(e.enumElev, newRotation));
        }
        return new ElevationType(new ElevationStruct(elev.enumElev, rotation+elev.rotation), newNorth, newEast, newSouth, newWest);
    }

    public override string ToString()
    {
        string str=elev.ToString();
        /*str += "North:\n";
        foreach (ElevationStruct e in neighboorNorth)
        {
            str += e.ToString() + "\n";
        }
        str += "east:\n";
        foreach (ElevationStruct e in neighboorEast)
        {
            str += e.ToString() + "\n";

        }
        str += "south:\n";

        foreach (ElevationStruct e in neighboorSouth)
        {
            str += e.ToString() + "\n";

        }
        str += "west:\n";

        foreach (ElevationStruct e in neighboorWest)
        {
            str += e.ToString() + "\n";

        }*/

        return str;
    }
}
