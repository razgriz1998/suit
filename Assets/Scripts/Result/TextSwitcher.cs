using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSwitcher : MonoBehaviour {
    private float nextTime;
    [SerializeField]
    private float interval; //点滅周期
    int alpha = 1;
    Text text;

    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
        nextTime = Time.time + interval;

    }
	
	// Update is called once per frame
	void Update () {
        text.color = new Color(1, 1, 1, Mathf.Sin(Time.time*5.0f) * 0.5f + 0.5f);
	}
}
