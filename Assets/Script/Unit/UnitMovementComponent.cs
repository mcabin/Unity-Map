using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script
{
    public class UnitMovementComponent : SelectableObject
    {
        private Unit unit;
        public PathFinding pathFinding;
        private FoundPath currentPath;
        public PathArrow arrow;


        public override void select()
        {
            base.select();
            arrow.Activate();
        }

        public override void deselect()
        {
            base.deselect();
            arrow.Deactivate();
        }

        void Initialise(Unit unit,PathFinding pathFinding)
        {
           this.pathFinding = pathFinding;
           this.unit = unit;
        }
        // Use this for initialization
        void Start()
        {
            PathArrow arrow=this.AddComponent<PathArrow>();
        }

        // Update is called once per frame
        void Update()
        {
            move();
        }

        private void move()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.Right))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.NameToLayer("Ground")))
                {
                    TileView obj = hit.collider.GetComponent<TileView>();
                    if (obj != null)
                    {
                        
                        List<TileNode> listNode = pathFinding.findPath(unit, obj.tile.coordW, obj.tile.coordH);
                        currentPath=new FoundPath(listNode);
                    }

                }
                
            }
        }
    }
}