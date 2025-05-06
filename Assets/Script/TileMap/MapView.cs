using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Script
{

    public class MapView : MonoBehaviour
    {
        TileMapModel mapModel;
        private int mapWidth, mapHeight = 10;
        private int altitudeScale, temperatureModifier, moistureModifier = 1;
        private int seed;

        public void Initialize(int mapWidth, int mapHeight, int altitudeScale, int temperatureModifier, int moistureModifier, int seed) {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.altitudeScale = altitudeScale;
            this.temperatureModifier = temperatureModifier;
            this.moistureModifier = moistureModifier;
            this.seed = seed;
            GameObject mapObject = new GameObject("Map");
            mapObject.transform.SetParent(transform);

            GenerateMap generator = new GenerateMap(mapWidth, mapHeight, altitudeScale, seed, temperatureModifier, moistureModifier);
            mapModel = generator.generateTilesMap();
            PathFinding.Initialize(mapWidth, mapHeight, generator.createListOfTileNode());

            for (int h = 0; h < mapHeight; h++)
            {
                for (int w = 0; w < mapWidth; w++)
                {
                    GameObject tileObject = new GameObject("Tile " + w + " " + h);
                    TileView tileScript = tileObject.AddComponent<TileView>();
                    tileScript.Initialize(mapModel.GetTile(w, h));
                    tileScript.transform.SetParent(mapObject.transform);
                }
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}