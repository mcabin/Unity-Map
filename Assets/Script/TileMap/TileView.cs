using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Script
{

    public class TileView : MonoBehaviour
    {
        public Tile tile;
        GameObject model;
        private bool isSelected;
        public static event Action<TileView> OnMouseOverTile;
        public void Select()
        {
            isSelected = true;
        }

        public void DeSelect()
        {
            isSelected=false;
        }
        public void Initialize(Tile tile)
        {
            this.tile = tile;
            model = Instantiate(MapViewAsset.getTileModel(tile.altitude.elevationType.elev));
            Material material = MapViewAsset.getTileMaterial(tile.biome.type);
            this.transform.position = MapViewAsset.getTilePosition(tile.coordW, tile.coordH);
            this.transform.Rotate(0, tile.altitude.elevationType.rotation, 0);
            //Material
            Renderer renderer = model.GetComponent<Renderer>();
            if (renderer == null)
            {
                renderer = model.AddComponent<MeshRenderer>();
            }
            model.transform.SetParent(this.transform, false);
            renderer.material = material;
            //Collider
            createCollider();
            //Name
            this.name = "Tile " + tile.coordW + ", " + tile.coordH;
        }

        

        private void createCollider()
        {
            int colliderWidthSize = 2;
            int colliderHeightSize = 2;
            BoxCollider collider = this.AddComponent<BoxCollider>();
            collider.enabled = true;
            if (tile.altitude.type == TileEnum.AltitudeEnum.PLAIN)
            {
                collider.size = new Vector3(colliderHeightSize, 0.3f, colliderWidthSize);
                collider.center = new Vector3(0, -0.25f, 0);
            }
            else if (tile.altitude.type == TileEnum.AltitudeEnum.PLATEAU)
            {
                collider.size = new Vector3(colliderHeightSize, 0.8f, colliderWidthSize);
            }
            else if (tile.altitude.type == TileEnum.AltitudeEnum.SEA)
            {
                collider.size = new Vector3(colliderHeightSize, 0.18f, colliderHeightSize);
                collider.center = new Vector3(0, -0.31f, 0);

            }
        }
        private void OnMouseEnter()
        {
            if (tile != null)
            {
                OnMouseOverTile?.Invoke(this);
            }
        }
    }
}