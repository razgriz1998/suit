using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisKeyManager : MonoBehaviour {

    //注意
    //この関数を使う際は、使う場所の前に以下のコードを記述する
    //private bool isKeyDown;
    //if (Input.GetAxis("Vertical1") == 0 && Input.GetAxis("Horizontal1") == 0) {
    //  isKeyDown = false;
    //}
    //
   
    /// <summary>
    /// 十字キーを押したときに動作
    /// -1:下　1:上
    /// </summary>
    /// <param name="controller_num"></param>
    /// <param name="isKeyDown"></param>
    /// <returns></returns>
    public int GetVerticalKeyDown(ref bool isKeyDown, string controller_num = "") {

        if (Input.GetAxis("Vertical" + controller_num) == 1.0f && !isKeyDown) {
            isKeyDown = true;
            return 1;
        }
        else if (Input.GetAxis("Vertical" + controller_num) == -1.0f && !isKeyDown) {
            isKeyDown = true;
            return -1;
        }
        else {
            return 0;
        }
    }

    /// <summary>
    /// 十字キーを押したときに動作
    /// -1:左　1:右
    /// </summary>
    /// <param name="controller_num"></param>
    /// <param name="isKeyDown"></param>
    /// <returns></returns>
    public int GetHorizontalKeyDown(ref bool isKeyDown,string controller_num = "") {

        if (Input.GetAxis("Horizontal" + controller_num) == 1.0f && !isKeyDown) {
            isKeyDown = true;
            return 1;
        }
        else if (Input.GetAxis("Horizontal" + controller_num) == -1.0f && !isKeyDown) {
            isKeyDown = true;
            return -1;
        }
        else {
            return 0;
        }
    }
}
