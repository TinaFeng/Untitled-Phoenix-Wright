using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is called by the Interactable script if it is attached to the same GameObject.
//Deactivates the GameObject this script is attached to.

public class TestBehavior : MonoBehaviour, IInteractionBehavior {

	public void PerformBehavior()
    {
        if(gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
	
}
