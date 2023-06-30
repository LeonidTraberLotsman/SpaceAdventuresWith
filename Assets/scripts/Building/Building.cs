using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public Vector3 CorrectionVector;
    protected Color NativeColor = Color.clear;
    // Start is called before the first frame update
    /*
     This is abstract class for every thing, that can be build

     
     */

    // Checking if can be built, if not returning the reason, else ""
    public virtual void Paint(Color newColor)
    {
        if(NativeColor==Color.clear) NativeColor=GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = newColor;
    }

    public virtual void PaintBack()
    {
        GetComponent<Renderer>().material.color = NativeColor;
    }

    public abstract string CanBuild(Vector2 where,ShipManager ship);

    //Build that building On ship
    public virtual void Build(Vector2 where, ShipManager ship)
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
