using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//After loading and processing dialogues from JsonLoader, go to dialogue manager script
//This is the script that is in charge of "Phoenix Wright Style" text dialogue

//Dialogue manager reads from a list of processed dialogue class, and plays them in type writer effect on Canvas UI
public class Dialogue_Manager : MonoBehaviour {


    // ------Inspector Input-------
    public GameObject panel;  //UI panel,which is where we should attach this script to. The reason to use a public inspector input is for potential deactivation
    public Text name; //The UI text field that is going to display who's speaking
    public Text conversation;///The UI text field that is going to display what are they saying

    

    //animation//protrait display
    GameObject animation_display; 
    Animator anim;


    public GameObject Arrow;        // The little arrow thing to tell the player next dialogue
   
    public string section_call="null";     //making it publuc for debug purposes. This is which part of the dialogue we are calling.

    //------ End of Inspector Input--------


    // Dialogue diplayers  

    public Dictionary<string, List<Type_Dialogue>> Script = 
        new Dictionary<string, List<Type_Dialogue>>(); // Prepare a Script object that's dictionary composed from JsonLoader


    int line_count = 0;                                         // a counter that keeps track of which line is playing
    int end_of_chapter;                                         // an end indicator to tell object stop conversation
    bool in_conversation = false;                               // determine whether the chat box disappears
   

   
    ///
    float wait_time = 0.1f; //how long per each letter
    bool done = true; // is the current dialogue donw (are we calling arrow)
    ///  
  

    ///All the Audio Crap in the world. Probably will move most of the audio to a separate audio manager
    AudioClip typing;

    List<string> malevoices = new List<string>{ "???", "Miles" }; // a list of strings containing names of the male voices
                                                                                   // the type writer can then play different noises accordingly
    List<string> fremalevoices = new List<string> {};
    
    //Can add more List<string> for more type noises

    AudioSource audioSource;
    AudioClip male;
    AudioClip female;
    AudioClip writer;
    // audio objects
    ///
    void Start () {

        //Script
         GameObject loader = GameObject.FindGameObjectWithTag("Script_Data");  //find the xml loader that has the xml data in the scene
         Script =  loader.GetComponent<LoadJson>().sections;       //Assgin the processed dialogues in xmlloder as Script
        //panel.SetActive(false);                                    //Panel is initially in a resting state,comment out for debug purposes

        //Audio
        audioSource = GetComponent<AudioSource>();  //assgin audio source
        male= Resources.Load("TypingMale") as AudioClip;
        female = Resources.Load("TypingFemale") as AudioClip;
        writer = Resources.Load("Typewriter") as AudioClip; //Load noises

        //Art

        Arrow.SetActive(false);     //shut the arrow

        animation_display = GameObject.FindGameObjectWithTag("Character_Animator");
        anim = animation_display.GetComponent<Animator>();

    }

    private void OnEnable() //everytime when the panel shows up,reset everything.
    {
        reset_dialogue_box();
    }

    // Update is called once per frame
    void Update () {
        
        if (section_call != "null")//if we know what we are talking
        {

        //    Debug.Log(section_call);
            
            end_of_chapter = Script[section_call].Count;    //Mark the end

            //Kaitlyn - Maybe this belongs at the end of foward dialogue
            if (done)//if one dialogue is over
            {
                //If there is a multiple choice question, bring up the choices instead of the arrow.
                //TESTING
                //Debug.Log(line_count);
                /*if (Script[section_call][line_count].extra.ContainsKey("Multiple Choice"))
                {
                    Debug.Log("Multiple Choice");
                }*/
                Arrow.SetActive(true);//pop the arrow
            }
            if (line_count >= end_of_chapter && done == true)   // if we hit the end
            {
                in_conversation = false;
            }
            if (Input.GetMouseButtonDown(0) && in_conversation == false && done == true)
            {
                //section_call = "null";
                panel.SetActive(false);
                reset_dialogue_box();
               
            }
            else if (line_count ==0)//if we are just starting, auto play one
            {
                forward_dialogue();
            }
            
            else if (Input.GetMouseButtonDown(0) &&in_conversation)//if mouse click
            {
                forward_dialogue();
            }
        }

	}

