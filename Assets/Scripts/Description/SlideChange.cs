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

    private int page_num;
    private bool isKeyDown;

    AxisKeyManager axiskeymanger;

    private AudioSource SE_pagechange;

    // Use this for initialization
    void Start () {
        axiskeymanger = new AxisKeyManager();
        obj[0].SetActive(true);
        slidenum_text.text = "( 1 / " + obj.Length + " )";
        page_num = 0;
        SE_pagechange = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        //カーソル操作
        if (Input.GetAxis("Horizontal1") == 0) {
            isKeyDown = false;
        }

        int AxisValue = axiskeymanger.GetHorizontalKeyDown(ref isKeyDown, "1");

        if (AxisValue == 1) {

            SE_pagechange.PlayOneShot(SE_pagechange.clip);

            if(page_num < obj.Length - 1) {
                page_num++;
                obj[page_num].SetActive(true);
                obj[page_num - 1].SetActive(false);
                slidenum_text.text = "( " + (page_num + 1) + " / " + obj.Length + " )";
            }
            else {
                SceneManager.LoadScene(next_scene_name);
            }
        }
        else if (AxisValue == -1) {

            SE_pagechange.PlayOneShot(SE_pagechange.clip);

            if (page_num > 0) {
                page_num--;
                obj[page_num].SetActive(true);
                obj[page_num + 1].SetActive(false);
                
                slidenum_text.text = "( " + (page_num + 1) + " / " + obj.Length + " )";
                
            }
            else {
                SceneManager.LoadScene(before_scene_name);
            }
        }
    }
}