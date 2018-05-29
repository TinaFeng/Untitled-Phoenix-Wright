using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour {
    // Use this for initialization
    public GameObject press_button;
    Dialogue_Manager dialogue_manager;
	void Start () {
        dialogue_manager = GameObject.FindGameObjectWithTag("Dialogue_Manager").GetComponent<Dialogue_Manager>();
	}
	
    void Update()
    {
        if (dialogue_manager.can_press == false)
        {
            press_button.SetActive(false);
        }
        else
        {
            press_button.SetActive(true);
        }
    }


	public void OnPress()
    {
        dialogue_manager.PressSection();
	}
}
