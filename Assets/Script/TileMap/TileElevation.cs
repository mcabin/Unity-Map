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
        elevationType=TileAsset.getElevationType(elevEnum).clone(rotation);
    }

    private bool isValid(ElevationType type)
    {
        return westNeighboor.eastEdge.compare( type.westEdge) 
            && eastNeighboor.westEdge.compare(type.eastEdge)
            && southNeighboor.northEdge.compare(type.southEdge)
            && northNeighboor.southEdge.compare(type.northEdge);
    }
    public  void setTypeWithNeighboor()
    {
        ElevationType[] elevTypes= TileAsset.getElevationTypes();
        List<ElevationType> possibleElevList= new List<ElevationType>();
        for(int i = 0; i < elevTypes.Length; i++)
        {
            ElevationType baseType = elevTypes[i];
            if (baseType.elev == TileEnum.ElevEnum.PLAIN || baseType.elev == TileEnum.ElevEnum.PLATEAU || baseType.elev == TileEnum.ElevEnum.UNKNOW)
                continue;
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
        System.Random random = new System.Random();
        if(possibleElevList.Count <= 0)
        {
            Debug.Log(" List count " + possibleElevList.Count + " N:" + northNeighboor + " E:" + eastNeighboor + " S:" + southNeighboor + " O:" + westNeighboor);
            throw new Exception("List empty");
        }
        if(possibleElevList.Count > 1)
        {
            possibleElevList.RemoveAll(obj => obj.elev == TileEnum.ElevEnum.ELEV_PPP);

            ElevationType epeType=possibleElevList.FirstOrDefault(obj => obj.elev == TileEnum.ElevEnum.ELEV_EPE);
        }
        int chosen=random.Next(0, possibleElevList.Count);
        Debug.Log(coordX+" "+ coordY+" Type: " + possibleElevList[chosen]+" "+chosen+" Count "+possibleElevList.Count );

        elevationType = possibleElevList[chosen];
    }
}
