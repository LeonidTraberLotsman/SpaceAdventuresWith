using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorthDoor : Building
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
        ShipManager.Cell nextCell = ship.GetCell(where+ new Vector2(0,1));
        if (nextCell != null && nextCell.furniture != null) res += "door is blocked by "+ nextCell.furniture.name + "\n";
        if (cell != null && cell.furniture != null) res += "door is blocked by "+ cell.furniture.name + "\n";
        if (cell != null && cell.NorthWall == null) { res += "Door must be placed on wall\n"; return res; }
        if (cell != null && cell.NorthWall.GetComponent<NorthDoor>())
            res += "Door exist\n";
        return res;
    }

    //Build that building On ship
    public override void Build(Vector2 where, ShipManager ship)
    {
        ShipManager.Cell cell = ship.GetCell(where);
        if (cell.NorthWall != null)
        {
            if (!cell.NorthWall.GetComponent<NorthDoor>()) Destroy(cell.NorthWall);
            else {
                Destroy(this.gameObject);
                return; }
            //wallpaper managment have to be there
            cell.NorthWall = this.gameObject;
            transform.position = ship.transform.position + new Vector3((where.x), 0, where.y)+ CorrectionVector;
        }
        else Debug.Log("Door can't be placed");
        //Destroy(this);
    }
}
