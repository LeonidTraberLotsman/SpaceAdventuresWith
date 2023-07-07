using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveFurniture : Building
{

    public bool ON = true;
    public int energy_use = 0;

    public Renderer Therenderer;
    // Start is called before the first frame update

    public override void Paint(Color newColor)
    {
        if (NativeColor == Color.clear) NativeColor = Therenderer.material.color;
        Therenderer.material.color = newColor;
    }
    public override void PaintBack()
    {
        Therenderer.material.color = NativeColor;
    }



    public Transform InteractionPoint;

    public override string CanBuild(Vector2 where, ShipManager ship)
    {
        string res = "";
        if ((where.x < 1 || where.x > ShipManager.MaxCells) || (where.y < 1 || where.y > ShipManager.MaxCells)) return "out of dock\n";
        ShipManager.Cell cell = ship.GetCell(where);
        if (cell != null && cell.furniture != null) res += cell.furniture.transform.name +" exist\n";
        else Debug.Log(cell.furniture);
        if (cell != null && cell.floor == null) res += "Floor is needed\n";
        return res;
    }

    public override void Build(Vector2 where, ShipManager ship)
    {
        ShipManager.Cell cell = ship.GetCell(where);
        if (cell.furniture == null)
        {

            cell.furniture = this;
            //Debug.Log(cell.furniture);
            transform.position = ship.transform.position + new Vector3((where.x), 0, where.y)+CorrectionVector;
            
            ship.furnitures.Add(this);
        }
        else Debug.Log(cell.furniture.transform.name +" exists");
        //Destroy(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual IEnumerator Interact(Character that_character)
    {
        yield return (new Character.ReachPointTask(transform.position)).Do(that_character);
        yield return null;
        yield return new WaitForSeconds(5);
    }
}
