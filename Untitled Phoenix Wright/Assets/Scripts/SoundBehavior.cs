using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is called by the Interactable script if it is attached to the same GameObject.
//Loads the file with the supplied name as an AudioClip and plays it.

public class SoundBehavior : MonoBehaviour, IInteractionBehavior {

    public string audio_name;
    AudioManager audio_manager;
	// Use this for initialization
	void Start () {
        audio_manager = GameObject.FindGameObjectWithTag("Audio_Manager").GetComponent<AudioManager>();
	}
	
	public void PerformBehavior () {
        audio_manager.PlayOneSFX(audio_name);
	}
}
