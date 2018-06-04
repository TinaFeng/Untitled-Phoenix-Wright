using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Converters;
using System;
//using Newtonsoft.Json;
//using System.ComponentModel;

// This is a loder script that loads the desginated story scripting by its scene.

// How to use: Attach this script on a gameobject(can be script loader, or anything in this scene)
// and make sure to name the JSON file same as the scene name



// Type_Dialogue is a class that stores information for each individual dialogues
// such as who's speaking(name), what do they look like(icon), and what are they saying(text)
// can add more variables

public class Type_Dialogue
{
    //[DefaultValue("John Doe")]
    //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public string name;

        public string character;
        public string animation;
    public int characterXPos;
        public string text;
    public string[] multipleChoice;
    public int correctChoice;
    public bool presentable;
    public string evidence;
    public string obtainEvidence; //Evidence the player will obtain completing dialogue
    public string next_section = null;
    public string bgm;
    public string press = null;
    public string next_scene;
        //public Dictionary <string, string[]> extra = new Dictionary <string, string[]>();
    //public string[] extra;


    public string background; //The background image
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

        Debug.Log(Dialogue_File_Name);

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

                        try
                        {
                            line.name = item["name"].ToString();
                        }
                        catch(Exception e)
                        {
                            line.name = null;
                        }

                        line.character = item["character"].ToString();

                        //Adds in the character's X position.  Defaults to 0
                        try
                        {
                            line.characterXPos = item["characterXPos"].ToObject<int>();
                        }
                        catch (Exception e)
                        {
                            line.characterXPos = 0;
                        }

                        line.animation = item["animation"].ToString();
                        line.text = item["text"].ToString();

                        //Adds in the multiple choices
                        try
                        {
                            if (item["multipleChoice"].Count() != 0)
                            {
                                line.multipleChoice = new string[item["multipleChoice"].Count()];
                                int index = 0;
                                foreach (string choice in item["multipleChoice"])
                                {
                                    line.multipleChoice[index] = choice;
                                    index++;
                                }
                                if (line.multipleChoice.Count() == 1 || line.multipleChoice.Count() > 4)
                                {
                                    Debug.Log("There are " + line.multipleChoice.Count() + " in the mulitple choice Array. 2-4 please.");
                                }
                            }
                        }
                        catch(Exception e)
                        {
                            line.multipleChoice = null;
                        }

                        //Adds in the correct choice.  Some checks since tied to multipleChoice
                        try
                        {
                            line.correctChoice = item["correctChoice"].ToObject<int>();
                            //Debug.Log("When read in..." + line.correctChoice);
                            if (line.multipleChoice == null)
                            {
                                Debug.Log("No multiple choices");
                            }
                        }
                        catch (Exception e)
                        {
                            line.correctChoice = 0;
                        }

                        //presentable option
                        try
                        {
                            if (item["presentable"].ToString() == "1")
                                line.presentable = true;
                            else
                                line.presentable = false;
                        }
                        catch (Exception e)
                        {
                            line.presentable = false;
                        }
                        //evidence answer to present
                        try
                        {
                            if (item["evidence"].ToString() !="")
                            line.evidence = item["evidence"].ToString();
                        }
                        catch (Exception e)
                        {
                            line.evidence = null;
                        }

                        try
                        {
                            if (item["obtainEvidence"].ToString() != "")
                                line.obtainEvidence = item["obtainEvidence"].ToString();
                        }
                        catch (Exception e)
                        {
                            line.obtainEvidence = null;
                        }

                        //section to call if pressed
                        try
                        {
                            line.press = item["press"].ToString();
                        }
                        catch (Exception e)
                        {
                            line.press = null;
                        }
                        //if we have a next section variable
                        try
                        {
                            if (item["nextsection"].ToString() != "")
                                line.next_section = item["nextsection"].ToString();
                        }
                        catch (Exception e)
                        {
                            line.next_section = null;
                        }

                        //if we have next scene
                        try
                        {
                            if (item["nextscene"].ToString() != "")
                                line.next_scene = item["nextscene"].ToString();
                            Debug.Log(item["nextscene"].ToString());
                        }
                        catch (Exception e)
                        {
                            line.next_scene = null;
                        }



                        //Gets tag for background music, if any.
                        try
                        {
                            if (item["bgm"].ToString() != "")
                                line.bgm = item["bgm"].ToString();
                        }
                        catch (Exception e)
                        {
                            line.bgm = null;
                        }

                        try
                        {
                            if (item["background"].ToString() != "")
                                line.background = item["background"].ToString();
       
                            //Debug.Log(line.evidence);
                        }
                        catch (Exception e)
                        {
                            line.background = null;
                        }
                        dialogues.Add(line); //add this line to list
                    }

                }

                
            }
            sections.Add(section_name, dialogues);// add list into a dictionary
        }
            

        }
    }
