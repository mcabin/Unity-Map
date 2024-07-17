using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TileMapViewer : MonoBehaviour
{
    public int mapWidth = 100;
    public int mapHeight = 100;
    public GameObject[] featureObjects;
    public GameObject[] plainTilesObject;
    public GameObject[] plateauTileObjects;
    public GameObject waterTile;
    public GameObject[] elevationObject;
    public Material[] elevationMaterial;
    public int tileSize=2;

    void Start()
    {
        
    }

    public void spawnElevation(int biomeId,int elevId,GameObject parent)
    {
        GameObject newElev = Instantiate(elevationObject[elevId],parent.transform);
        newElev.GetComponentInChildren<Renderer>().material = elevationMaterial[biomeId];
        parent.transform.parent = this.gameObject.transform;

    }
    public void showTile(int coordX, int coordY,int tileId,int tileAltitude)
    {
        GameObject newTileObject = new GameObject("Tile "+ coordX.ToString()+" "+coordY.ToString());
        newTileObject.transform.position = new Vector3(coordX * tileSize, 0, coordY * tileSize);
        GameObject upObject=null;
        if (tileAltitude == 0)
        {
            upObject = Instantiate(waterTile, newTileObject.transform);
            upObject.transform.localPosition=new Vector3(0, -0.3f, 0);
        }
        else if (tileAltitude ==1)
        {
            upObject = Instantiate(plainTilesObject[tileId], newTileObject.transform);
        }
        else if (tileAltitude == 2)
        {
            spawnElevation(tileId, 0,newTileObject);
        }
        else if (tileAltitude >2)
        {
            upObject = Instantiate(plateauTileObjects[tileId], newTileObject.transform);

        }
        if(upObject != null)
        {
            newTileObject.transform.parent = this.gameObject.transform;
        }

    }
}
