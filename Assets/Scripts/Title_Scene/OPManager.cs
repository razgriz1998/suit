using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPManager : MonoBehaviour {

    static OPManager _instance = null;

    static OPManager instance
    {
        get { return _instance ?? (_instance = FindObjectOfType<OPManager>()); }
    }

    private AudioSource audioSource;

    [SerializeField]
    private float loopTime, endTime;

    // Use this for initialization
    void Start() {
        if (this != instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (audioSource.isPlaying) {
            if (audioSource.time >= endTime) {
                audioSource.time = loopTime;
            }
        }

        if (Application.loadedLevelName == "Game_Main") {
            if (this == instance) {
                _instance = null;
            }
            Destroy(this.gameObject);
        }
    }
}
