using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//After loading and processing dialogues from JsonLoader, go to dialogue manager script
//This is the script that is in charge of "Phoenix Wright Style" text dialogue

//Dialogue manager reads from a list of processed dialogue class, and plays them in type writer effect on Canvas UI
public class Dialogue_Manager : MonoBehaviour {


    // ------Inspector Input-------
    public GameObject panel;  //UI panel,which is where we should attach this script to. The reason to use a public inspector input is for potential deactivation
    public Text name; //The UI text field that is going to display who's speaking
    public Text conversation;///The UI text field that is going to display what are they saying
    public GameObject multChoicePanel;
    public GameObject PressButton;

    //animation//protrait display
    GameObject animation_display; 
    Animator anim;

    Image background;//The background image

    GameObject presentButton; //button used to present evidence
    //GameObject courtRecordButton; //button used to open court record


    public GameObject Arrow;        // The little arrow thing to tell the player next dialogue
   
    public string section_call="null";     //making it publuc for debug purposes. This is which part of the dialogue we are calling.

    //------ End of Inspector Input--------


    // Dialogue diplayers  

    public Dictionary<string, List<Type_Dialogue>> Script = 
        new Dictionary<string, List<Type_Dialogue>>(); // Prepare a Script object that's dictionary composed from JsonLoader


    int line_count = -1;                                         // a counter that keeps track of which line is playing
    int last_finished_line = -1;        //The index of the last line played and finished.
    int end_of_chapter;                                         // an end indicator to tell object stop conversation
    bool in_conversation = false;                               // determine whether the chat box disappears

    int examincation_index = -1;  //checking cross examintation sentences when pressed
    

   
    ///
    float wait_time = 0.01f; //how long per each letter
    bool done = true; // is the current dialogue donw (are we calling arrow)
    public bool is_court = false; //determine if we shut the chat box or not
    public bool can_press = false;// determine whether to hide press button
                                  ///  
    public bool is_pressing = false;


    //extra functionality variables
    public string next = "";
    public string playerPresentItem = "";
    int playerMultChoiceSelection = -1; //The player's choice when selecting one of the multiple choice options
    string playerEvidenceSelection = null; //The player's choice of evidence to present

    ///All the Audio Crap in the world. Probably will move most of the audio to a separate audio manager
    AudioClip typing;

    List<string> malevoices = new List<string>{ "???", "Miles" }; // a list of strings containing names of the male voices
                                                                                   // the type writer can then play different noises accordingly
    List<string> fremalevoices = new List<string> {};

    List<string> typesound = new List<string> {" ",",",".","?","!"};
    //Can add more List<string> for more type noises
    
    AudioSource audioSource;
    AudioClip male;
    AudioClip female;
    AudioClip writer;
    // audio objects

    AudioManager audio_manager;
    //Handles all audio except for typing sounds (would be an easy change though...)
    
    ///
    void Awake () {

        //Script
         GameObject loader = GameObject.FindGameObjectWithTag("Script_Data");  //find the xml loader that has the xml data in the scene
         Script =  loader.GetComponent<LoadJson>().sections;       //Assgin the processed dialogues in xmlloder as Script
        //panel.SetActive(false);                                    //Panel is initially in a resting state,comment out for debug purposes

        //Audio
        audioSource = GetComponent<AudioSource>();  //assgin audio source
        male= Resources.Load("TypingMale") as AudioClip;
        female = Resources.Load("TypingFemale") as AudioClip;
        writer = Resources.Load("Typewriter") as AudioClip; //Load noises

        audio_manager = GameObject.FindGameObjectWithTag("Audio_Manager").GetComponent<AudioManager>();

        //Art

        Arrow.SetActive(false);     //shut the arrow

        animation_display = GameObject.FindGameObjectWithTag("Character_Animator");
        anim = animation_display.GetComponent<Animator>();

        background = GameObject.FindGameObjectWithTag("Background").GetComponent<Image>();

        presentButton = GameObject.FindGameObjectWithTag("Present_Button");
        presentButton.SetActive(false);

        //courtRecordButton = GameObject.Find("CourtRecord");

        //Debug.Log(line_count);
        //Debug.Log(Script.Count);
    }

    private void OnEnable() //everytime when the panel shows up,reset everything.
    {
        reset_dialogue_box();
    }

