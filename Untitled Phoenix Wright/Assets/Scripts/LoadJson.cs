using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Converters;

// This is a loder script that loads the desginated story scripting by its scene.

// How to use: Attach this script on a gameobject(can be script loader, or anything in this scene)
// and make sure to name the JSON file same as the scene name



// Type_Dialogue is a class that stores information for each individual dialogues
// such as who's speaking(name), what do they look like(icon), and what are they saying(text)
// can add more variables

public class Type_Dialogue
{ 
        public string name;
        public string character;
        public string animation;
        public string text;
        public Dictionary <string, string[]> extra = new Dictionary <string, string[]>();
    //public string[] extra;
}


public class LoadJson : MonoBehaviour{

    private string Dialogue_File_Name;//json path, ideally should be the scene name

    public List<Type_Dialogue> dialogues = new List<Type_Dialogue>(); // make a list of dialogue types for conversations
    public Dictionary<string, List<Type_Dialogue>> sections = new Dictionary<string, List<Type_Dialogue>>(); 
    //a dictionary that divides the script into sections

    // Use this for initialization
    void Start () {
        LoadDialogueData();  // When every scene starts, load json data
    }

    public void LoadDialogueData()  // readoing the XML, split it into a list of informations per dialogue
    {


            Dialogue_File_Name = SceneManager.GetActiveScene().name; // getting the file name from current scene


            TextAsset rawJson = Resources.Load<TextAsset>(Dialogue_File_Name); //Load the JSON file by its name
           
            Newtonsoft.Json.Linq.JObject Jo = Newtonsoft.Json.Linq.JObject.Parse(rawJson.text); // Parse the JSON file into key value JSON pairs
    
            
            string section_name = "null";   //storing a string for dictionary key
                                                                         //Debug.Log((Jo["Scene"]).GetType());

            foreach (var scene in Jo["Scene"]) //each scene here is a key for dictionary
        {
            dialogues = new List<Type_Dialogue>(); //create a new list for diectionary values


            foreach (var conversation in scene) //split the scene with scene name and the dialogue arrays
            {
                //Also I give up on this part, assume the one that's not an array, is the key for dictionary  
                // can also do indexes 
               
        
                if (conversation.First.Type.ToString() == "String")//if the value is a string, make it the key
                {
                    section_name = conversation.First.ToString();
                 
                }
                else if (conversation.First.Type.ToString() == "Array")
                {

                    foreach ( var item in conversation.First)//breaking down to each array element
                    {
                       Type_Dialogue line = new Type_Dialogue();
                       //  Debug.Log(item);
                        line.name = item["name"].ToString();
                        line.character = item["character"].ToString();
                        line.animation = item["animation"].ToString();
                        line.text = item["text"].ToString();
                        //line.extra = item["extra"].ToString();
                        dialogues.Add(line); //add this line to list
                    }

                }

                
            }
            sections.Add(section_name, dialogues);// add list into a dictionary
        }
            

        }
    }
