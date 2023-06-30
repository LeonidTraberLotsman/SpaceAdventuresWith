using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingChosingButton : MonoBehaviour
{
    public GUI gui;
    public int PrefabNum;

    public Text my_text;

    public void Choose()
    {
        gui.ChosenPotentialBuilding = PrefabNum;
        gui.ShowBuildingButtons(false);
        Debug.Log("chosen");
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
