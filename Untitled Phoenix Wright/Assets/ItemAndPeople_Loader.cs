using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Converters;

public class Type_Inventory // Type_Inventory stores information for each item and/or people profile.
                           // Such as name, description, unlocked or not, even more
                           
{
    public string display_name;
    public string description;
    public string file_name;
    public bool is_unlocked = false;
    

}


public class ItemAndPeople_Loader : MonoBehaviour {

    public List<Type_Inventory> items = new List<Type_Inventory>(); //create an empty list of items
    public List<Type_Inventory> people = new List<Type_Inventory>(); //create an empty list of people


    // Use this for initialization
    void Start () {
        Load_Inventory("Item_List");
	}
	
    public void Load_Inventory(string content)
    {
        
        TextAsset rawJson = Resources.Load<TextAsset>(content);
        //Debug.Log(rawJson.text);
        Newtonsoft.Json.Linq.JObject Jo = Newtonsoft.Json.Linq.JObject.Parse(rawJson.text);
        
        string section_name = "null";

        foreach (var item in Jo[content])
        {
            Debug.Log(item.First);
        }
    }

}