    // Update is called once per frame
    void Update () {
        if (section_call != "null")//if we know what we are talking
        {
            //Debug.Log("Lince con" + line_count);
            if (line_count == -1)//if we are just starting, auto play one
            {
                //Debug.Log("Forwarding 1st line");
                line_count++;
                forward_dialogue();
            }

            end_of_chapter = Script[section_call].Count;    //Mark the end

            //dealing with evidence
            //May change to something more efficient
            //Only want the button visible during cross-examination or when the player needs to show
            //an item


            /*if (done)//if one dialogue is over
            {
                //Arrow.SetActive(true);//pop the arrow
            }*/

            if (done)
            {
                if (line_count >= end_of_chapter && done == true)   // if we hit the end
                {
                    in_conversation = false;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow) && in_conversation == false && done == true)
                {//determie if we should shut dialogue box
                 //section_call = "null";

        
                    if (!is_court)
                    {
                        if (Script[section_call][line_count - 1].next_scene != null)
                        {
                            SceneManager.LoadScene(Script[section_call][line_count - 1].next_scene);
                        }
                        reset_dialogue_box();
                        panel.SetActive(false);
                    }
                    else
                    {
                        Debug.Log(next);
                        if (next != null)
                        {
                            Debug.Log("Moving on");
                            section_call = next;
                            reset_dialogue_box();
                        }
                        else
                        {
                            line_count = 0;
                            in_conversation = true;
                        }
                    }
                }

                else if (Input.GetKeyDown(KeyCode.LeftArrow) && in_conversation && line_count > 0 && done == true)
                {
                    line_count -= 1;
                    forward_dialogue();
                }
                /*else if (Input.GetKeyDown(KeyCode.RightArrow) && in_conversation)//if commad
                {
                    //forward_dialogue();
                }*/

            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && in_conversation)//if commad
            {
                if (next != null)
                {
                    section_call = next;
                    reset_dialogue_box();
                    if (is_pressing)
                    {
                        line_count = examincation_index;
                        is_pressing = false;
                    }
                    
                }
                else
                {
                    if (line_count < Script[section_call].Count - 1)
                    {
                        if (done)
                        {
                            line_count++;
                        }
                    }
                    else
                        line_count = 0;
                }
               forward_dialogue();
                
            }

            //Perhaps there is a better place to incriment line count

        }

	}

    void reset_dialogue_box() //clear all contents
    {
        line_count = -1;
        name.text = "";
        conversation.text = "";
        animation_display = GameObject.FindGameObjectWithTag("Character_Animator");
        Vector3 animation_displayPos = animation_display.transform.localPosition;
        animation_display.transform.localPosition = new Vector3(0, animation_displayPos.y, animation_displayPos.z);
        anim = animation_display.GetComponent<Animator>();
        animation_display.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<GameObject>("Arts/" + "Characters/null/null").GetComponent<Animator>().runtimeAnimatorController; 
        animation_display.GetComponent<Image>().sprite = Resources.Load<GameObject>("Arts/" + "Characters/null/null").GetComponent<Image>().sprite;
        playerPresentItem = "";

        background = GameObject.FindGameObjectWithTag("Background").GetComponent<Image>();
        background.sprite = null;

        done = true;
        in_conversation = true;
        next = null;
    }

    void forward_dialogue() //move on to next line 
    {
        //    Debug.Log("Current Index: " + line_count + " MaxCount: " + end_of_chapter);
        if (!done)   //if we are not done, make it type faster and make sure the arrow is not on
        {
            wait_time = 0.02f;
            Arrow.SetActive(false);
        }
        else  //if we are done, set wait time back to default. 
        {
            name.text = Script[section_call][line_count].name;

            // Check if the current line can be pressed for information.

            if (Script[section_call][line_count].press != null)
            {
                can_press = true;
                PressButton.SetActive(true);
            }
            else
            {
                can_press = false;
                PressButton.SetActive(false);
            }



            if (Script[section_call][line_count].background != null)
                background.sprite = Resources.Load<Sprite>("Arts/" + "Backgrounds/" + Script[section_call][line_count].background);

            GameObject animation_prefab = Resources.Load<GameObject>("Arts/" + "Characters/" + Script[section_call][line_count].character + "/" + Script[section_call][line_count].animation);

            animation_display.GetComponent<Animator>().runtimeAnimatorController = animation_prefab.GetComponent<Animator>().runtimeAnimatorController;
            animation_display.GetComponent<Image>().sprite = animation_prefab.GetComponent<Image>().sprite;

            if (animation_display.GetComponent<Animator>() != null)
            {
                animation_display.GetComponent<Animator>().runtimeAnimatorController = animation_prefab.GetComponent<Animator>().runtimeAnimatorController;
                animation_display.GetComponent<Image>().sprite = animation_prefab.GetComponent<Image>().sprite;
            }
            
            //Moves the character to the x position specified in the JSON
            int characterXPos = Script[section_call][line_count].characterXPos;
            Vector3 animation_displayPos = animation_display.transform.localPosition;
            animation_display.transform.localPosition = new Vector3(characterXPos, animation_displayPos.y, animation_displayPos.z);

            //If the player is moving to a new piece of the conversation

            if (Script[section_call][line_count].presentable)
            {
                presentButton.SetActive(true);
                //playerPresentItem = Script[section_call][line_count].evidence;
                Debug.Log("This line can present evidence");
            }

            if (Script[section_call][line_count].next_section != null)
            {
                next = Script[section_call][line_count].next_section;
                Debug.Log("going to another line");
            }

            if (line_count > last_finished_line)
            {
                wait_time = 0.01f;

                //play the current line out
                string processing = Script[section_call][line_count].text; //make a string for the content
                                                                           // Debug.Log(Script[line_count].icon);

                StartCoroutine(PlayText(processing, conversation));//call Coroutine to type write
                Arrow.SetActive(false);//shut the arrow
            }
            else
            {
                //Otherwise displays text while skipping multiple choice, etc.
                DisplayText();
            }
        }
    }

    //Used to display a part of the text conversation without doing the typing animations, etc.
    void DisplayText()
    {

        string convo = Script[section_call][line_count].text;

        int colorCodeStart = convo.IndexOf("<color=#");
        int colorCodeEnd = convo.IndexOf(">");

        //In case of color coding
        if(colorCodeStart != -1 && colorCodeEnd != -1)
        {
            string command_end = "</color>";//(Currently hardcoded) Dealing with rich text crap
            string command_begin = ""; //^

            command_begin = convo.Substring(colorCodeStart, colorCodeEnd);
            conversation.text = command_begin + convo.Substring(colorCodeEnd) + command_end;
        }
        else
        {
            conversation.text = convo;
        }
        done = true;
    }

    IEnumerator PlayText(string story, Text conversation)  // Type Writer Coroutine (requires the string to type, and where to display)
    {
        done = false; //mark it as not done
        conversation.text = ""; //clear the current chat box
        string command_end = "</color>";//(Currently hardcoded) Dealing with rich text crap
        string command_begin = ""; //^
        bool color = false;//are we using rich text bs


        //Play/Change/Stop background music, if specified.
        if(Script[section_call][line_count].bgm != null)
        {
            string tag = Script[section_call][line_count].bgm;
            if (tag == "stop")
            {
                audio_manager.ToggleBGM(false);
            }
            else
            {
                audio_manager.ToggleBGM(true);
                audio_manager.SetBGM(tag);
            }
        }

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

        for (int i = 0; i != story.Length; i++) //for every letter in the dialogue
        {

            if (story[i] == '<' && story[i + 1] != '/')//if this is the beginning of a rich text, record the color
            {
                command_begin = "";
                command_begin += story[i];
                int count = i + 1;
                while (story[count] != '>')
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
                int count = i;
                while (story[count] != '>') //until you hit the end of the command, skip everything
                {
                    i++;
                    count++;
                }

                color = false;//not in color anymore
            }


            if (story[i] == '>')// if it hits > , skip
                continue;

            //Search for audio cues
            if (story[i] == '{')
            {
                i++;
                string command = "";
                while(story[i] != '=')
                {
                    command = command + story[i];
                    i++;
                }
                //skip '='
                i++;
                //Handle sound commands
                if(command.Trim() == "sound")
                {
                    string tag = "";
                    //Get all text up to closing brace
                    while(story[i] != '}')
                    {
                        tag += story[i];
                        i++;
                    }
                    //Isolate tag
                    char[] toTrim = { ' ', '\n', '\'' };
                    tag = tag.Trim(toTrim);
                    audio_manager.PlayOneSFX(tag);
                    //skip '}'
                    i++;
                }
            }

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
            if (current != "\n")
            {
                wait_time = 0.03f;
                if (typesound.Contains(current))
                {
                    audioSource.PlayOneShot(typing, 0.7f);
                    if (current != " ")
                        wait_time = 0.1f;
                    else
                        wait_time = 0.03f;
                }
            }

            yield return new WaitForSeconds(wait_time); //type writer


        }


        //Throws up the multiple choice panel if there is a question
        if (Script[section_call][line_count].multipleChoice != null)
        {
            //Debug.Log("Line count" + line_count);
            /*foreach (string s in Script[section_call][line_count].multipleChoice)
                Debug.Log(s);*/
            multChoicePanel.GetComponent<MultChoicePanelManager>().DisplayChoices(Script[section_call][line_count].multipleChoice);
            multChoicePanel.SetActive(true);
            //Need to select correct button arrangement based on number of choices

            //Stops player from progressing until they choose the correct option
            playerMultChoiceSelection = -1;
            //Debug.Log("Correct choice " + Script[section_call][line_count].correctChoice);
            while (true)
            {
                yield return new WaitUntil(() => playerMultChoiceSelection >= 0);
                if (playerMultChoiceSelection == Script[section_call][line_count].correctChoice)
                {
                    break;
                }
                else
                {
                    //else, there is a penalty. Greys out the player's incorrect guess
                    multChoicePanel.GetComponent<MultChoicePanelManager>().DisableIncorrectGuess(playerMultChoiceSelection, Script[section_call][line_count].multipleChoice.Length);
                    playerMultChoiceSelection = -1;
                }
            }
            multChoicePanel.GetComponent<MultChoicePanelManager>().ResetChoices();
            multChoicePanel.SetActive(false);
        }

        if (Script[section_call][line_count].presentable)
        {
            Debug.Log("EVIDENCE " + Script[section_call][line_count].evidence);
            presentButton.SetActive(true);
            //GameObject.FindGameObjectWithTag("Present_Button").GetComponent<CanvasGroup>().alpha = 1;
            //GameObject.FindGameObjectWithTag("Present_Button").GetComponent<CanvasGroup>().interactable = true;
            //GameObject.FindGameObjectWithTag("Present_Button").GetComponent<CanvasGroup>().blocksRaycasts = true;
            //Debug.Log(Script[section_call][line_count].evidence);
            playerEvidenceSelection = "";
            while (true)
            {
                //Debug.Log("Awaiting player evidence selection");

                yield return new WaitUntil(() => !playerEvidenceSelection.Equals(""));
                //Debug.Log("playerEvidence selection " + playerEvidenceSelection + playerEvidenceSelection.Length);
                //Debug.Log("Evidence " + Script[section_call][line_count].evidence + Script[section_call][line_count].evidence.Length);
                //Debug.Log(playerEvidenceSelection.Equals(Script[section_call][line_count].evidence));
                if (playerEvidenceSelection.Equals(Script[section_call][line_count].evidence))
                {
                    //Automatically moves to next line, if there is one.
                    if(line_count + 1 < end_of_chapter)
                    {
                        presentButton.SetActive(false);
                        done = true;
                        line_count += 1;
                        forward_dialogue();
                        yield break;
                    }
                    break;
                }
                else
                {
                    //else, there is a penalty.
                    conversation.text = "WTF IS THIS!!!";
                    playerEvidenceSelection = "";
                    Arrow.SetActive(true);
                    presentButton.SetActive(false);
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.RightArrow));
                    DisplayText();
                    Arrow.SetActive(false);
                    presentButton.SetActive(true);
                }
            }

            //GameObject.FindGameObjectWithTag("Present_Button").GetComponent<CanvasGroup>().alpha = 0;
            //GameObject.FindGameObjectWithTag("Present_Button").GetComponent<CanvasGroup>().interactable = false;
            //GameObject.FindGameObjectWithTag("Present_Button").GetComponent<CanvasGroup>().blocksRaycasts = false;
            presentButton.SetActive(false);
        }

        //GameObject.FindGameObjectWithTag("Present_Button").SetActive(false);
        //line_count++;
        done = true; // when we are done, mark done as true
        Arrow.SetActive(true);//pop the arrow

        last_finished_line = line_count;
    }

    // Set the next section to be the press response section.
    public void PressSection()
    {
        //Needed to prevent crashes if the press button is clicked before text finishes.
        if (done)
        {
            is_pressing = true;
            examincation_index = line_count;
            section_call = Script[section_call][line_count].press;
            reset_dialogue_box();
        }
    }

    public void PresentEvidence(string presenting)
    {
        //Debug.Log("Presenting" + presenting);
        //Debug.Log("Player present item" + playerPresentItem);
        audio_manager.PlayOneSFX("objection");
        /*if (presenting == playerPresentItem)
        {
            section_call = section_call + "Right";

        }
        else
            section_call = section_call + "Wrong";*/
        //Print negative statement.

        playerEvidenceSelection = presenting;
        //reset_dialogue_box();
    }
    //Called by multiple choice buttons
    public void setPlayerMultChoiceSelection(int sel)
    {
        //Debug.Log("Player choice " + sel);
        playerMultChoiceSelection = sel;
        //Debug.Log(playerMultChoiceSelection);
    }

    //Can be called by a piece of evidence when the player clicks it
    public void setPlayerEvidenceSelection(string sel)
    {
        playerEvidenceSelection = sel;
    }
}
