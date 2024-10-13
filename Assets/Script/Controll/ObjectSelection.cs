using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class ObjectSelection : MonoBehaviour
    {

        public SelectableObject selectedObjects;

        private static ObjectSelection _instance;

        public static ObjectSelection Instance { get { return _instance; } }


        private void Awake()
        {
            if(_instance != null && _instance !=this)
            {
                Destroy(_instance);
            }
            else
            {
                _instance = this;
            }
        }
        public void Select(SelectableObject obj)
        {
            if(selectedObjects != null)
            {
                selectedObjects.Deselect();

            }
            obj.Select();
            selectedObjects = obj;
            
        }

        public void Deselect()
        {
            if(selectedObjects != null)
            {
                selectedObjects.Deselect();
            }
            selectedObjects = null;
        }
    }
}