using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class UnitClass
    {
        public float defaultHealth { get; private set; }
        public float defaultMovement { get; private set; }
        public List< UnitModifier > modifiers { get; private set; }
    }
}