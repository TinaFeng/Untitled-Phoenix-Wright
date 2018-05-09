using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultChoicePanelManager : MonoBehaviour {

    public GameObject twoChoices;
    public GameObject threeChoices;
    public GameObject fourChoices;

    // Use this for initialization
    void Start ()
    {
        //Maybe make arrays of button gameobjects?
    }
	
    //Displays the correct number of choices and changes the text on the buttons
    public void DisplayChoices(string[] choices)
    {
        if (choices.Length == 2)
        {
            twoChoices.transform.Find("Choice1").gameObject.GetComponentInChildren<Text>().text = choices[0];
            twoChoices.transform.Find("Choice1").gameObject.GetComponent<Button>().interactable = true;
            twoChoices.transform.Find("Choice2").gameObject.GetComponentInChildren<Text>().text = choices[1];
            twoChoices.transform.Find("Choice2").gameObject.GetComponent<Button>().interactable = true;
            twoChoices.SetActive(true);
        }
        else if (choices.Length == 3)
        {
            //Debug.Log(choices[0] + " " + choices[1] + " " + choices[2]);
            threeChoices.transform.Find("Choice1").gameObject.GetComponentInChildren<Text>().text = choices[0];
            threeChoices.transform.Find("Choice1").gameObject.GetComponent<Button>().interactable = true;
            threeChoices.transform.Find("Choice2").gameObject.GetComponentInChildren<Text>().text = choices[1];
            threeChoices.transform.Find("Choice2").gameObject.GetComponent<Button>().interactable = true;
            threeChoices.transform.Find("Choice3").gameObject.GetComponentInChildren<Text>().text = choices[2];
            threeChoices.transform.Find("Choice3").gameObject.GetComponent<Button>().interactable = true;
            threeChoices.SetActive(true);
        }
        else if (choices.Length == 4)
        {
            fourChoices.transform.Find("Choice1").gameObject.GetComponentInChildren<Text>().text = choices[0];
            fourChoices.transform.Find("Choice1").gameObject.GetComponent<Button>().interactable = true;
            fourChoices.transform.Find("Choice2").gameObject.GetComponentInChildren<Text>().text = choices[1];
            fourChoices.transform.Find("Choice2").gameObject.GetComponent<Button>().interactable = true;
            fourChoices.transform.Find("Choice3").gameObject.GetComponentInChildren<Text>().text = choices[2];
            fourChoices.transform.Find("Choice3").gameObject.GetComponent<Button>().interactable = true;
            fourChoices.transform.Find("Choice4").gameObject.GetComponentInChildren<Text>().text = choices[3];
            fourChoices.transform.Find("Choice4").gameObject.GetComponent<Button>().interactable = true;
            fourChoices.SetActive(true);
        }
        else
        {
            Debug.Log("There are " + choices.Length + " multiple choices");
        }
    }

    //Will disable a button if the player makes an incorrect guess
    public void DisableIncorrectGuess(int buttonNum, int numChoices)
    {
        buttonNum += 1;
        string buttonName = "Choice" + buttonNum;
        if (numChoices == 2)
        {
            twoChoices.transform.Find(buttonName).gameObject.GetComponent<Button>().interactable = false;
        }
        else if (numChoices == 3)
        {
            threeChoices.transform.Find(buttonName).gameObject.GetComponent<Button>().interactable = false;
        }
        else if (numChoices == 4)
        {
            fourChoices.transform.Find(buttonName).gameObject.GetComponent<Button>().interactable = false;
        }
    }

    //Resets everything once the player chooses the correct option.
    public void ResetChoices()
    {
        twoChoices.SetActive(false);
        threeChoices.SetActive(false);
        fourChoices.SetActive(false);
    }
}
