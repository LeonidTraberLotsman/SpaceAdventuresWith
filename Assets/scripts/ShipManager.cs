using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShipManager : MonoBehaviour
{
    public const int MaxCells = 50;

    public int energy = 0;
    
    public List<ActiveFurniture> furnitures = new List<ActiveFurniture>();
    public GameObject floorPrefab;
    public Cell[][] Cells= new Cell[MaxCells][];

    public GameObject[] PotentialsPrafabes;

    Room spaceRoom;
    public List<Room> rooms = new List<Room>();

    public class Room
    {
        public int num;
        public Color color;
        public List<Cell> cells = new List<Cell>();
        public Room()
        {
            color = new Color(Random.value, Random.value, Random.value);
        }
    }

    public class Cell
    {
        public GameObject floor=null;
        public GameObject NorthWall;
        public GameObject WestWall;
        public ActiveFurniture furniture;
        public Room room;


        public string Serialise(Vector2 where, ShipManager ship)
        {
            string res = "<";
            res += "(" + where.x.ToString() + "," + where.y.ToString() + ")";
            if (floor != null) res += "+"; else res += "-";
            if (NorthWall != null) { 
             if(NorthWall.GetComponent<NorthDoor>()) res += "_";
                else res += "+"; 
            } else res += "-";
            if (WestWall != null)
            {
                if (WestWall.GetComponent<WestDoor>()) res += "_";
                else res += "+";
            }
            else res += "-";

            if (furniture != null)
            {
                for (int i =3; i < ship.PotentialsPrafabes.Length; i++)
                {
                    if (furniture.transform.name == ship.PotentialsPrafabes[i].transform.name)
                    {
                        res += i.ToString()+",";
                        break;
                    }
                }
               
                res += (furniture.transform.rotation.eulerAngles.y / 90).ToString();


            }
            else
            {
                res += "-1,";
            }

            res += ">";
            return res;
        }
    }

    public Cell GetCell(Vector2 where)
    {
        Cell res1 = Cells[10][10];
        Cell res = Cells[(int)where.x][(int)where.y];
        if (res == null)
        {
            //Debug.Log("Created cell " + where.x + " " + where.y);
            res=Cells[(int)where.x][(int)where.y] = new Cell();
        }
        return res;
        
    }

   
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < MaxCells; i++)
        {
            Cells[i] =new  Cell[MaxCells];
       
        }
        RecalculateRooms();
    }

    public void RecalculateRooms()
    {
        Debug.Log("Recalculate");
        int c = 0;

        //creating space room
        rooms = new List<Room>();
        spaceRoom = new Room();
        spaceRoom.num = 0;
        rooms.Add(spaceRoom);

        foreach (GameObject marker in markers)
        {
            Destroy(marker);
        }

        //deleting old rooms, if it exists
        for (int x = 0; x < MaxCells; x++)
        {
            for (int y = 0; y < MaxCells; y++)
            {
                Cell that_cell = Cells[x][y];
                if (that_cell != null)
                {
                    c++;
                    that_cell.room = null;
                    if (that_cell.floor == null)
                    {
                        that_cell.room = spaceRoom;
                    }
                }

            }

        }

        for (int x = 0; x < MaxCells; x++)
        {
            for (int y = 0; y < MaxCells; y++)
            {
                Cell that_cell = Cells[x][y];
                if (that_cell != null)
                {
                    //logic of room bonding
                    if (that_cell.room == null)
                    {
                        bool ShouldCreateRoom = true;

                        //x axis logic

                        

                        if (that_cell.WestWall == null)
                        {
                            if (Cells[x - 1][y] != null)
                            {
                                if (Cells[x - 1][y].room != null)
                                {
                                    that_cell.room = Cells[x - 1][y].room;
                                    that_cell.room.cells.Add(that_cell);
                                    ShouldCreateRoom = false;
                                }

                            }
                            else
                            {
                                that_cell.room = spaceRoom;
                                that_cell.room.cells.Add(that_cell);
                                ShouldCreateRoom = false;
                            }
                        }



                        // y axis logic

                        if (that_cell.NorthWall == null)
                        {
                            if (Cells[x][y - 1] != null)
                            {
                                if (Cells[x][y - 1].room != null)
                                {
                                    that_cell.room = Cells[x ][y- 1].room;
                                    that_cell.room.cells.Add(that_cell);
                                    ShouldCreateRoom = false;
                                }

                            }
                            else
                            {
                                that_cell.room = spaceRoom;
                                that_cell.room.cells.Add(that_cell);
                                ShouldCreateRoom = false;
                            }
                        }

                        //new room creating
                        if (ShouldCreateRoom)
                        {
                            Room new_room = new Room();
                            new_room.num = rooms.Count;
                            rooms.Add(new_room);
                            that_cell.room = new_room;
                            that_cell.room.cells.Add(that_cell);
                        }

                        //check if it is space connected from backward

                        //x axis
                        if (Cells[x + 1][y] == null)
                        {

                            RoomToSpace(that_cell.room);
                        }
                        else
                        {
                            if (Cells[x + 1][y].WestWall == null && Cells[x + 1][y].room == spaceRoom)
                            {
                                RoomToSpace(that_cell.room);
                            }
                        }

                        //y axis

                        if (Cells[x][y + 1] == null)
                        {

                            RoomToSpace(that_cell.room);
                        }
                        else
                        {
                            if (Cells[x][y + 1].NorthWall == null && Cells[x][y + 1].room == spaceRoom)
                            {
                                RoomToSpace(that_cell.room);
                            }
                        }

                    }
                }

            }

        }

        VisualiseRooms();
        Debug.Log("c=" + c.ToString());
    }

    void VisualiseRooms()
    {
        //visualisation
        for (int x = 0; x < MaxCells; x++)
        {
            for (int y = 0; y < MaxCells; y++)
            {
                Cell that_cell = Cells[x][y];
                if (that_cell != null)
                {
                    if (that_cell.room != null)
                    {
                        PlaceMarker(new Vector2(x, y), that_cell.room.color);
                    }
                }

            }

        }
    }

    void RoomToSpace(Room that_room)
    {
        foreach (Cell that_cell in that_room.cells)
        {
            that_cell.room = spaceRoom;
        }
        rooms.Remove(that_room);
    }

    public GameObject markerPrefab;
    public List<GameObject> markers = new List<GameObject>();
    void PlaceMarker(Vector2 where, Color color)
    {
        GameObject marker = Instantiate(markerPrefab);
        marker.GetComponent<Renderer>().material.color = color;
        marker.transform.position = transform.position + new Vector3((where.x), 0, where.y);
        markers.Add(marker);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            RecalculateRooms();
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            DeleteShip();
        }
    }

    void DeleteShip()
    {
        for (int x = 0; x < MaxCells; x++)
        {
            for (int y = 0; y < MaxCells; y++)
            {
                Cell our_cell = Cells[x][y];
                if (our_cell != null)
                {
                    if (our_cell.floor != null) Destroy(our_cell.floor);
                    if (our_cell.NorthWall != null) Destroy(our_cell.NorthWall);
                    if (our_cell.WestWall != null) Destroy(our_cell.WestWall);
                    if (our_cell.furniture != null) Destroy(our_cell.furniture.gameObject);
                    
                }
                Cells[x][y] = null;
            }
        }
    }

    

    public void Save(string path)
    {
        string data = "";

        for (int x = 0; x < MaxCells; x++)
        {
            for (int y = 0; y < MaxCells; y++)
            {
                Cell our_cell = Cells[x][y];
                if (our_cell != null)
                {
                    if(our_cell.floor!=null || our_cell.NorthWall != null || our_cell.WestWall!= null || our_cell.furniture != null)
                    data += our_cell.Serialise(new Vector2(x, y),this);
                }
            }
        }

        StreamWriter NewFile = File.CreateText(path);
        NewFile.Write(data);
        NewFile.Close();
    }

    public void Load(string path)
    {

        try
        {
            StreamReader ReadFile = File.OpenText(path);
            string data = ReadFile.ReadToEnd();
            ReadFile.Close();

            DeleteShip();

            ParseShip(data);

            
        }
        catch (System.Exception)
        {
            Debug.Log("No file");

        }
       
    }

    void ParseShip(string ShipData)
    {
       
        string[] cellDatas=ShipData.Split(">");
        foreach (string CellData in cellDatas)
        {
            ParseCell(CellData);
        }
    }
    void ParseCell(string CellData)
    {
        if (CellData.Length < 3) return;

        int num = CellData.IndexOf('(');
        string coordStr = CellData.Substring(num+1);
        num = CellData.IndexOf(')');
        coordStr = coordStr.Substring(0,num-2);
        Debug.Log(coordStr);

        string[] coord= coordStr.Split(',');
        Vector2 where = new Vector2(int.Parse(coord[0]), int.Parse(coord[1]));
        //Debug.Log(where);
        Cell cell = GetCell(where);

        string restOfData = CellData.Substring(num+1);
        Debug.Log(restOfData);
        if (restOfData[0] == '+') AddBuilding(where, 0,0);
        if (restOfData[1] == '+') AddBuilding(where, 1,0);
        if (restOfData[1] == '_') { AddBuilding(where, 1,0); AddBuilding(where, 7,0); }
        if (restOfData[2] == '+') AddBuilding(where, 2,0);
        if (restOfData[2] == '_') { AddBuilding(where, 2,0); AddBuilding(where, 8,0); }

        try
        {
            restOfData = CellData.Substring(num + 4);
            num = restOfData.IndexOf(',');
            
            int rot= 0;
            try
            {
               
                rot = int.Parse(restOfData.Substring(num+1));
              
               
            }
            catch (System.Exception)
            {

               
            }
            
            AddBuilding(where, int.Parse(restOfData.Substring(0, num)),rot);


        }
        catch (System.Exception)
        {

            
        }
       

    }
    public void AddBuilding(Vector2 where,int what,int rot)
    {
        GameObject potential = Instantiate(PotentialsPrafabes[what]);
        Building building = potential.GetComponent<Building>();
        potential.transform.name = PotentialsPrafabes[what].transform.name;
        //potential.transform.position = transform.position + new Vector3((where.x), 0, where.y) + building.CorrectionVector;
        building.Build(where, this);

        Debug.Log("res3:" + rot.ToString());
        potential.transform.Rotate(Vector3.up * rot*90);

        Debug.Log("It works");
        Debug.Log(potential.transform.name);

    }
}
