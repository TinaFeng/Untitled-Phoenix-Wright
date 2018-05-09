using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


/* Class to play various audio clips such as typing sounds,
 * sound effects, and background music.
 */

//For the full list of audio tags, see the Sound_List.json file in the Resources/Audio directory.

//Next steps: Fancy stuff (fade out)

public class AudioManager : MonoBehaviour
{
    //AudioSources
    //AudioSource for typing sounds
    public AudioSource type_source;
    //AudioSource for various scripted sound effects
    public AudioSource sfx_source;
    //AudioSource for background music
    public AudioSource bgm_source;
    //AudioSource for sound effects related to UI interactions (e.g. button presses, clicking to progress dialogue, etc.)
    public AudioSource ui_source;

    //Dictionaries associating AudioClips and sound file names to tags
    //Set public for testing purposes; change to private later

    /* Dictionary containing typing AudioClips
     */
    public Dictionary<string, AudioClip> voices = new Dictionary<string, AudioClip>();

    /* Dictionary containing file names and tags for sound effects.
     */
    public Dictionary<string, string> sfx_dict = new Dictionary<string, string>();

    /* Dictionary containing file names and tags for BGM
     */
    public Dictionary<string, string> bgm_dict = new Dictionary<string, string>();
    //AudioClip typing;

    //Hold the tag of the last assigned sound to the AudioSources.
    //Used to avoid redundant loading of sound files.
    string current_sfx = "";
    string current_bgm = "";
    string current_ui = "";

    //Determines whether the background music should be played.
    public bool bgm_playing = false;

    // Intitialization
    void Awake()
    {
        //Load sounds
        LoadSounds();

        //Initialize AudioSources
        type_source = gameObject.AddComponent<AudioSource>() as AudioSource;
        sfx_source = gameObject.AddComponent<AudioSource>() as AudioSource;
        bgm_source = gameObject.AddComponent<AudioSource>() as AudioSource;
        ui_source = gameObject.AddComponent<AudioSource>() as AudioSource;

        bgm_source.loop = true; //bgm loops by default; other sounds probably shouldn't

        //Set default values for AudioSources
        SetUISound("select");
        SetTypeSound("neutral"); //default typing sound
        SetBGM("lobby");
        type_source.volume = 0.4f;
        sfx_source.volume = 1.0f;
        ui_source.volume = 0.7f;
        bgm_source.volume = 0.5f;

        //ToggleBGM(true);

        //Attach Listeners to buttons for UI sounds
        AttachListenersToButtons();

    }

    // Update is called once per frame
    void Update()
    {
        //If background music is set to play and is not currently playing, play it.
        if (bgm_playing == true && bgm_source.isPlaying == false)
        {
            bgm_source.Play();
        }
        else
        {
            //If background music is set to not play and is currently playing, stop it.
            if (bgm_playing == false && bgm_source.isPlaying == true)
            {
                bgm_source.Stop();
            }
        }
    }

    // Options methods

    // Sets the background music on or off.
    // If the background music was on, turn it off, and vice versa.
    public void ToggleBGM()
    {
        bgm_playing = !bgm_playing;
    }

    // Sets the background music on or off.
    // True sets the background music to on, false sets it to off
    public void ToggleBGM(bool on)
    {
        bgm_playing = on;
    }

    //Sets the current backgroud music usig the associated tag
    public void SetBGM(string tag)
    {
        if (bgm_dict.ContainsKey(tag))
        {
            //No need to set if specified bgm is the same.
            if (tag != current_bgm)
            {
                bgm_source.Stop();
                bgm_source.clip = Resources.Load(bgm_dict[tag]) as AudioClip; ;
                //Set current_bgm to avoid redundant loading
                current_bgm = tag;
            }
        }
        else
        {
            TagNotFound(tag);
        }
    }

    // Set the current typing sound using the associated tag.
    public void SetTypeSound(string tag)
    {
        if (voices.ContainsKey(tag))
        {
            type_source.clip = voices[tag];
        }
        else
        {
            TagNotFound(tag);
        }
    }

    // Set the current sound played by ui
    public void SetUISound(string tag)
    {
        if (sfx_dict.ContainsKey(tag))
        {
            //No need to set if specified bgm is the same.
            if (tag != current_ui)
            {
                ui_source.clip = Resources.Load(sfx_dict[tag]) as AudioClip; 
                //Set current_bgm to avoid redundant loading
                current_ui = tag;
            }
        }
        else
        {
            TagNotFound(tag);
        }
    }

    // Sound Playing Methods

    // Plays the currently set typing sound.
    public void PlayTyping()
    {
        type_source.Play();
    }

    // Plays a single sound effect as specified by the tag.
    public void PlayOneSFX(string tag)
    {
        if (sfx_dict.ContainsKey(tag))
        {
            //No need to load sound effect if it was played last. 
            if (current_sfx != tag)
            {
                AudioClip sfx = Resources.Load(sfx_dict[tag]) as AudioClip;
                sfx_source.clip = sfx;
                current_sfx = tag;
            }
            sfx_source.Play();
        }
        else
        {
            TagNotFound(tag);
        }
    }

