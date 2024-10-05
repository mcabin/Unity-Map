using UnityEditor;
using UnityEngine;

namespace Assets.Script
{
    public class TileSelection : MonoBehaviour
    {
        public TileView tileSelected;

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
        }

        public void Deselect()
        {
            tileSelected.DeSelect();
        }
    }
}