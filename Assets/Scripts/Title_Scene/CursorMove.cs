﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorMove : MonoBehaviour {

    [SerializeField]
    private Vector3 StartButton_pos = Vector2.zero;

    [SerializeField]
    private Vector3 RuleButton_pos = Vector2.zero;

    [SerializeField]
    private Vector3 ExitButton_pos = Vector2.zero;

    [SerializeField]
    private string StartSceneName;

    [SerializeField]
    private string RuleSceneName;

    private bool isKeyDown;


    // Use this for initialization
    void Start() {
        this.transform.localPosition = StartButton_pos;
    }

    // Update is called once per frame
    void Update() {

        //カーソルの回転
        transform.Rotate(5, 0, 0);

        //コントローラ操作
        if (Input.GetAxis("Vertical") == 0) {
            isKeyDown = false;
        }

        int AxisValue = GetAxisDown();

        if (AxisValue == 1) {
            if (this.transform.localPosition == StartButton_pos) {
                this.transform.localPosition = ExitButton_pos;
            }
            else if (this.transform.localPosition == RuleButton_pos) {
                this.transform.localPosition = StartButton_pos;
            }
            else if (this.transform.localPosition == ExitButton_pos) {
                this.transform.localPosition = RuleButton_pos;
            }
        }
        else if (AxisValue == -1) {
            if (this.transform.localPosition == StartButton_pos) {
                this.transform.localPosition = RuleButton_pos;
            }
            else if (this.transform.localPosition == RuleButton_pos) {
                this.transform.localPosition = ExitButton_pos;
            }
            else if (this.transform.localPosition == ExitButton_pos) {
                this.transform.localPosition = StartButton_pos;
            }
        }

        //シーンチェンジ
        if (Input.GetKeyDown("joystick button 1")){
            if (this.transform.localPosition == StartButton_pos) {
                SceneManager.LoadScene(StartSceneName);
            }
            else if (this.transform.localPosition == RuleButton_pos) {
                SceneManager.LoadScene(RuleSceneName);
            }
            else if (this.transform.localPosition == ExitButton_pos) {
                Application.Quit();
                Debug.Log("Application Quit");
            }
        }
    }

    public int GetAxisDown() {
       
        if (Input.GetAxis("Vertical") == 1.0f && !isKeyDown) {
            isKeyDown = true;
            return 1;
        }
        else if(Input.GetAxis("Vertical") == -1.0f && !isKeyDown){
            isKeyDown = true;
            return -1;
        }
        else {
            return 0;
        }
    }
}
