using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Quick and dirty script to handle drawing the defense bench.
public class BenchManager : MonoBehaviour {
    string last_bg;
    GameObject bg_object;
    public GameObject defense_bench;
    public GameObject witness_stand;
    public GameObject prosec_stand;
    string defense_bg = "defense_stand";
    string witness_bg = "witness_stand";
    string prosec_bg = "prosecution_stand";
	// Use this for initialization
	void Awake () {
        bg_object = GameObject.FindGameObjectWithTag("Background");
        last_bg = "";
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (bg_object.active==true && bg_object.GetComponent<Image>().sprite != null )
        {
            string bg = bg_object.GetComponent<Image>().sprite.name;
            if (last_bg != bg)
            {
                print(bg);
                //Check defense
                if (last_bg != defense_bg && bg == defense_bg)
                {
                    defense_bench.SetActive(true);
                }
                else
                {
                    if (last_bg == defense_bg && bg != defense_bg)
                    {
                        defense_bench.SetActive(false);
                    }
                }
                //Check witness
                if (last_bg != witness_bg && bg == witness_bg)
                {
                    witness_stand.SetActive(true);
                }
                else
                {
                    if (last_bg == witness_bg && bg != witness_bg)
                    {
                        witness_stand.SetActive(false);
                    }
                }
                //Check prosecution
                if (last_bg != prosec_bg && bg == prosec_bg)
                {
                    prosec_stand.SetActive(true);
                }
                else
                {
                    if (last_bg == prosec_bg && bg != prosec_bg)
                    {
                        prosec_stand.SetActive(false);
                    }
                }
                last_bg = bg;
            }
        }
	}
}
