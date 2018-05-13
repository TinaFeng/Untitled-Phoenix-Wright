using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is called by the Interactable script if it is attached to the same GameObject.
//Activates the supplied GameObject.

public class TestBehavior2 : MonoBehaviour, IInteractionBehavior {

    public GameObject obj;

    public void PerformBehavior()
    {
        obj.SetActive(true);
    }
}
