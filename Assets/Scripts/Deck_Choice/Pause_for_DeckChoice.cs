using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_for_DeckChoice : MonoBehaviour {

    [SerializeField]
    private GameObject PauseCanvas;

    [SerializeField]
    private GameObject ContinueIcon;

    [SerializeField]
    private GameObject ExitIcon;

    private bool isKeyDown;
    private int player_num = 0;

    AxisKeyManager axiskeymanger;

    // Use this for initialization
    void Start () {
        axiskeymanger = new AxisKeyManager();
        PauseCanvas.SetActive(false);
        ContinueIcon.SetActive(true);
        ExitIcon.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if (!GameState.Instance.isPause) {

            for (int GamePadNum = 1; GamePadNum <= 4; GamePadNum++) {

                if (Input.GetButtonDown("Pause" + GamePadNum)) {

                    player_num = GamePadNum;
                    PauseCanvas.SetActive(true);
                    GameState.Instance.isPause = true;

                }
            }
 
        }
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
