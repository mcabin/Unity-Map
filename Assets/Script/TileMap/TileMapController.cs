using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileMapController: MonoBehaviour
{
    public TileMapModel tileMap;
    public TileMapViewer tileMapView;
    public int mapHeight;
    public int mapWidth;
    public int seed;
    public float altitudeScale;
    public float temperatureModifier;
    public float moistureModifier;
    void Start()
    {
        tileMap = new TileMapModel(mapHeight, mapWidth,seed,altitudeScale,temperatureModifier,moistureModifier);
        tileMapView=GetComponent<TileMapViewer>();
        updateView();
    }
    private void Update()
    {
    }

    void updateView()
    {
        for (int h = 0; h < mapHeight; h++)
        {
            for (int w = 0; w < mapWidth; w++)
            {
                int tileId=(int)tileMap.getTile(w, h).biome.type;
                AltitudeType tileAltitude=tileMap.getTile(w, h).altitude;
                if(tileAltitude.type==TileEnum.AltitudeEnum.ELEVATION )
                {
                    TileElevation elevTile=(TileElevation)tileMap.getTile(w,h);
                    tileMapView.showElevation(w, h,tileId,(int)elevTile.elevationType.elev.enumElev,elevTile.elevationType.elev.rotation);
                }
                else
                {
                    tileMapView.showTile(w, h, tileId, tileAltitude.level);
                }
            }
        }
    }

}