    // Sets the UI sound to "select" and plays it.
    void PlaySelect()
    {
        SetUISound("select");
        ui_source.Play();
    }

    
    // Volume methods

    
    //These methods set the volume for a particular AudioSource to a value between 0.0 and 1.0
    public void SetTypingVolume(float value)
    {
        if(0 <= value && value <= 1.0f)
            type_source.volume = value;
    }

    public void SetBGMVolume(float value)
    {
        if (0 <= value && value <= 1.0f)
            bgm_source.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        if (0 <= value && value <= 1.0f)
            sfx_source.volume = value;
    }

    public void SetUIVolume(float value)
    {
        if (0 <= value && value <= 1.0f)
            ui_source.volume = value;
    }


    // These methods decrease the volume of a particular AudioSource by the specified value,
    // or 5% if no value is specified.
    public void DecreaseTypingVolume(float value = 0.05f)
    {
        if(0 <= value && value <=1.0f)
        {
            if(type_source.volume - value < 0)
            {
                type_source.volume = 0.0f;
            }
            else
            {
                type_source.volume -= value;
            }
        }
    }

    public void DecreaseBGMVolume(float value = 0.05f)
    {
        if (0 <= value && value <= 1.0f)
        {
            if (bgm_source.volume - value < 0)
            {
                bgm_source.volume = 0.0f;
            }
            else
            {
                bgm_source.volume -= value;
            }
        }
    }

    public void DecreaseSFXVolume(float value = 0.05f)
    {
        if (0 <= value && value <= 1.0f)
        {
            if (sfx_source.volume - value < 0)
            {
                sfx_source.volume = 0.0f;
            }
            else
            {
                sfx_source.volume -= value;
            }
        }
    }

    public void DecreaseUIVolume(float value = 0.05f)
    {
        if (0 <= value && value <= 1.0f)
        {
            if (ui_source.volume - value < 0)
            {
                ui_source.volume = 0.0f;
            }
            else
            {
                ui_source.volume -= value;
            }
        }
    }


    // These methods increase the volume of a particular AudioSource by the specified value,
    // or 5% if no value is specified.
    public void IncreaseTypingVolume(float value = 0.05f)
    {
        if (0 <= value && value <= 1.0f)
        {
            if (type_source.volume + value > 1.0f)
            {
                type_source.volume = 1.0f;
            }
            else
            {
                type_source.volume += value;
            }
        }
    }

    public void IncreaseBGMVolume(float value = 0.05f)
    {
        if (0 <= value && value <= 1.0f)
        {
            if (bgm_source.volume + value > 1.0f)
            {
                bgm_source.volume = 1.0f;
            }
            else
            {
                bgm_source.volume += value;
            }
        }
    }

    public void IncreaseSFXVolume(float value = 0.05f)
    {
        if (0 <= value && value <= 1.0f)
        {
            if (sfx_source.volume + value > 1.0f)
            {
                sfx_source.volume = 1.0f;
            }
            else
            {
                sfx_source.volume += value;
            }
        }
    }

    public void IncreaseUIVolume(float value = 0.05f)
    {
        if (0 <= value && value <= 1.0f)
        {
            if (ui_source.volume + value > 1.0f)
            {
                ui_source.volume = 1.0f;
            }
            else
            {
                ui_source.volume += value;
            }
        }
    }
    
    
    // Internal Utility Methods


    // Initializes sound dictionaries from the "Sound_List" file.
    void LoadSounds()
    {
        voices = SoundParser.LoadAsClips("Typing");
        bgm_dict = SoundParser.LoadAsFilenames("BGM");
        sfx_dict = SoundParser.LoadAsFilenames("SFX");
    }

    // Attaches an event listener that calls the PlaySound method whenever a button is pressed.
    // Needs to be improved to not rely upon the canvas being named "Canvas"
    void AttachListenersToButtons()
    {
        GameObject canvas = GameObject.FindWithTag("Main Canvas");
        //Get all the buttons, active and inactive, on this canvas.
        Button[] buttons = canvas.GetComponentsInChildren<Button>(true);
        for(int i = 0; i< buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(PlaySelect);
        }
    }

    // Prints that the specified tag was not found.
    // Used if a tag isn't in a dictionary.
    void TagNotFound(string tag)
    {
        Debug.Log("Sound tag " + tag + " not found.");
    }

    // Prints out the contents of each dictionary.
    // Used for debugging problems with assigning sounds.
    void PrintDicts()
    {
        foreach (string key in voices.Keys)
        {
            print(key + ":" + voices[key]);
        }
        foreach (string key in bgm_dict.Keys)
        {
            print(key + ":" + bgm_dict[key]);
        }
        foreach (string key in sfx_dict.Keys)
        {
            print(key + ":" + sfx_dict[key]);
        }
    }
}

