using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseCursorManager : MonoBehaviour {
    [SerializeField]
    private int num;
    [SerializeField]
    private GameObject go;
    private GameManager gm;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        if (gm == null)
        {
            gm = go.GetComponent<GameManager>();
        }
        if (gm.Pause&&gm.PauseSelected == num)
        {
            GetComponent<Image>().enabled = true;
        }
        else
        {
            GetComponent<Image>().enabled = false;
        }
	}
}
