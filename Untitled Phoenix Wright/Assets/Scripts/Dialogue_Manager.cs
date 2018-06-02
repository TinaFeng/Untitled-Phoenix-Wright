using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//After loading and processing dialogues from JsonLoader, go to dialogue manager script
//This is the script that is in charge of "Phoenix Wright Style" text dialogue

//Dialogue manager reads from a list of processed dialogue class, and plays them in type writer effect on Canvas UI
public class Dialogue_Manager : MonoBehaviour
{


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

    public string section_call = "null";     //making it publuc for debug purposes. This is which part of the dialogue we are calling.

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
    bool fast_forward = false;
    bool done = true; // is the current dialogue donw (are we calling arrow)
    public bool is_investigation; //determine if we should leave dialogue box on or off
    public bool can_press = false;// determine whether to hide press button
                                  ///  
    public bool is_pressing = false;
    public bool speedrun = false;

    //extra functionality variables
    public string next = "";
    public string playerPresentItem = "";
    int playerMultChoiceSelection = -1; //The player's choice when selecting one of the multiple choice options
    string playerEvidenceSelection = null; //The player's choice of evidence to present

    ///All the Audio Crap in the world. Probably will move most of the audio to a separate audio manager
    AudioClip typing;

    List<string> malevoices = new List<string> { "???", "Miles" }; // a list of strings containing names of the male voices
                                                                   // the type writer can then play different noises accordingly
    List<string> fremalevoices = new List<string> { };

    List<string> typesound = new List<string> { " ", ",", ".", "?", "!" };
    //Can add more List<string> for more type noises

    AudioSource audioSource;
    AudioClip male;
    AudioClip female;
    AudioClip writer;
    // audio objects

    AudioManager audio_manager;
    //Handles all audio except for typing sounds (would be an easy change though...)

    ///
    void Awake()
    {

        //Script
        GameObject loader = GameObject.FindGameObjectWithTag("Script_Data");  //find the xml loader that has the xml data in the scene
        Script = loader.GetComponent<LoadJson>().sections;       //Assgin the processed dialogues in xmlloder as Script
        //panel.SetActive(false);                                    //Panel is initially in a resting state,comment out for debug purposes

        //Audio
        audioSource = GetComponent<AudioSource>();  //assgin audio source
        male = Resources.Load("TypingMale") as AudioClip;
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

    }

    private void OnEnable() //everytime when the panel shows up,reset everything.
    {
        reset_dialogue_box();
    }

    ///Using Update to keep track of line counts and pressing keys

    void Update()
    {
        if (section_call != "null")//if we know what we are talking
        {
            if (line_count == -1)//if we are just starting, auto play one
            {
                line_count++;
                forward_dialogue();
            }

            if (line_count >= Script[section_call].Count - 1)   // if we hit the end
                {

                    in_conversation = false;    //in_conversation determines if the current sections is over or not
                }


            if (Input.GetKeyDown(KeyCode.RightArrow) && !done)
            {
       
                fast_forward = true;
                Arrow.SetActive(false);
            }

            else if (Input.GetKeyDown(KeyCode.RightArrow) && in_conversation == false && done) //end of section handling
            {

                if (is_investigation) //investigation determines whether the current arc is investigation or not. If it is , we turn off chat box after interaction.
                {
                    reset_dialogue_box();
                    panel.SetActive(false);
                }
                else//
                {
                    if (next != null)
                    {
                        Debug.Log("Next section at switching back");
                        section_call = next;
                        reset_dialogue_box();
                        

                    }
                    else if (Script[section_call][line_count].next_scene != null)
                    {
                        SceneManager.LoadScene(Script[section_call][line_count].next_scene);
                    }
                    else
                    {
                        //if not moving on, roll back
                        Debug.Log("Roll back at conversation == false 165");
                        line_count = 0;
                        in_conversation = true;
                    }
                }

            }

            ///-------------------------------Dialogue Controls---------------------------------------------
            ///--------------------------------------------------------Right Arrow---------------------------------------------
            ///
            else if (Input.GetKeyDown(KeyCode.RightArrow) && in_conversation)
            {
   
                    if (next != null)
                    {

                        section_call = next;
                        reset_dialogue_box();

                    }
                    else
                    {
                        if (line_count < Script[section_call].Count - 1)
                        {
                            line_count++;
                        }
                        else
                        {
                            Debug.Log("Roll back at 197 size out of bound");
                            line_count = 0; // roll back
                        }
                    }
                    forward_dialogue();
                
            }



            ///--------------------------------------------------------Left Arrow---------------------------------------------
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && in_conversation && line_count > 0 && done == true)
            {
                line_count -= 1;
                forward_dialogue();
            }


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
        if (is_pressing)  // used handle transition from press statements to normal dialogue
        {
            is_pressing = false;
            
            line_count = examincation_index;
            if (line_count != -1)
                forward_dialogue();
            examincation_index = 0;
            
        }
    }

    void forward_dialogue() //move on to next line 
    {


        
            name.text = Script[section_call][line_count].name;

            _PressCheck();

            _PresentCheck();

            _LoadBackground();

            _AnimationHandler();

            _CharacterPositionSwitch();

            _PresentCheck();

            _NextScetionSaver();

            _RunDialogue();

            _BgmHandler();
        
    }



