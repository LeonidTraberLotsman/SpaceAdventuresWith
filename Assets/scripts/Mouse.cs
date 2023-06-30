using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public Transform Cursor;
    public Vector3 Mpos;
    GUI gui;
    ShipManager manager;
    // Start is called before the first frame update
    void Start()
    {
        gui=GetComponent<GUI>();
        manager=GetComponent<ShipManager>();
    }

    // Update is called once per frame
    Vector2 GetCoordinates(Vector3 phys)
    {
        return new Vector2((int)phys.x, (int)phys.z);
    }

    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            Mpos = hit.point;
            var cord = GetCoordinates(Mpos);
            Cursor.transform.position= new Vector3(cord.x+0.5f,0,cord.y+0.5f) ;
        }
        if (gui.mode == GUI.Mode.Build)
        {
            gui.MarkPotentialBuilding(GetCoordinates(Mpos));
        }    
        
    }
}
