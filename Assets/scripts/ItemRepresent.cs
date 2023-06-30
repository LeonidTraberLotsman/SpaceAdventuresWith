using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRepresent : MonoBehaviour
{
    public  Item item;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public class Item
    {
        public int HowMuch;
        public GameObject prefab;
        public string name;
        public float weight;
        public float price;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
