using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class SelectableObject : MonoBehaviour
    {
        protected bool isSelected=false ;

        public virtual void select()
        {
            isSelected = true;
        }

        public virtual void deselect() {
            isSelected = false;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}