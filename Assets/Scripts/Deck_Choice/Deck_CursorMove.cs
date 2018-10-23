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

    private bool isKeyDown;
    private bool isDecide;

    AxisKeyManager axiskeymanger;

	// Use this for initialization
	void Start () {

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

                if (AxisValue == 1 || AxisValue == -1) {
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
                        GameState.Instance.deck_type[player_num - 1] = 0;
                    }
                    else if (this.transform.localPosition == black_deck_pos) {
                        GameState.Instance.deck_type[player_num - 1] = 1;
                    }

                    DecidedObj.SetActive(true);
                    GameState.Instance.isSetDeck++;
                    isDecide = true;
                }
            }
            else {

                //キャンセル
                if (Input.GetButtonDown("Cancel" + player_num)) {
                    DecidedObj.SetActive(false);
                    GameState.Instance.isSetDeck--;
                    isDecide = false;
                }
            }

            //ポーズ
            if (Input.GetButtonDown("Pause" + player_num)) {
                Debug.Log(player_num + "start");
            }

            //シーンチェンジ
            if (player_num == 1 && GameState.Instance.isSetDeck == 4) {
                SceneManager.LoadScene("Game_Main");
            }


        }
        //ポーズ中
        else {

        }
    }
}
