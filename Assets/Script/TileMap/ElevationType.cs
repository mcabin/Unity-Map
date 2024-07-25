using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public struct ElevationStruct
{
    public int rotation;
    public TileEnum.ElevEnum enumElev;
}
public class ElevationType
{
    ElevationStruct elev;
    List<ElevationStruct> neighboorNorth;
    List<ElevationStruct> neighboorEast;
    List<ElevationStruct> neighboorSouth;
    List<ElevationStruct> neighboorWest;
    bool isBorder;

    public void rotate(int rotation)
    {
        
    }
}
