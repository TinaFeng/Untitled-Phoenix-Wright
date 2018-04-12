using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Converters;
//[System.Serializable]
public class Type_Dialogue // Type_Dialogue stores information for each individual dialogues.
                            // Such as who's speaking(name), what do they look like(icon), and what are they saying(text)
                            //class "Objects" is the orginal type dialogue that contains basic information
{
        public string name;
        public string character;
        public string animation;
        public string text;

}


public class LoadJson : MonoBehaviour{

    private string Dialogue_File_Name;//json path, ideally should be the scene name

    public List<Type_Dialogue> dialogues = new List<Type_Dialogue>(); // make a list of dialogue types for future use
    public Dictionary<string, List<Type_Dialogue>> sections = new Dictionary<string, List<Type_Dialogue>>(); // a dictionary
    //public List<Type_Dialogue1> dialogues = new List<Type_Dialogue1>(); // make a list of dialogue types for future use
    //public Dictionary<string, List<Type_Dialogue1>> sections = new Dictionary<string, List<Type_Dialogue1>>(); // a dictionary

    // Use this for initialization
    void Start () {
        LoadDialogueData();  // When every scene starts, load json data
    }

    public void LoadDialogueData()  // readoing the XML, split it into a list of informations per dialogue
    {


            Dialogue_File_Name = SceneManager.GetActiveScene().name; // getting the file name from current scene


            TextAsset rawJson = Resources.Load<TextAsset>(Dialogue_File_Name);
           
            Newtonsoft.Json.Linq.JObject Jo = Newtonsoft.Json.Linq.JObject.Parse(rawJson.text);
    
            
            string section_name = "null";                                // name used to link dictionary
                                                                         //Debug.Log((Jo["Scene"]).GetType());

            foreach (var scene in Jo["Scene"]) //each scene here is a key for dictionary
        {
            dialogues = new List<Type_Dialogue>();


            foreach (var conversation in scene) //split the scene with scene name and the dialogue arrays
            {
                //Also I give up on this part, assume the one that's not an array, is the key for dictionary  

               
        
                if(conversation.First.Type.ToString() == "String")//if the value is a string, make it the key
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
                        dialogues.Add(line);
                    }

                }

                
            }
            sections.Add(section_name, dialogues);
        }
            

        }
    }
