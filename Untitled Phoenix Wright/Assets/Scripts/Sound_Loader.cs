using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Converters;

//Used to load JSON file for AudioManager.
//Does not need to be attached to any GameObject; its static methods
//are called directly by the AudioManager script.

public class SoundParser{
    // Use this for initialization
    private const string json_filename = "Sound_List";
	
    //Loads the specified section as a dictionary of AudioClips.
    public static Dictionary<string, AudioClip> LoadAsClips(string section)
    {
        TextAsset rawJson = Resources.Load<TextAsset>(json_filename);
        Newtonsoft.Json.Linq.JObject Jo = Newtonsoft.Json.Linq.JObject.Parse(rawJson.text); //Parse Json
        Dictionary<string, AudioClip> clip_dict = new Dictionary<string, AudioClip>();
        var sounds = Jo.Value<Newtonsoft.Json.Linq.JObject>(section); //Get the specified section
        foreach (Newtonsoft.Json.Linq.JProperty pair in sounds.Properties()) //Get the key:value pairs
        {
            clip_dict[pair.Name] = Resources.Load<AudioClip>(pair.Value.ToString());
        }
        return clip_dict;
    }

    //Loads the specified section as a dictionary of strings representing filenames.
    public static Dictionary<string, string> LoadAsFilenames(string section)
    {
        TextAsset rawJson = Resources.Load<TextAsset>(json_filename);
        Newtonsoft.Json.Linq.JObject Jo = Newtonsoft.Json.Linq.JObject.Parse(rawJson.text);
        Dictionary<string, string> str_dict = new Dictionary<string, string>();
        var sounds = Jo.Value<Newtonsoft.Json.Linq.JObject>(section);
        foreach (Newtonsoft.Json.Linq.JProperty pair in sounds.Properties())
        {
            str_dict[pair.Name] = pair.Value.ToString();
        }
        return str_dict;
    }

}
