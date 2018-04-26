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
}
