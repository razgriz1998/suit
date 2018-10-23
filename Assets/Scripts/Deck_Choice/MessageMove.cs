using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageMove : MonoBehaviour {

    [SerializeField]
    private Vector3 start_pos;

    [SerializeField]
    private Vector3 end_pos;

    [SerializeField]
    private float move_speed = 1.0f;

	// Use this for initialization
	void Start () {
        this.transform.localPosition = start_pos;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 current_pos = this.transform.localPosition;

        current_pos.x -= move_speed;

        if (current_pos.x < end_pos.x)
            current_pos = start_pos;

        this.transform.localPosition = current_pos;
        
	}
}
