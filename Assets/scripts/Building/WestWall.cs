﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WestWall : Building
{
    public Renderer TheRenderer;
    // Start is called before the first frame update

    public override void Paint(Color newColor)
    {
        if (NativeColor == Color.clear) NativeColor = TheRenderer.material.color;
        TheRenderer.material.color = newColor;
    }
    public override void PaintBack()
    {
        TheRenderer.material.color = NativeColor;
    }

    public override string CanBuild(Vector2 where, ShipManager ship)
    {
        string res = "";
        if ((where.x < 1 || where.x > ShipManager.MaxCells) || (where.y < 1 || where.y > ShipManager.MaxCells)) return "out of dock\n";
        ShipManager.Cell cell = ship.GetCell(where);
        if (cell != null && cell.WestWall != null) res += "It exist\n";
        return res;
    }

    //Build that building On ship
    public override void Build(Vector2 where, ShipManager ship)
    {
        ShipManager.Cell cell = ship.GetCell(where);
        if (cell.WestWall == null)
        {
            cell.WestWall = this.gameObject;
            transform.position = ship.transform.position + new Vector3((where.x), 0, where.y);
        }
        else Debug.Log("WestWall exists");
        Destroy(this);
    }
}