    void reset_dialogue_box() //clear all contents
    {
        line_count = 0;
        name.text = "";
        conversation.text = "";
        animation_display = GameObject.FindGameObjectWithTag("Character_Animator");
        anim = animation_display.GetComponent<Animator>();
        animation_display.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<GameObject>("Arts/" + "Characters/null/null").GetComponent<Animator>().runtimeAnimatorController; 
        animation_display.GetComponent<Image>().sprite = Resources.Load<GameObject>("Arts/" + "Characters/null/null").GetComponent<Image>().sprite;

        done = true;
        in_conversation = true;
    }

    void forward_dialogue() //move on to next line 
    {
      
        //    Debug.Log("Current Index: " + line_count + " MaxCount: " + end_of_chapter);
        if (!done)   //if we are not done, make it type faster and make sure the arrow is not on
        {
            wait_time = 0.04f;
            Arrow.SetActive(false);
        }
        if (done) //if we are done, set wait time back to default. 
        {
            wait_time = 0.1f;
            name.text = Script[section_call][line_count].name;
            //play the current line out
            string processing = Script[section_call][line_count].text; //make a string for the content
                                                                       // Debug.Log(Script[line_count].icon);

            
            
            GameObject animation_prefab = Resources.Load<GameObject>("Arts/" + "Characters/" + Script[section_call][line_count].character+ "/" + Script[section_call][line_count].animation);

            
//            Debug.Log(("Arts/" + "Characters/" + Script[section_call][line_count].character + "/" + Script[section_call][line_count].animation));

            
            animation_display.GetComponent<Animator>().runtimeAnimatorController = animation_prefab.GetComponent<Animator>().runtimeAnimatorController;
            animation_display.GetComponent<Image>().sprite = animation_prefab.GetComponent<Image>().sprite;

            StartCoroutine(PlayText(processing, conversation));//call Coroutine to type write
            Arrow.SetActive(false);//shut the arrow

            //TESTING: If there is a multiple choice question, the panel is brought up
            //Debug.Log("Key count in " + line_count + " " + Script[section_call][line_count].extra.Keys.Count);
            if (Script[section_call][line_count].extra.ContainsKey("Multiple Choice"))
            {
                Debug.Log("Multiple Choice");
                //Put up multiple choice until the player makes a guess
            }
            //TESTING
            line_count++;//prepare for the next
        }

    }

    IEnumerator PlayText(string story,Text conversation)  // Type Writer Coroutine (requires the string to type, and where to display)
    {
        done = false; //mark it as not done
        conversation.text = ""; //clear the current chat box
        string command_end = "</color>";//(Currently hardcoded) Dealing with rich text crap
        string command_begin = ""; //^
        bool color = false;//are we using rich text bs


        //determine which sound is beeping, can add more if statements
        if (malevoices.Contains(Script[section_call][line_count].name))
        {
            typing = male;
        }
        else if (Script[section_call][line_count].name == " ")
        {
            typing = writer;
        }
        else
        {
            typing = female;
        }
    

        string current; // current is the string we are about to type out

        for (int i =0; i!= story.Length;i++) //for every letter in the dialogue
        {
            
            if(story[i] == '<' && story[i+1] != '/')//if this is the beginning of a rich text, record the color
            { 
                command_begin = "";
                command_begin += story[i];
                int count = i + 1;
                while(story[count]!='>')
                {
                    command_begin += story[count];
                    count++;
                }//getting the color code

                command_begin += story[count];
                color = true; //begin coloring returns
                i = count + 1;
            }

            if (story[i] == '<' && story[i + 1] == '/') //if it is a </  sign
            {
                i++; 
                int count =i;
                while (story[count] !='>') //until you hit the end of the command, skip everything
                {
                    i++;
                    count++;
                }
                
                color = false;//not in color anymore
            }


            if (story[i] == '>')// if it hits > , skip
                continue;

            current = story[i].ToString();
            if (color == true)  // if we are still in color mode, add color
             {
                string add = command_begin + story[i] + command_end;
                conversation.text += add;
          
             }
            else//if not, just add the text to yield string
             {
                conversation.text += story[i];
                
                
               }


            //Audio Management
            if(current!= "\n")
            {
                audioSource.PlayOneShot(typing, 0.7f);
            }
           
            yield return new WaitForSeconds(wait_time); //type writer


        }

        done = true; // when we are done, mark done as true
    }
}
