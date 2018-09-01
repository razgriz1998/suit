using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PushStartButton : MonoBehaviour {

    [SerializeField]
    private string next_scene = null;

    public void PushButton()
    {
        SceneManager.LoadScene(next_scene);
    }
}
