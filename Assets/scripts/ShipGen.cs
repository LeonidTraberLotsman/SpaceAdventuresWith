using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGen : MonoBehaviour
{
    public ShipManager ship;

    // Start is called before the first frame update
    void Start()
    {
        ship = GetComponent<ShipManager>();
        //Generate();
        StartCoroutine(gen());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator gen()
    {
        yield return new WaitForSeconds(1);
        Generate();
    }
    void FillFloor(Vector2 start, Vector2 finish)
    {
        for (int x = (int)start.x; x < finish.x; x++)
        {
            for (int y = (int)start.y; y < finish.y; y++)
            {
                ship.AddBuilding(new Vector2(x, y), 0, 0);
            }
        }
    }
    void GenereteRoom(Vector2 start, Vector2 finish)
    {
        FillFloor(start, finish);
        for (int x = (int)start.x; x < finish.x; x++)
        {

            ship.AddBuilding(new Vector2(x, start.y), 1, 0);
            ship.AddBuilding(new Vector2(x, finish.y), 1, 0);
        }
        for (int y = (int)start.y; y < finish.y; y++)
        {

            ship.AddBuilding(new Vector2(start.x, y), 2, 0);
            ship.AddBuilding(new Vector2(finish.x, y), 2, 0);
        }
    }

    public void Generate()
    {
        //Debug.Log("generating ship");
        //ShipManager.Cell cell= ship.GetCell(new Vector2(0, 0));
        Vector2 point = new Vector2(20, 20);
        int cockpitLeng = Random.Range(1, 5);
        GenereteRoom(point, point + new Vector2(cockpitLeng, 2));
        GenereteRoom(point + new Vector2(cockpitLeng, 0), point + new Vector2(cockpitLeng + 7, 2));
        GenereteRoom(point + new Vector2(0, 2), point + new Vector2(2, 4));
        //GenereteRoom(new Vector2(4, 4), new Vector2(9, 9));
        ship.AddBuilding(point + new Vector2(1, 3), 6, 3);
        ship.AddBuilding(point + new Vector2(2, 4), 6, 3);
    }
}