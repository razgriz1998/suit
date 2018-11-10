using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitcher : MonoBehaviour {

    GameManager gm;
    // Use this for initialization
    void Start () {
        gm = GameManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
        if (gm.KeyInput)
        {
            if (gm.TurnEndButton)
            {
                Debug.Log("おいおいおい");
                GetComponent<Canvas>().enabled = false;
            }
            else
            {
                GetComponent<Canvas>().enabled = true;
            }
        }
	}
}
