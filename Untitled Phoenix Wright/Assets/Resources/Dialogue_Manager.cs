using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//Dialogue manager reads from a list of processed dialogue class, and plays them in type writer effect on Canvas UI
public class Dialogue_Manager : MonoBehaviour {


    // Inspector Input-------
    public GameObject panel;  //this UI panel
    public Text name; //The UI text field that is going to display who's speaking
    public Text conversation;///The UI text field that is going to display what are they saying

    
    public GameObject ImagePosition;// where the character face/animation located. 
                                    // Using ImagePosition because we need this position to call image from resources
                             
    private RawImage image;         // The Image object after we load ImagePosition from resrouce file


    public GameObject Arrow;        // The little arrow thing to tell the player next dialogue
   
    public string section_call="null";     //for debug purposes, this is which part of the dialogue we are calling

    // End of Inspector Input--------


    // dialogue play 

    public Dictionary<string, List<Type_Dialogue>> Script = 
        new Dictionary<string, List<Type_Dialogue>>();          // Prepare a Script object that's a dictionary
    int line_count = 0;                                         // a counter that keeps track of which line is playing
    int end_of_chapter;                                         // an end indicator to tell object stop conversation
    bool in_conversation = false;                               // determine whether the chat box disappears


   
    ///
    float wait_time = 0.1f; //how long per each letter
    bool done = true; // is the current dialogue donw (are we calling arrow)
    ///  
  

    ///All the Audio Crap in the world
    AudioClip typing;

    List<string> malevoices = new List<string>{ "???", "老师", "米祖", "裁判长" }; // a list of strings containing names of the male voices
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
         Script =  loader.GetComponent<LoadXml>().sections;       //Assgin the processed dialogues in xmlloder as Script
        //panel.SetActive(false);                                    //Panel is initially in a resting state

        //Audio
        audioSource = GetComponent<AudioSource>();  //assgin audio source
        male= Resources.Load("TypingMale") as AudioClip;
        female = Resources.Load("TypingFemale") as AudioClip;
        writer = Resources.Load("Typewriter") as AudioClip; //Load noises

        //Art
        image = (RawImage)ImagePosition.GetComponent<RawImage>();       // get the raw image object in imageposition
        image.texture = (Texture)Resources.Load("Arts/null");  //and assign the image to null (a blank png)
        Arrow.SetActive(false);     //shut the arrow
    }

    private void OnEnable() //everytime when the panel shows up,reset everything.
    {
        reset_dialogue_box();
    }

    // Update is called once per frame
    void Update () {
        
        if (section_call != "null")//if we know what we are talking
        {

            Debug.Log(section_call);
            
            end_of_chapter = Script[section_call].Count;    //Mark the end as 1+ the size of script

            if (done)//if one dialogue is over
            {
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

            else if (Input.GetMouseButtonDown(0) &&in_conversation)//if mouse click
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
                    image.texture = (Texture)Resources.Load("Arts/" + Script[section_call][line_count].icon);//load icon
                    StartCoroutine(PlayText(processing, conversation));//call Coroutine to type write
                    Arrow.SetActive(false);//shut the arrow
                    line_count++;//prepare for the next

                }


             

                //Debug.Log(line_count);
                //Debug.Log(end_of_chapter);
            }
        }

	}

    void reset_dialogue_box()
    {
        line_count = 0;
        name.text = "";
        conversation.text = "";
        image = (RawImage)ImagePosition.GetComponent<RawImage>();
        image.texture = (Texture)Resources.Load("null");
        done = true;
        in_conversation = true;
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
