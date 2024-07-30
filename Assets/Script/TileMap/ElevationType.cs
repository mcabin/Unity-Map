using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public struct ElevationStruct
{
    public int rotation;
    public TileEnum.ElevEnum enumElev;
    public ElevationStruct( TileEnum.ElevEnum enumElev,int rotation)
    {
        this.rotation = rotation;
        this.enumElev = enumElev;
    }
}
public class ElevationType
{
    ElevationStruct elev;
    List<ElevationStruct> neighboorNorth;
    List<ElevationStruct> neighboorEast;
    List<ElevationStruct> neighboorSouth;
    List<ElevationStruct> neighboorWest;

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
        return new ElevationType(new ElevationStruct(elev.enumElev, elev.rotation), newNorth, newEast, newSouth, newWest);
    }
}
