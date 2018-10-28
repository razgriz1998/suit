using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkIcon : MonoBehaviour {
    /*
    [SerializeField]
    private GameObject Obj;
    */
    [SerializeField]
    private float BlinkSpeed = 0.05f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float color_a = this.gameObject.GetComponent<Image>().color.a;

        if (color_a < 0.0f || color_a > 1.0f) {
            BlinkSpeed *= -1;
        }

        this.GetComponent<Image>().color = new Color(255, 255, 255, color_a + BlinkSpeed);
	}
}
