using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestimonyAnimation : MonoBehaviour {
    public GameObject chatbox;
    public GameObject court_record_container;
    public GameObject press_button_container;
    public GameObject audio_manager;
	
	void begin_animation()
    {
        if (chatbox != null)
            chatbox.SetActive(false);
        if (court_record_container != null)
            court_record_container.SetActive(false);
        if (press_button_container != null)
            press_button_container.SetActive(false);
        if (audio_manager != null)
            audio_manager.GetComponent<AudioManager>().ToggleBGM(false);
    }

    void finish_animation()
    {
        if (chatbox != null)
            chatbox.SetActive(true);
        if (court_record_container != null)
            court_record_container.SetActive(true);
        if (press_button_container != null)
            press_button_container.SetActive(true);
        if (audio_manager != null)
            audio_manager.GetComponent<AudioManager>().ToggleBGM(true);
        gameObject.SetActive(false);
    }
}
