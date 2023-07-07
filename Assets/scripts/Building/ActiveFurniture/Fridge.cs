using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : StorageFurniture
{

    public override void Build(Vector2 where, ShipManager ship)
    {
        Debug.Log("fridge created");

        //ship.tasks.Add(new Character.InteractTask(this));

        //ShipManager.Cell cell = ship.GetCell(where);
        //if (cell.furniture == null)
        //{
        //    cell.furniture = this;
        //    //Debug.Log(cell.furniture);
        //    transform.position = ship.transform.position + new Vector3((where.x) - 0.5f, 0, where.y - 0.5f);
        //    ship.furnitures.Add(this);
        //}
        //else Debug.Log(cell.furniture.transform.name + " exists");
        ////Destroy(this);
    }


    // Start is called before the first frame update
    void Start()
    {

        //CorrectionVector = new Vector3(-0.5f, 0, -0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public override IEnumerator Interact(Character that_character)
    {
        yield return (new Character.ReachPointTask(transform.position)).Do(that_character);
        yield return null;
        yield return new WaitForSeconds(5);
    } 
}
