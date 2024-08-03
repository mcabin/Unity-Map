using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TileMapViewer : MonoBehaviour
{
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

    public void showElevation(int coordX,int coordY,int tileId,int elevId,int rotation)
    {
        GameObject newTileObject = new GameObject("Tile " + coordX.ToString() + " " + coordY.ToString());
        newTileObject.transform.position = new Vector3(coordX * tileSize, 0, coordY * tileSize);
        GameObject newElev = Instantiate(elevationObject[elevId], newTileObject.transform);
        newElev.GetComponentInChildren<Renderer>().material = elevationMaterial[tileId];
        newTileObject.transform.eulerAngles=new Vector3(transform.eulerAngles.x, rotation ,transform.eulerAngles.z);
        newTileObject.transform.parent = this.gameObject.transform;

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
