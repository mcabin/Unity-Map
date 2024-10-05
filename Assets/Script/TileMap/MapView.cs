using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Script
{

    public class MapView : MonoBehaviour
    {
        TileMapModel mapModel;
        public int mapWidth, mapHeight = 10;
        public int altitudeScale, temperatureModifier, moistureModifier = 1;
        public int seed;
        public PathFinding pathfinding;
        // Start is called before the first frame update
        void Start()
        {
            GenerateMap generator = new GenerateMap(mapWidth, mapHeight, altitudeScale, seed, temperatureModifier, moistureModifier);
            mapModel = generator.generateTilesMap();
            pathfinding.Initialize(mapWidth, mapHeight, generator.createListOfTileNode());

            for (int h = 0; h < mapHeight; h++)
            {
                for (int w = 0; w < mapWidth; w++)
                {
                    GameObject tileObject = new GameObject("Tile " + w + " " + h);
                    TileView tileScript = tileObject.AddComponent<TileView>();
                    tileScript.Initialize(mapModel.getTile(w, h));
                    tileScript.transform.SetParent(transform);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}