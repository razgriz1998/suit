using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushExitButton : MonoBehaviour {

    public void PushButton() {
        Application.Quit();
        Debug.Log("Application Quit");
    }

}
