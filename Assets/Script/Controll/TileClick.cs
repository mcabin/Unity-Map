using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script.Controll
{
    public class TileClick : MonoBehaviour
    {

        private Camera myCamera;
        public LayerMask groundable;
        // Use this for initialization
        void Start()
        {
            myCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            handleClick();
        }

        private void handleClick()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.Left))
            {
                RaycastHit hit;
                Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundable))
                {
                    TileView obj = hit.collider.GetComponent<TileView>();
                    if (obj != null)
                    {
                        TileSelection.Instance.Select(obj);
                    }

                }
                else
                {
                    TileSelection.Instance.Deselect();
                }
            }
        }
    }
}