using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    public GameObject model;
    private UnitModel unit;
    // Start is called before the first frame update
    public void Initialise(UnitModel unit)
    {
        this.unit = unit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
