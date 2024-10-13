using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class SelectableComponent : MonoBehaviour
    {

        public bool isSelected { get; private set; }

        public virtual void  Select()
        {
            isSelected = true;
        }

        public virtual void Deselect()
        {
            isSelected = false;
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}