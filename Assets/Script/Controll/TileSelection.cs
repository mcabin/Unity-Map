using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Script
{
    public class TileSelection : MonoBehaviour
    {
        private Camera myCamera;
        public LayerMask groundable;
        public TileView tileSelected;
        public static event Action isUpdated;
        private static TileSelection _instance;

        public static TileSelection Instance { get { return _instance;  } }



        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(_instance);
            }
            else
            {
                _instance = this;
                TileView.OnMouseOverTile += Select;
            }
        }

        public void Select(TileView tile)
        {
            if(tileSelected != null)
            {
                tileSelected.DeSelect();
            }
            tile.Select();
            tileSelected = tile;
            isUpdated?.Invoke();
    }

    public void Deselect()
        {
            tileSelected.DeSelect();
        }
    }
}