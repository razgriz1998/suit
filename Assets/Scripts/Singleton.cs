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

    //ポーズ中か否か
    public bool isPause = false;

    //1…黒,0…赤
    public int[] deck_type = new int[4];

    //デッキ選択した人数（デッキ選択で4になればシーンチェンジ)
    public int isSetDeck = 0;

    //初期化
    public void Init() {
        isPause = false;
        for (int i = 0; i < 4; i++) {
            deck_type[i] = 0;
        }
        isSetDeck = 0;
    }
}