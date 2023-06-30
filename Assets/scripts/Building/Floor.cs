﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : Building
{
    public  override string CanBuild(Vector2 where, ShipManager ship)
    {
        string res = "";
        if ((where.x < 1 || where.x > ShipManager.MaxCells) || (where.y < 1 || where.y > ShipManager.MaxCells)) return "out of dock\n";
        ShipManager.Cell cell = ship.GetCell(where);
        if (cell != null && cell.floor != null) res+= "It exist\n";
        return res;
    }

    //Build that building On ship
    public override void Build(Vector2 where, ShipManager ship)
    {
       

        ShipManager.Cell cell = ship.GetCell(where);
        if (cell.floor == null)
        {
           
            cell.floor = this.gameObject;
           
            transform.position = ship.transform.position + new Vector3((where.x), 0, where.y);
        }
        else
        {
            Debug.Log("Floor exists");
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
