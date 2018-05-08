using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is called by the Interactable script if it is attached to the same GameObject.
//Loads the file with the supplied name as an AudioClip and plays it.

public class SoundBehavior : MonoBehaviour, IInteractionBehavior {

    AudioSource audioSource;
    public string audio_name;
	// Use this for initialization
	void Start () {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>(audio_name);
	}
	
	public void PerformBehavior () {
        audioSource.Play();
	}
}
