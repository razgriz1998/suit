using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorMove : MonoBehaviour {

    [SerializeField]
    private Vector3 StartButton_pos = Vector2.zero;
    /*
    [SerializeField]
    private Vector3 RuleButton_pos = Vector2.zero;
    */
    [SerializeField]
    private Vector3 ExitButton_pos = Vector2.zero;
    
    [SerializeField]
    private string StartSceneName;
    /*
    [SerializeField]
    private string RuleSceneName;
    */

    [SerializeField]
    private int rotate_speed = 5;

    [SerializeField]
    private AudioSource SE_cursor_move;

    [SerializeField]
    private AudioSource SE_decide;

    private bool isKeyDown;

    AxisKeyManager axiskeymanager = new AxisKeyManager();

    // Use this for initialization
    void Start() {
        this.transform.localPosition = StartButton_pos;

        Cursor.visible = false;

        //シングルトン初期化
        GameState.Instance.Init();
    }

    // Update is called once per frame
    void Update() {

        //カーソルの回転アニメーション
        transform.Rotate(rotate_speed, 0, 0);
        
        //コントローラ操作
        if (Input.GetAxis("Vertical1") == 0 && Input.GetAxis("Horizontal1") == 0) {
            isKeyDown = false;
        }
        
        int AxisValue = axiskeymanager.GetVerticalKeyDown(ref isKeyDown,"1");

        /*
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
        */
        if (AxisValue == 1 || AxisValue == -1 || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)) {
            if (this.transform.localPosition == StartButton_pos) {
                this.transform.localPosition = ExitButton_pos;
            }
            else if (this.transform.localPosition == ExitButton_pos) {
                this.transform.localPosition = StartButton_pos;
            }
            SE_cursor_move.PlayOneShot(SE_cursor_move.clip);

        }

        //シーンチェンジ
        if (Input.GetButtonDown("Submit1")){

            SE_decide.PlayOneShot(SE_decide.clip);

            if (this.transform.localPosition == StartButton_pos) {
                
                SceneManager.LoadScene(StartSceneName);
            }
            /*
            else if (this.transform.localPosition == RuleButton_pos) {
                SceneManager.LoadScene(RuleSceneName);
            }
            */
            else if (this.transform.localPosition == ExitButton_pos) {
                Application.Quit();
                Debug.Log("Application Quit");
            }
        }
    }
}
