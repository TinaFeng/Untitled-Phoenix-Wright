using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour {

    public GameObject black_panel;
    public GameObject chatbox;
    Animator anim;
    string next_scene;
    //string scene_path = "Scene/";
	void Start () {
        if(black_panel != null)
            anim = black_panel.GetComponent<Animator>();
	}
	
    //Hides chatbox while fading animations are playing.
	void Update () {
        if (anim != null && chatbox != null)
        {
            if (anim.GetBool("Talking") == false && chatbox.activeInHierarchy)
            {
                chatbox.SetActive(false);
            }
            else
            {
                if (anim.GetBool("Talking") == true && chatbox.activeInHierarchy == false)
                {
                    chatbox.SetActive(true);
                }
            }
        }
	}

    // Prepares for scene change. Scene change
    // doesn't actually occur until loaded and fade has completed.
    public void ChangeScene(string next)
    {
        next_scene = next;
        anim.SetBool("Talking", false);
        StartCoroutine(_NextSceneAsync());
    }

    //Experimenting w/ async scene loading
    IEnumerator _NextSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(next_scene);
        asyncLoad.allowSceneActivation = false;
        // Wait until fade finishes to change scene
        while(anim.GetBool("Finished") == false)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
