using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script
{
    public class UnitView : SelectableObject
    {
        private Unit unit;
        private GameObject model;
        public void Initialize(Unit unit)
        {
            this.unit = unit;
            model = Instantiate(UnitViewAsset.getUnitModel(unit.getUnitClass(0).type));
         
            this.transform.position = UnitViewAsset.getUnitPostion(unit.coord);
            model.transform.SetParent(this.transform);
            UnitMovementComponent moveComp = this.AddComponent<UnitMovementComponent>();
            moveComp.Initialize(unit);
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}