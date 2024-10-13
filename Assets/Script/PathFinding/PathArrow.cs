using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Assets.Script
{
    public class PathArrow : MonoBehaviour
    {

        public LineRenderer lineRenderer;
        FoundPath path;
        // Start is called before the first frame update
        void Start()
        {
            lineRenderer = this.AddComponent<LineRenderer>();
            lineRenderer.enabled = false;
        }

        public void Activate()
        {
            lineRenderer.enabled = true;
        }
        public void Deactivate()
        {
            lineRenderer.enabled = false;

        }
        public void changePath(FoundPath path)
        {
            this.path = path;

            lineRenderer.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                SimpleNode node = path.getNode(i);
                lineRenderer.SetPosition(i, MapViewAsset.getTilePosition(node.coordW, node.coordH, 1));
            }
        }
   
        // Update is called once per frame
        void Update()
        {

        }
    }
}
