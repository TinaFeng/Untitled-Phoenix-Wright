using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//A Unity UI script that handles all events when pressing a button

public class On_ButtonClick : MonoBehaviour {


    public void OnButtonClick(string button_name)
    {
        if (button_name == "Court Record")//if clicking court record button
        {
            GameObject court_panel = GameObject.FindGameObjectWithTag("Court_Record");
            if (court_panel.GetComponent<CanvasGroup>().alpha == 0)
                Turn_on_Panel(court_panel);
            else
                Turn_off_Panel(court_panel);
            //pull out Court Record
        }

        else if (button_name == "Evidence")
        {
            GameObject evidence_panel = GameObject.FindGameObjectWithTag("Evidence");
            Panel_LayerShift(evidence_panel);
        }
        else if (button_name == "Portrait")
        {
            GameObject portrait_panel = GameObject.FindGameObjectWithTag("Portrait");
            Panel_LayerShift(portrait_panel);
        }
        else if (button_name == "item")
        {
          //  Debug.Log(this.GetComponent<Item_Button_Behavior>().item_name);
            Item_Button_Behavior item_button = GetComponent<Item_Button_Behavior>();
            Display_Item(item_button.item_name, item_button.item_description);
        }
        else if (button_name == "Left")
        {
            GameObject panel;
            if (GameObject.FindGameObjectWithTag("Court_Record").transform.GetChild(1).name == "Evidence")
                panel = GameObject.FindGameObjectWithTag("Evidence").transform.GetChild(0).gameObject;
            else
                panel = GameObject.FindGameObjectWithTag("Portrait").transform.GetChild(0).gameObject;

            Scroll(panel, "Left");


        }
        else if (button_name == "Right")
        {
            GameObject panel;
//            Debug.Log(GameObject.FindGameObjectWithTag("Court_Record").transform.GetChild(1).name);
            if (GameObject.FindGameObjectWithTag("Court_Record").transform.GetChild(1).name == "Evidence")
                panel = GameObject.FindGameObjectWithTag("Evidence").transform.GetChild(0).gameObject;
            else
                panel = GameObject.FindGameObjectWithTag("Portrait").transform.GetChild(0).gameObject;

            Scroll(panel, "Right");
        }
        else if (button_name == "Present")
        {

            Present();
        }
        else if (button_name ==  "Press")
        {
            GameObject manager = GameObject.FindGameObjectWithTag("Dialogue_Manager");
             manager.GetComponent<Dialogue_Manager>().PressSection();
        }
 

    }
    void Present()
    {
 
        string presenting = GameObject.FindGameObjectWithTag("Display_Panel").transform.GetChild(1).GetChild(0).GetComponent<Text>().text;
        GameObject.FindGameObjectWithTag("Dialogue_Manager").GetComponent<Dialogue_Manager>().PresentEvidence(presenting);
        Turn_off_Panel(GameObject.FindGameObjectWithTag("Court_Record"));

    }
    void Display_Item(string itemname, string description)
    {
        GameObject panel = GameObject.FindGameObjectWithTag("Display_Panel");
        panel.transform.GetChild(0).GetComponent<Image>().sprite = GetComponent<Image>().sprite;//image
        panel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = itemname;
        panel.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = description;
    }
    void Turn_on_Panel(GameObject panel) // turn on a panel
    {
        panel.GetComponent<CanvasGroup>().alpha = 1;
        panel.GetComponent<CanvasGroup>().interactable = true;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    void Turn_off_Panel(GameObject panel) //turn off a panel
    {
        panel.GetComponent<CanvasGroup>().alpha = 0;
        panel.GetComponent<CanvasGroup>().interactable = false;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    void Panel_LayerShift(GameObject panel, int position = 0) // put a panel on top
    {
        int index = panel.transform.GetSiblingIndex();
        if (panel.name == "Evidence") // if we are uping index
        {
            index = GameObject.FindGameObjectWithTag("Portrait").transform.GetSiblingIndex();
            if (panel.transform.GetSiblingIndex() < index) // pull up only at bottom
                panel.transform.SetSiblingIndex(index);
        }
        else if (panel.name == "Portrait")
        {
            index = GameObject.FindGameObjectWithTag("Evidence").transform.GetSiblingIndex();
            if (panel.transform.GetSiblingIndex() < index) // pull up only at bottom
                panel.transform.SetSiblingIndex(index);
        }
        else
        panel.transform.SetSiblingIndex(index + position);
    }
    void Scroll(GameObject panel,string direction)
    {
        if (direction == "Left")
        {
            panel.GetComponent<GridLayoutGroup>().padding.left -= Mathf.RoundToInt(panel.GetComponent<GridLayoutGroup>().cellSize.x);
            LayoutRebuilder.MarkLayoutForRebuild(panel.GetComponent<RectTransform>());

        }
        if(direction == "Right")
        {
            //+ panel.GetComponent<GridLayoutGroup>().spacing.x
            panel.GetComponent<GridLayoutGroup>().padding.left += Mathf.RoundToInt(panel.GetComponent<GridLayoutGroup>().cellSize.x);
            LayoutRebuilder.MarkLayoutForRebuild(panel.GetComponent<RectTransform>());
        }
            
    }

    public void ChangeScene(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
