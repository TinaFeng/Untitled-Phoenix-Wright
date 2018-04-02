using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public string item;


    public GameObject dialogue_manager;
	// Use this for initialization
	void Start () {
     
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            if(item !="")
            {
                dialogue_manager.GetComponent<Dialogue_Manager>().section_call = item;
                dialogue_manager.gameObject.SetActive(true);
             
            }
        }
	}
}
