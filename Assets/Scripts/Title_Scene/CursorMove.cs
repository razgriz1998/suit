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
    private GameObject Start_Icon;

    [SerializeField]
    private int rotate_speed = 5;

    [SerializeField]
    private AudioSource SE_cursor_move;

    [SerializeField]
    private AudioSource SE_decide;

    private bool isFirstRotate;
    private int rotate_count;

    private bool isKeyDown;
    private bool isDecide;

    private float scene_change_delay = 0.5f;
    private float flashing_time;
    private float flashing_interval = 0.02f;
    private bool isIconEnable;

    AxisKeyManager axiskeymanager = new AxisKeyManager();

    // Use this for initialization
    void Start() {
        this.transform.localPosition = StartButton_pos;

        Cursor.visible = false;

        rotate_count = 0;

        isFirstRotate = true;

        isDecide = false;

        flashing_time = 0;

        isIconEnable = true;
        //シングルトン初期化
        GameState.Instance.Init();
    }

    // Update is called once per frame
    void Update() {

        if (!isDecide) {
            //カーソルの回転アニメーション
            if (Mathf.Abs(this.transform.localEulerAngles.x) <= 0.01f) {
                if (isFirstRotate) {
                    rotate_count++;
                    if (rotate_count >= 30) {
                        isFirstRotate = !isFirstRotate;
                        rotate_count = 0;
                        transform.Rotate(rotate_speed, 0, 0);
                    }
                }
                else {
                    isFirstRotate = !isFirstRotate;
                    transform.Rotate(rotate_speed, 0, 0);
                }
            }
            else {
                transform.Rotate(rotate_speed, 0, 0);
            }

            //コントローラ操作
            if (Input.GetAxis("Vertical1") == 0 && Input.GetAxis("Horizontal1") == 0) {
                isKeyDown = false;
            }

            int AxisValue = axiskeymanager.GetVerticalKeyDown(ref isKeyDown, "1");

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
            if (Input.GetButtonDown("Submit1")) {

                SE_decide.PlayOneShot(SE_decide.clip);

                if (this.transform.localPosition == StartButton_pos) {
                    isDecide = true;
                    StartCoroutine(ChangeNextScene());
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
        else {
            //決定時のアニメーション
            transform.Rotate(rotate_speed * 5, 0, 0);

            flashing_time += Time.deltaTime;
            if (flashing_time >= flashing_interval) {
                flashing_time = 0;
                Start_Icon.SetActive(!isIconEnable);
                isIconEnable = !isIconEnable;
            }
        }
    }

    IEnumerator ChangeNextScene() {

        yield return new WaitForSeconds(scene_change_delay);

        SceneManager.LoadScene(StartSceneName);

    }
}


