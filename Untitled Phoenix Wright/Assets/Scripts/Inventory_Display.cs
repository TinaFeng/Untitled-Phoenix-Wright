using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory_Display : MonoBehaviour {

    List<Type_Inventory> items;
    List<Type_Inventory> portrait;
    GameObject loader;
    public GameObject item_box;

    
    List<GameObject> Item_List;
    List<GameObject> Protrait_List;
    // Use this for initialization
    void Start () { //find the list of items and people
        loader = GameObject.FindGameObjectWithTag("Script_Data");  //find the xml loader that has the xml data in the scene

        items = loader.GetComponent<ItemAndPeople_Loader>().items;


        Item_List = new List<GameObject>();
        
        //portrait = GetComponent<ItemAndPeople_Loader>().people;
	}

    private void OnCanvasGroupChanged()
    {
        if (transform.GetChild(1).name =="Evidence")
        {
            if (items != null)
            {
                Instantiate_Items(items);
                
            }

        }

    }
    // Update is called once per frame
    void Update () {


      //  Debug.Log(GetComponent<RectTransform>().rect.width);
    }


    public void Instantiate_Items(List<Type_Inventory> item_collection)
    {
        Transform[] currentlist = { };

        Transform container = transform.GetChild(1).GetChild(0); //the panel (evidence/people) currently on top's container

        if (container.childCount == 1)
        {
            Item_List.Add(container.GetChild(0).gameObject);
        }
        if (container.childCount >1)
        {
            currentlist = container.GetComponentsInChildren<Transform>();
            
                
        }
        for (int i = 0; i != item_collection.Count; i++)
        {
         //   Debug.Log(item_collection[i].display_name);
            if ( !Search(currentlist, item_collection[i].display_name) )
            {
                GameObject item = Instantiate(item_box, container);
                item.GetComponent<Item_Button_Behavior>().item_name = item_collection[i].display_name;
                item.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/" + "Items/" + item_collection[i].display_name);


                item.GetComponent<RectTransform>().Translate(new Vector3 (i*item.GetComponent<RectTransform>().rect.width, 0,0));

                Item_List.Add(item);
                //Debug.Log(item.GetComponent<Item_Button_Behavior>().item_name);    
                //item's position = i * something something . Align them nicely
                
                //                Debug.Log(container.parent.GetComponent<Rect>().height);
                //                Debug.Log(i);
            }

            
        }

    }

    private bool Search (Transform[] children, string name)
    {
        if (children.Length == 0)
            return false;
        for (int i = 1;i!= children.Length;i++)
        {
            
            if (children[i].GetComponent<Item_Button_Behavior>().item_name == name)
                return true;
        }
        
            return false;
    }
}