    IEnumerator PlayText(string story, Text conversation)  // Type Writer Coroutine (requires the string to type, and where to display)
    {
        done = false; //mark it as not done
        conversation.text = ""; //clear the current chat box
        string command_end = "</color>";//(Currently hardcoded) Dealing with rich text crap
        string command_begin = ""; //^
        bool color = false;//are we using rich text bs
        _AnimationTalk();

        _BgmHandler();

        _SetTypeSound();


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
                while (story[i] != '=')
                {
                    command = command + story[i];
                    i++;
                }
                //skip '='
                i++;
                //Handle sound commands
                if (command.Trim() == "sound")
                {
                    string tag = "";
                    //Get all text up to closing brace
                    while (story[i] != '}')
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
            if (fast_forward)
            {
                yield return new WaitForSeconds(0.01f);
            }
            else
            yield return new WaitForSeconds(wait_time); //type writer


        }


        //Throws up the multiple choice panel if there is a question
        //Multiple Choice Handling--------------------------
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

        //Presetable Handling-------------------------


        //finishing up handling

        done = true; // when we are done, mark done as true
        fast_forward = false;
        Arrow.SetActive(true);//pop the arrow

        _AnimationTalk();
        last_finished_line = line_count;
    }

    // Set the next section to be the press response section.
    public void PressSection()
    {
        //Needed to prevent crashes if the press button is clicked before text finishes.
        if (done)
        {
            examincation_index = line_count;
            section_call = Script[section_call][line_count].press;
            reset_dialogue_box();
            is_pressing = true;
        }
    }



    public void PresentEvidence(string presenting)
    {
        if (done)
        {
            audio_manager.PlayOneSFX("objection");

            Debug.Log("Presenting: " + presenting);

            if (presenting.Equals(Script[section_call][line_count].evidence))
                    {
                        //Automatically moves to next line, if there is one.
                        conversation.text = "Fine";
                        done = true;
                        Arrow.SetActive(true);
                        presentButton.SetActive(true);
                }
                  
                    
             else
                    {
                        //else, there is a penalty.
                        conversation.text = "WTF IS THIS!!!";
                        done = true;
                        Arrow.SetActive(true);
                        presentButton.SetActive(true);
                    }
                }
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




    //---------Helper Functions---------------//

    private void _PressCheck()
    {
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
    }


    private void _LoadBackground() //Background Switching Helper
    {
        if (Script[section_call][line_count].background != null)
            background.sprite = Resources.Load<Sprite>("Arts/" + "Backgrounds/" + Script[section_call][line_count].background);

    }


    private void _AnimationHandler() //switches animation
    {
        GameObject animation_prefab = Resources.Load<GameObject>("Arts/" + "Characters/" + Script[section_call][line_count].character + "/" + Script[section_call][line_count].animation);

        if (animation_display.GetComponent<Animator>() != null)
        {
            animation_display.GetComponent<Animator>().runtimeAnimatorController = animation_prefab.GetComponent<Animator>().runtimeAnimatorController;
            animation_display.GetComponent<Image>().sprite = animation_prefab.GetComponent<Image>().sprite;
        }
    }


    private void _CharacterPositionSwitch() // move position accordingly
    {
        //Moves the character to the x position specified in the JSON
        int characterXPos = Script[section_call][line_count].characterXPos;
        Vector3 animation_displayPos = animation_display.transform.localPosition;
        animation_display.transform.localPosition = new Vector3(characterXPos, animation_displayPos.y, animation_displayPos.z);
    }


    private void _PresentCheck()
    {
        if (Script[section_call][line_count].presentable)
        {
            presentButton.SetActive(true);
        }
        else
        {
            presentButton.SetActive(false);
        }
    } //check if we can present evidence


    private void _NextScetionSaver() //save the next section name for next iteration
    {
        if (Script[section_call][line_count].next_section != null)
        {
            next = Script[section_call][line_count].next_section;
        }
    }


    private void _RunDialogue() //type writer or speed run.
    {

        if (speedrun)
            _DisplayText();
        else
        { 
            //play the current line out
            string processing = Script[section_call][line_count].text; //make a string for the content
                                                                       // Debug.Log(Script[line_count].icon);

            StartCoroutine(PlayText(processing, conversation));//call Coroutine to type write
            Arrow.SetActive(false);//shut the arrow
        }

    }


    private void _DisplayText()
    {

        string convo = Script[section_call][line_count].text;

        int colorCodeStart = convo.IndexOf("<color=#");
        int colorCodeEnd = convo.IndexOf(">");

        //In case of color coding
        if (colorCodeStart != -1 && colorCodeEnd != -1)
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
        Arrow.SetActive(true);
    } //speed run


    private void _BgmHandler()//BGM Handling
    {
        //Play/Change/Stop background music, if specified.
        if (Script[section_call][line_count].bgm != null)
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
    }


    //Coroutine Helper
    private void _SetTypeSound()
    {

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


    } //set typing noise

    private void _AnimationTalk()
    {
  
        GameObject animation_prefab = Resources.Load<GameObject>("Arts/" + "Characters/" + Script[section_call][line_count].character + "/" + Script[section_call][line_count].animation);

        if (animation_display.GetComponent<Animator>() != null)
        {
            if (!done)
                animation_display.GetComponent<Animator>().SetBool("Talking", true);
            else
                animation_display.GetComponent<Animator>().SetBool("Talking",false);
        }
    }
}
