using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//might not work after Json changes

public class Interactable : MonoBehaviour {

    //This is a really bad way of making mouse interaction work. Please code a better one
    //This Script takes: the dialogue manager panel, and cursor manager place holder.

    //item: the item's name, which will be connected to dictionary key
    //dialogue_manager/cursor_manager: the object will call them when interacting with the object
    
    public string item;


    public GameObject dialogue_manager;
    public GameObject cursor_manager;

    private void OnMouseEnter()//when hover over, change cursor to interact
    {
        
        Cursor.SetCursor(cursor_manager.GetComponent<Cursor_Manager>().Interactable, Vector2.zero, CursorMode.Auto);

        //Interactable and Normal in the SetCursor function are cursor textures in Cursor_Manager
    }

    private void OnMouseExit()//when mouse leaves, return to normal
    {

        Cursor.SetCursor(cursor_manager.GetComponent<Cursor_Manager>().Normal, Vector2.zero, CursorMode.Auto);
    }
    private void OnMouseDown() //when click, if it has a name(interactables have a public string recording its name), call dialogue manager and play description
    {
        if (item != "")
        {
            dialogue_manager.GetComponent<Dialogue_Manager>().section_call = item;//assign tag to dialogue manager so that it can play
            dialogue_manager.gameObject.SetActive(true);//active dialogue manager

        }
    }

}
