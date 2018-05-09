using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtons : MonoBehaviour {

    AudioManager audioManager;
    //Tag lists
    List<string> typing_tags;
    List<string> bgm_tags;
    List<string> sfx_tags;

    int current_bgm = 0;
    int current_voice = 0;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio_Manager").GetComponent<AudioManager>();
        typing_tags = new List<string>(audioManager.voices.Keys);
        bgm_tags = new List<string>(audioManager.bgm_dict.Keys);
        sfx_tags = new List<string>(audioManager.sfx_dict.Keys);
    }

	// Turns BGM on and off
	public void BGMOnOffTest()
    {       
        audioManager.ToggleBGM();
    }

    // Plays a random sound effect
    public void SFXTest()
    {
        string random_tag = "";
        while (random_tag == "")
        {     
            random_tag = sfx_tags[Random.Range(0, sfx_tags.Count)];
            if(random_tag == "select")
            {
                random_tag = "";
            }
        }
        
        audioManager.PlayOneSFX(random_tag);
    }

    public void NextBGM()
    {
        if(current_bgm == bgm_tags.Count-1)
        {
            current_bgm = 0;
        }
        else
        {
            current_bgm++;
        }
        audioManager.SetBGM(bgm_tags[current_bgm]);
    }

    public void NextTypeSound()
    {
        if (current_voice == typing_tags.Count - 1)
        {
            current_voice = 0;
        }
        else
        {
            current_voice++;
        }
        audioManager.SetTypeSound(typing_tags[current_voice]);
    }

    public void UpBGMVolume()
    {
        audioManager.IncreaseBGMVolume();
    }

    public void DownBGMVolume()
    {
        audioManager.DecreaseBGMVolume();
    }
}
