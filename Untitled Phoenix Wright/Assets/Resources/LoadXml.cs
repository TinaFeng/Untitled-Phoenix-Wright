using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Xml;
using System.IO;

public class Type_Dialogue // Type_Dialogue stores information for each individual dialogues.
                           // Such as who's speaking(name), what do they look like(icon), and what are they saying(text)
{
    
    public string name;
    public string icon;
    public string text;

}

public class LoadXml : MonoBehaviour {

    public TextAsset script;   //The XML file including the script

    public List<Type_Dialogue> dialogues = new List<Type_Dialogue>(); // make a list of dialogue types for future use
    public Dictionary<string, List<Type_Dialogue>> sections = new Dictionary<string, List<Type_Dialogue>>(); // a dictionary
    //that associates string with dialogue section
    private void Start()
    {
        GetScene();  // When it starts, call Get Scene (see below

    }
    public void GetScene()  // readoing the XML, split it into a list of informations per dialogue
    {
        XmlDocument xmlDoc = new XmlDocument();//create an xml
        xmlDoc.LoadXml(script.text);//load the script
        XmlNodeList SceneList = xmlDoc.GetElementsByTagName("Scene");//Getting all the scene nodes together
        string section_name = "null";                                // name used to link dictionary

        foreach(XmlNode SceneInfo in SceneList)//for each scene node (SceneInfo.Name == Scene)
        {

            XmlNodeList SceneContent = SceneInfo.ChildNodes; //Get all the content in this scene
            dialogues = new List<Type_Dialogue>();
            foreach (XmlNode SceneItems in SceneContent) // and for each content
            {
                if (SceneItems.Name == "name")//if it is a name,
                {
                    section_name = SceneItems.InnerText; // pull out the name and prepare it for dictionary
                }

                if(SceneItems.Name == "object")// if it is an object, built it into a Type_Dialogue
                {
                    Type_Dialogue line = new Type_Dialogue();

                    line.name = SceneItems.Attributes["name"].Value;//person speaking

                    string[] two_other = new string[2];//split the icon and text
                    two_other =SceneItems.InnerText.Split('*');
                
                    line.icon = two_other[0];
                    line.text = two_other[1];

                    //Debug.Log(line.name);
                    //Debug.Log(line.icon);
                    //Debug.Log(line.text);
                    dialogues.Add(line);//add the dialogue object to collection
                }

            }
            sections.Add(section_name, dialogues); // add name and dialogues to dictionary
        }

   
    }
}
