using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script
{
    public class UnitView : SelectableObject
    {
        private Unit unit;

        public void Initialize(Unit unit)
        {
            this.unit = unit;
            UnitMovementComponent moveComp=this.AddComponent<UnitMovementComponent>();
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