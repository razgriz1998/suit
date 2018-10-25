using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : class, new() {
    protected Singleton() {
        Debug.Assert(instance == null);
    }
    protected static T instance;
    static Singleton() {
        instance = new T();
    }

    public static T Instance
    {
        get {
            return instance;
        }
    }
}

public class GameState : Singleton<GameState> {
    /*
    int[] _ModelNum = new int[4];
    public int PlayerNum { get; set; }
    public int[] ControllerNum = new int[4];
    public int[] ModelNum
    {
        get { return _ModelNum; }
        set { _ModelNum = value; }
    }
    */

    //ポーズ中か否か
    public bool isPause = false;

    //0…赤,1…黒
    public int[] deck_type = new int[4];

    //デバッグ用
    public int isSetDeck = 0;
}