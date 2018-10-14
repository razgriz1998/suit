using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck_CursorMove : MonoBehaviour {

    [SerializeField]
    private int player_num;

    [SerializeField]
    private Vector3 red_deck_pos;

    [SerializeField]
    private Vector3 black_deck_pos;

    private bool isKeyDown;

    AxisKeyManager axiskeymanger;

	// Use this for initialization
	void Start () {

        axiskeymanger = new AxisKeyManager();
        this.transform.localPosition = red_deck_pos;
    }
	
	// Update is called once per frame
	void Update () {

        //コントローラ操作
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
    }
}
