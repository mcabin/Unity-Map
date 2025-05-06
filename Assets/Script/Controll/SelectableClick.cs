using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script
{
    public class SelectableClick : MonoBehaviour
    {
        private Camera myCamera;
        public LayerMask selectable;
        // Use this for initialization
        void Start()
        {
            myCamera=Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            handleClick();
        }

        private void handleClick()
        {
            if(Input.GetMouseButtonDown((int)MouseButton.Left)) {
                RaycastHit hit;
                Ray ray=myCamera.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit,Mathf.Infinity,selectable)) {
                    SelectableObject obj=hit.collider.GetComponent<SelectableObject>();
                    if (obj != null){
                        ObjectSelection.Instance.Select(obj);
                    }
                }
                else
                {
                    ObjectSelection.Instance.Deselect();
                }
            }
        }
    }
}