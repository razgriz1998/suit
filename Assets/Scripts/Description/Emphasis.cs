using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emphasis : MonoBehaviour {

    private float sumTime = 0.0f;
    private float intervalTime = 0.2f;
    void Update() {
            sumTime += Time.deltaTime;
            if (sumTime > 8.0) {
                intervalTime += Time.deltaTime;
            if (intervalTime >= 0.2f) {
                intervalTime = 0.0f;
                //gameObject.renderer.enabled = !gameObject.renderer.enabled;
            }
        }
    }
}
