using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlideChange : MonoBehaviour {

    public GameObject[] obj;
    public string before_scene_name;
    public string next_scene_name;
    public Text slidenum_text;

    private int count;
    private bool isKeyDown;
    AxisKeyManager axiskeymanger;

    // Use this for initialization
    void Start () {
        axiskeymanger = new AxisKeyManager();
        obj[0].SetActive(true);
        slidenum_text.text = "( 1 / " + obj.Length + " )";
        count = 0;
	}
	
	// Update is called once per frame
	void Update () {

        //カーソル操作
        if (Input.GetAxis("Horizontal1") == 0) {
            isKeyDown = false;
        }

        int AxisValue = axiskeymanger.GetHorizontalKeyDown(ref isKeyDown, "1");

        if (AxisValue == 1) {
            if(count < obj.Length - 1) {
                count++;
                obj[count].SetActive(true);
                obj[count - 1].SetActive(false);
                slidenum_text.text = "( " + (count + 1) + " / " + obj.Length + " )";
            }
            else {
                SceneManager.LoadScene(next_scene_name);
            }
        }
        else if (AxisValue == -1) {
            if (count > 0) {
                count--;
                obj[count].SetActive(true);
                obj[count + 1].SetActive(false);
                
                slidenum_text.text = "( " + (count + 1) + " / " + obj.Length + " )";
                
            }
            else {
                SceneManager.LoadScene(before_scene_name);
            }
        }
    }
}