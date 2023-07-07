using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUI : MonoBehaviour
{
    public Mode mode = Mode.None;

    public Transform canvas;

    public int ChosenPotentialBuilding=0;
    public GameObject[] PotentialsPrafabes;
    GameObject potential;
    public Text RedText;
    public Text TopText;
    ShipManager shipManager;

    public GameObject CrewManagmentCanvas;

    public List<GameObject> Buttons;
    Vector3 BuildingRotation= new Vector3();

    public GameObject consumer_hud_prefab;

    public enum Mode { None, Build,CrewManagment,EnergyManagment}

    public GameObject button_prefab;
    public List<GameObject> temp_buttons = new List<GameObject>();

    List<energy_hud> energy_Huds = new List<energy_hud>();

    // Start is called before the first frame update
    void Start()
    {
        shipManager=GetComponent<ShipManager>();
        PotentialsPrafabes = shipManager.PotentialsPrafabes;
        //foreach (var item in Buttons)
        //{
        //    item.SetActive(false);
        //}
        ShowBuildingButtons(false);
        TopText.text = "Press B to open building list\nPress C to open crew managment";
        ChangeMode(Mode.None);

    }
    public void MarkPotentialBuilding(Vector2 where)
    {
        
        if (potential != null)
        {
            if (ChosenPotentialBuilding < 0)
            {
                Destroy(potential.gameObject);
                potential = null;
                return;
            }
            if (potential.transform.name != PotentialsPrafabes[ChosenPotentialBuilding].name)
            {
                Destroy(potential.gameObject);
                potential = null;
                return;
            }
            else
            {
                
            }
        }
        else
        {
            if (ChosenPotentialBuilding < 0) return;
            potential = Instantiate(PotentialsPrafabes[ChosenPotentialBuilding]);
            potential.transform.name = PotentialsPrafabes[ChosenPotentialBuilding].transform.name;
            if(ChosenPotentialBuilding > 2) potential.transform.Rotate( BuildingRotation);

            TopText.text = "Press Esc to open list";
            //foreach (var item in Buttons)
            //{
            //    item.SetActive(false);
            //}
            ShowBuildingButtons(false);


        }
        Building building = potential.GetComponent<Building>();

        string checkResult=building.CanBuild(where,shipManager);

        


        potential.transform.position = shipManager.transform.position + new Vector3((where.x), 0, where.y)+building.CorrectionVector;
        RedText.text = checkResult;
        if (checkResult == "") 
        {
            building.Paint( Color.green);
            if (Input.GetMouseButtonDown(0))
            {
                building.PaintBack();
                BuildingRotation=building.transform.rotation.eulerAngles;
                building.Build(where,shipManager);
                potential = null; 
                return;
            }
        }
        else
        {
            building.Paint(Color.red);
        }
    }

    

    public void ShowBuildingButtons(bool state)
    {
        if (state)
        {
            ShowBuildingButtons(false);
            foreach (var item in Buttons)
            {
                item.SetActive(true);
            }

            for (int i = 10; i < PotentialsPrafabes.Length; i++)
            {
                //Debug.Log(PotentialsPrafabes[i].transform.name);
                GameObject new_button = Instantiate(button_prefab);
                new_button.transform.name = PotentialsPrafabes[i].transform.name;
                temp_buttons.Add(new_button);
                new_button.transform.parent = canvas;

                RectTransform rect = new_button.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(112, -20 - 30 * (i - 10));

                BuildingChosingButton chosingButton = new_button.GetComponent<BuildingChosingButton>();
                chosingButton.my_text.text = PotentialsPrafabes[i].transform.name.Replace("_", " ");
                chosingButton.PrefabNum = i;
                chosingButton.gui = this;

            }
        }
        else {
            foreach (GameObject item in temp_buttons.ToArray())
            {
                temp_buttons.Remove(item);
                Destroy(item.gameObject);
                //Debug.Log("no more buttons");
            }
            //temp_buttons = new List<GameObject>();

            foreach (var item in Buttons)
            {
                item.SetActive(false);
            }

        }

    }

    void ShowConsumers()
    {
        foreach (ActiveFurniture furniture in shipManager.furnitures)
        {
            GameObject hud = Instantiate(consumer_hud_prefab);
            hud.transform.position = furniture.transform.position + Vector3.up*1f;
            energy_hud e_hud= hud.GetComponent<energy_hud>();
            e_hud.furniture = furniture;
            e_hud.ship = shipManager;
            energy_Huds.Add(e_hud);
        }
    }

    //void DeleteConsumers()
    //{
    //    foreach (energy_hud hud in energy_Huds.ToArray())
    //    {
    //        Destroy(hud.gameObject);
    //    }
    //    energy_Huds = new List<energy_hud>();
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.X) && mode == Mode.None)
        {
            ChangeMode(Mode.EnergyManagment);
           

        }
        if (Input.GetKeyUp(KeyCode.B) && mode==Mode.None)
        {
            ChangeMode(Mode.Build);
          
        }
        if (Input.GetKeyUp(KeyCode.C) && mode == Mode.None)
        {
            ChangeMode(Mode.CrewManagment);

        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            if(mode == Mode.Build)
            {
                if (potential != null)
                {
                    if (ChosenPotentialBuilding > 2 && ChosenPotentialBuilding!=7 && ChosenPotentialBuilding!=8)//Rotate furniture
                    {
                        potential.transform.Rotate(new Vector3(0, 90, 0));
                    }
                }

                //Rotating wall is different wall
                if (ChosenPotentialBuilding == 1)
                {
                    ChosenPotentialBuilding = 2;

                }
                else
                {
                    if (ChosenPotentialBuilding == 2)
                    {
                        ChosenPotentialBuilding = 1;

                    }
                }


                //Rotating door is different door
                if (ChosenPotentialBuilding == 7)
                {
                    ChosenPotentialBuilding = 8;

                }
                else
                {
                    if (ChosenPotentialBuilding == 8)
                    {
                        ChosenPotentialBuilding = 7;

                    }
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(mode == Mode.Build)
            {
                if (ChosenPotentialBuilding != -1)
                {
                    //foreach (var item in Buttons)
                    //{
                    //    item.SetActive(true);
                    //}

                    ShowBuildingButtons(true);

                    //foreach (GameObject item in temp_buttons)
                    //{
                    //    Destroy(item);

                    //}
                    //temp_buttons = new List<GameObject>();
                    ChosenPotentialBuilding = -1;
                }
                else
                {
                    ChangeMode(Mode.None);
                    ShowBuildingButtons(false);
                }
            }

            if (mode == Mode.CrewManagment)
            {
                CrewManagmentCanvas.SetActive(false);
                
            }

            if (mode == Mode.EnergyManagment)
            {
                ChangeMode(Mode.None);
            }
        }
    }


    void ChangeMode(Mode newMode)
    {
        

        if (mode == Mode.Build)
        {
            if (potential != null)
            {
                Destroy(potential.gameObject);
                potential = null;
            }
        }

        //if (mode == Mode.EnergyManagment)
        //{
        //    DeleteConsumers();
        //}

        if (mode == Mode.CrewManagment)
        {
            if (potential != null)
            {
                Destroy(potential.gameObject);
                potential = null;
            }
        }

        


        if (newMode==Mode.Build)
        {
            ShowBuildingButtons(true);
            ChosenPotentialBuilding = -1;
        }
        if (newMode == Mode.CrewManagment)
        {
            CrewManagmentCanvas.SetActive(true);
        }
        if (newMode == Mode.EnergyManagment)
        {
            ShowConsumers();
            Debug.Log("0_0");
        }

        mode = newMode;
    }

    public void SaveButtonClick()
    {
        Debug.Log("Saving");
        shipManager.Save("test.txt");
    }

    public void LoadButtonClick()
    {
        Debug.Log("Saving");
        shipManager.Load("test.txt");
    }
}
