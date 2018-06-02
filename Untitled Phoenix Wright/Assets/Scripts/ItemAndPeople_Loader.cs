using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Converters;

//Still under construction, but handles a different type of Json

public class Type_Inventory // Type_Inventory stores information for each item and/or people profile.
                           // Such as name, description, unlocked or not, even more
                           
{
    public string display_name;
    public string description;
    public bool is_unlocked = false;
    

}


public class ItemAndPeople_Loader : MonoBehaviour {

    public List<Type_Inventory> items = new List<Type_Inventory>(); //create an empty list of items
    public List<Type_Inventory> people = new List<Type_Inventory>(); //create an empty list of people


    // Use this for initialization
    void Awake () {
        Load_Inventory("Item_List");
        Load_Inventory("People_List");


        //foreach (var item in items)
        //{

        //    Debug.Log(item.display_name);
        //    Debug.Log(item.description);
        //}

    }

    public void Load_Inventory(string content)
    {
        
        TextAsset rawJson = Resources.Load<TextAsset>(content);
//        Debug.Log(rawJson.text);
        Newtonsoft.Json.Linq.JObject Jo = Newtonsoft.Json.Linq.JObject.Parse(rawJson.text);
        

        foreach (var item in Jo[content])
        {

            int count = 0;
            Type_Inventory individual = new Type_Inventory();
            foreach(var value in item)
            {
                if (count == 0)//first item, name
                {
                    individual.display_name = value.First.ToString();


                }
                else if (count == 1)// 2nd item, description
                    individual.description = value.First.ToString();
                else if (count == 2)// 3rd item, is unlocked
                {
                    if (value.First.ToString() == "1")// unlocked already
                        individual.is_unlocked = true;
                    else
                        individual.is_unlocked = false;
                }

                count++;
            }

            if (content == "Item_List")
                items.Add(individual);
            else if (content == "People_List")
                people.Add(individual);
        }
    }

}
