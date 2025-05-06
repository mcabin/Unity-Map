using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Assets.Script
{
    public class PathArrow : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private FoundPath path;

        void Awake()
        {
            // Récupère ou ajoute un LineRenderer
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }
            lineRenderer.enabled = false;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
        }

        public void Activate()
        {
            lineRenderer.enabled = true;
        }

        public void Deactivate()
        {
            lineRenderer.enabled = false;
        }

        public void changePath(FoundPath newPath)
        {
            if (newPath == null || newPath.Count == 0)
            {
                Deactivate();
                return;
            }

            this.path = newPath;
            lineRenderer.positionCount = path.Count;

            for (int i = 0; i < path.Count; i++)
            {
                SimpleNode node = path.getNode(i);
                lineRenderer.SetPosition(i, MapViewAsset.getTilePosition(node.coord, 1));
            }

            Activate();
        }
    }
}
