using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipManager : MonoBehaviour {
    private SpriteRenderer sr;
    [SerializeField]
    private Sprite infl;
    [SerializeField]
    private Sprite defl;
    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
        if (Player.SpecialPoint > 0)
        {
            sr.enabled = true;
            sr.sprite = infl;
        }
        else if(Player.SpecialPoint < 0)
        {
            sr.enabled = true;
            sr.sprite = defl;
        }
        else
        {
            sr.enabled = false;
        }
    }
}
