using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
            //pull out evidence panel
        }

    }

    void Turn_on_Panel(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().alpha = 1;
        panel.GetComponent<CanvasGroup>().interactable = true;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }


    void Turn_off_Panel(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().alpha = 0;
        panel.GetComponent<CanvasGroup>().interactable = false;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

}
