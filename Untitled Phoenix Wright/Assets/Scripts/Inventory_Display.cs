using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory_Display : MonoBehaviour {

    List<Type_Inventory> items;
    List<Type_Inventory> portrait;
    GameObject loader;
    public GameObject item_box;
    public GameObject EvidenceObtainedPanel; //Pop-up panel to show player they got new evidence

    List<GameObject> Item_List;
    List<GameObject> Portrait_List;
    // Use this for initialization
    void Start () { //find the list of items and people
        loader = GameObject.FindGameObjectWithTag("Script_Data");  //find the xml loader that has the xml data in the scene

        items = loader.GetComponent<ItemAndPeople_Loader>().items;
        Item_List = new List<GameObject>();
        portrait = loader.GetComponent<ItemAndPeople_Loader>().people ;
        Portrait_List = new List<GameObject>();
    }

    private void OnCanvasGroupChanged()
    {
        if (transform.GetChild(1).name =="Evidence")
        {
            if (items != null)
            {
                Instantiate_Items(items);
                Item_List[0].GetComponent<On_ButtonClick>().OnButtonClick("item");
            }
            


        }
        else if (transform.GetChild(1).name=="Portrait")
        {
            if (portrait != null)
            {
                Instantiate_Items(portrait);
            }
            Portrait_List[0].GetComponent<On_ButtonClick>().OnButtonClick("item");
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
            if (item_collection == items)
                Item_List.Add(container.GetChild(0).gameObject);
            else
                Portrait_List.Add(container.GetChild(0).gameObject);
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
                item.GetComponent<Item_Button_Behavior>().item_description = item_collection[i].description;

                if (!item_collection[i].is_unlocked)
                {
                    item.GetComponent<Button>().interactable = false;
                    item.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/" + "Items/" + "Inventory Background");
                }
                else
                {
                    item.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/" + "Items/" + item_collection[i].display_name);
                }

                item.GetComponent<RectTransform>().Translate(new Vector3(i * item.GetComponent<RectTransform>().rect.width, 0, 0));

                if (item_collection == items)
                    Item_List.Add(item);
                else
                    Portrait_List.Add(item);
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

    //Called by DialogueManager to unlock an item.
    public void UnlockItem (string newItem)
    {
        //Debug.Log(newItem);
        foreach (Type_Inventory i in items)
        {
            Debug.Log(newItem + " " + i.display_name);
            if (i.display_name.Equals(newItem))
            {
                Debug.Log("Should unlock");
                if (!i.is_unlocked)
                {
                    i.is_unlocked = true;
                    //Animation
                    Debug.Log("Unlocked " + i.display_name);
                    foreach (GameObject j in Item_List)
                    {
                        Debug.Log(j.GetComponent<Item_Button_Behavior>().item_name);
                        if (j.GetComponent<Item_Button_Behavior>().item_name.Equals(newItem))
                        {
                            j.GetComponent<Button>().interactable = true;
                            if (i.display_name != null)
                                j.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/" + "Items/" + i.display_name);
                            else
                                j.GetComponent<Image>().sprite = null;
                        }
                    }

                    StartCoroutine(DisplayEvidenceObtained());
                    //StartCoroutine(Wait(2.0f));
                    //Debug.Log("coroutine one");
                }

            }
        }
    }

    //Temporarly displays evidence obtained message
    IEnumerator DisplayEvidenceObtained()
    {
        //Show evidence obtained panel for a bit
        //Maybe edit to include the name of the evidence.
        EvidenceObtainedPanel.SetActive(true);
        //Hides the evidenceobtained panel when the player presses any key
        yield return new WaitUntil(() => Input.anyKey == true);
        EvidenceObtainedPanel.SetActive(false);
    }
}
