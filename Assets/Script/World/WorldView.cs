using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script.World
{
    public class WorldView : MonoBehaviour
    {
        private WorldModel worldModel;
        private MapView mapView;
        private Dictionary<Vector2Int,UnitView> unitView;
        public int mapWidth, mapHeight = 10;
        public int altitudeScale, temperatureModifier, moistureModifier = 1;
        public int seed;


        // Use this for initialization
        void Start()
        {
            MapView map=this.AddComponent<MapView>();
            map.Initialize(mapWidth,mapHeight,altitudeScale,temperatureModifier,moistureModifier, seed);
            map.transform.SetParent(transform);
            Vector2Int coord=new Vector2Int(0,0);
            Unit unit = new Unit(coord,UnitClass.getUnitClass(UnitEnum.ClassEnum.MAN_AT_ARMS));
            UnitView unitViewTest= this.AddComponent<UnitView>();
            unitViewTest.Initialize(unit);
            unitViewTest.transform.SetParent(this.transform);
            //this.unitView.Add(unit.coord,unitViewTest);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}