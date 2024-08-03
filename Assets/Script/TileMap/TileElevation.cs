using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEditor.Search;
using UnityEngine;

public class TileElevation : Tile
{
    public ElevationType elevationType { get; private set; }
    public ElevationType northNeighboor;
    public ElevationType southNeighboor;
    public ElevationType westNeighboor;
    public ElevationType eastNeighboor;

    public TileElevation(BiomeType biome, int coordX, int coordY, AltitudeType altitude) : base(biome, coordX, coordY, altitude)
    {
        this.elevationType = null;
        elevationType = null;
        northNeighboor = null;
        southNeighboor = null;
        westNeighboor = null;
        eastNeighboor = null;
    }
    public void setElevationType(TileEnum.ElevEnum elevEnum,int rotation=0) 
    {
        Debug.Log(rotation);
        elevationType=TileAsset.getElevationType(elevEnum).clone(rotation);
    }

    public  void setTypeWithNeighboor()
    {
        List<ElevationStruct> baseList;
        if (northNeighboor != null)
        {
             baseList= northNeighboor.neighboorSouth;

            if(southNeighboor != null)
            {
                baseList = baseList.Intersect(southNeighboor.neighboorNorth).ToList();
            }
            if(westNeighboor != null)
            {
                baseList = baseList.Intersect(westNeighboor.neighboorEast).ToList();

            }
            else if (eastNeighboor != null)
            {
                baseList = baseList.Intersect(eastNeighboor.neighboorWest).ToList();
            }
        }
        else if(southNeighboor != null)
        {
            baseList = southNeighboor.neighboorNorth;
            if (westNeighboor != null)
            {
                baseList = baseList.Intersect(westNeighboor.neighboorEast).ToList();

            }
            else if (eastNeighboor != null)
            {
                baseList = baseList.Intersect(eastNeighboor.neighboorWest).ToList();
            }
        }
        else if(westNeighboor != null)
        {
            baseList = westNeighboor.neighboorEast;
             if (eastNeighboor != null)
            {
                baseList = baseList.Intersect(eastNeighboor.neighboorWest).ToList();
            }
        }

        else if (eastNeighboor != null)
        {
            baseList = eastNeighboor.neighboorWest;
        }
        else
        {
            throw new Exception("All neighboor are empty for "+coordX+" "+coordY+" "+altitude.type);
        }
        if(baseList.Count <= 0)
        {
            Debug.Log("Type " + elevationType + "\n\n North " + northNeighboor + "\n\n south " + southNeighboor + "\n\n west " + westNeighboor + "\n\n east " + eastNeighboor);
            throw new Exception("No possible elevation for tile "+coordX+" "+coordY);
        }
        System.Random random = new System.Random();
        int randomIndex = random.Next(0, baseList.Count-1);
        ElevationStruct chosenElev = baseList[randomIndex];
        setElevationType(chosenElev.enumElev, chosenElev.rotation);
        Debug.Log("Type " + elevationType + "\n\n North " + northNeighboor + "\n\n south " + southNeighboor + "\n\n west " + westNeighboor + "\n\n east " + eastNeighboor);

    }

    private Exception Exception()
    {
        throw new NotImplementedException();
    }
}
