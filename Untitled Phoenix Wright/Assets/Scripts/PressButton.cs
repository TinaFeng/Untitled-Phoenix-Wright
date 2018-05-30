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

    }


	public void OnPress()
    {
        dialogue_manager.PressSection();
	}
}
