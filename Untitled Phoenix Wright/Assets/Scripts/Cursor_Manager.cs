using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//currently still under construction, this is a placeholder for all the cursor objects
public class Cursor_Manager : MonoBehaviour {

    public Texture2D Normal;
    public Texture2D Interactable;

    //container for two(more in the future maybe) cursor textures.

    //Detect if cursor is over interactable.
    void Update()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction, 0.05f);
        Debug.Log(hit.ToString());

        if (hit.transform != null && hit.transform.gameObject.GetComponent<Interactable>() != null)
        {
            Cursor.SetCursor(Interactable, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(Normal, Vector2.zero, CursorMode.Auto);
        }
    }
}
