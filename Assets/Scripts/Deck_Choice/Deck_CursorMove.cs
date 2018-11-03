using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deck_CursorMove : MonoBehaviour {

    [SerializeField]
    private int player_num;

    [SerializeField]
    private Vector3 red_deck_pos;

    [SerializeField]
    private Vector3 black_deck_pos;

    [SerializeField]
    private GameObject DecidedObj;

    [SerializeField]
    private GameObject PauseCanvas;

    [SerializeField]
    private GameObject ContinueIcon;

    [SerializeField]
    private GameObject ExitIcon;

    private bool isKeyDown;
    private bool isDecide;

    AxisKeyManager axiskeymanger;

    [SerializeField]
    private AudioSource SE_decide;

    [SerializeField]
    private AudioSource SE_cancel;

	// Use this for initialization
	void Start () {
        if (player_num == 1) {
            PauseCanvas.SetActive(false);
            ContinueIcon.SetActive(true);
            ExitIcon.SetActive(false);
        }
        axiskeymanger = new AxisKeyManager();
        this.transform.localPosition = red_deck_pos;
        DecidedObj.SetActive(false);
        isDecide = false;
    }
	
	// Update is called once per frame
	void Update () {

        //コントローラ操作
        //通常時
        if (!GameState.Instance.isPause) {

            if (!isDecide) {
                //カーソル操作
                if (Input.GetAxis("Horizontal" + player_num) == 0) {
                    isKeyDown = false;
                }

                int AxisValue = axiskeymanger.GetHorizontalKeyDown(ref isKeyDown, player_num.ToString());

                if (AxisValue == 1 || AxisValue == -1 || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) {
                    if (this.transform.localPosition == red_deck_pos) {
                        this.transform.localPosition = black_deck_pos;
                    }
                    else if (this.transform.localPosition == black_deck_pos) {
                        this.transform.localPosition = red_deck_pos;
                    }
                }

                //決定
                if (Input.GetButtonDown("Submit" + player_num)) {
                    if (this.transform.localPosition == red_deck_pos) {
                        GameState.Instance.deck_type[player_num - 1] = 1;
                    }
                    else if (this.transform.localPosition == black_deck_pos) {
                        GameState.Instance.deck_type[player_num - 1] = 0;
                    }
                    SE_decide.PlayOneShot(SE_decide.clip);
                    DecidedObj.SetActive(true);
                    GameState.Instance.isSetDeck++;
                    isDecide = true;
                }
            }
            else {

                //キャンセル
                if (Input.GetButtonDown("Cancel" + player_num)) {
                    SE_cancel.PlayOneShot(SE_cancel.clip);
                    DecidedObj.SetActive(false);
                    GameState.Instance.isSetDeck--;
                    isDecide = false;
                }
            }

            //ポーズ
            if (Input.GetButtonDown("Pause" + player_num)) {

                PauseCanvas.SetActive(true);
                GameState.Instance.isPause = true;

            }

            //シーンチェンジ
            if (player_num == 1 && GameState.Instance.isSetDeck == 4) {
                SceneManager.LoadScene("Game_Main");
            }


        }
        //ポーズ中
        else {

            //カーソル操作
            if (Input.GetAxis("Vertical" + player_num) == 0) {
                isKeyDown = false;
            }

            int AxisValue = axiskeymanger.GetVerticalKeyDown(ref isKeyDown, player_num.ToString());

            if (AxisValue == 1 || AxisValue == -1) {
                if (ContinueIcon.active) {
                    ContinueIcon.SetActive(false);
                    ExitIcon.SetActive(true);
                }
                else if (ExitIcon.active) {
                    ExitIcon.SetActive(false);
                    ContinueIcon.SetActive(true);
                }
            }

            //決定
            if (Input.GetButtonDown("Submit" + player_num)) {
                if (ContinueIcon.active) {

                    PauseCanvas.SetActive(false);
                    GameState.Instance.isPause = false;
                }
                else if (ExitIcon.active) {

                    SceneManager.LoadScene("Title");
                }
            }

            //キャンセル
            if (Input.GetButtonDown("Cancel" + player_num)) {
                PauseCanvas.SetActive(false);
                GameState.Instance.isPause = false;
            }
        }
    }
}
