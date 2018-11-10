using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
    [SerializeField]
    private GameObject go;
    private GameManager gm;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(gm == null){
            gm = go.GetComponent<GameManager>();
        }
        if (gm.Pause)
        {
            GetComponent<Canvas>().enabled = true;
        }
        else
        {
            GetComponent<Canvas>().enabled = false;
        }
	}
}
