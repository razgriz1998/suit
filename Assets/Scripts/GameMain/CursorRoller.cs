using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorRoller : MonoBehaviour {
    [SerializeField]
    private bool xRoller;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!xRoller)
        {
            transform.Rotate(new Vector3(0, 3.0f, 0));
        }
        else
        {
            transform.Rotate(new Vector3(3.0f, 0, 0));
        }
    }
}
