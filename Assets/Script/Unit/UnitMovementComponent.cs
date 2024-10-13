using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script
{
    public class UnitMovementComponent : SelectableComponent
    {
        private Unit unit;
        private FoundPath currentPath;
        private PathArrow arrow;
        

        public override void Select()
        {
            base.Select();
            arrow.Activate();
            TileSelection.isUpdated+=tileIsUpdated;
        }

        public override void Deselect()
        {
            base.Deselect();
            arrow.Deactivate();
            TileSelection.isUpdated -= tileIsUpdated;
        }

        public void Initialize(Unit unit)
        {
           this.unit = unit;
        }
        // Use this for initialization
        void Start()
        {
            arrow=this.AddComponent<PathArrow>();
        }
        
        // Update is called once per frame
        void Update()
        {
            if (isSelected)
            {
                move();
            }
        }
        
        private void move()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.Right))
            {
                
            }
        }

        private void tileIsUpdated()
        {
            TileView tile = TileSelection.Instance.tileSelected;
            Debug.Log(tile.tile.coordW + " " + tile.tile.coordH);
            currentPath = PathFinding.Instance.findPath(unit, tile.tile.coordW, tile.tile.coordH);
            arrow.changePath(currentPath);
        }
    }
}