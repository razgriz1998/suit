using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour {
    private AudioSource audioSource;
    [SerializeField]
    private float loopTime,endTime;
	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (audioSource.isPlaying)
        {
            if (audioSource.time >= endTime)
            {
                audioSource.time = loopTime;
            }
        }
	}
}
