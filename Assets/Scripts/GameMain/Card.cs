using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int Id { get; set; }
    public int Num { get; set; }//素点
    public int CalcNum{ get; set; }//カウント計算後の点
    public bool Count { get;  set; }//カウント系のカード
    public int Buf { get; set; }//バフ値
    static private List<string> texts;
    static private List<string> names;

    static Card()
    {
        texts = new List<string>();
        names = new List<string>();

        texts.Add("あなたは山札からカードを1枚引く");
        names.Add("ドロー");

        texts.Add("全てのプレイヤーの場の数字＋１\nあなたは山札からカードを1枚引く");
        names.Add("インフレ");

        texts.Add("全てのプレイヤーの場の数字ー１\nあなたは山札からカードを1枚引く");
        names.Add("デフレ");

        texts.Add("全てのプレイヤーは自分の手札をすべて山札に戻し、新たに山札からカードを3枚引く");
        names.Add("シャッフル");

        texts.Add("どちらかの効果がランダムで発動する\n1．全てのプレイヤーの場の数字＋２\n2．全てのプレイヤーの場の数字―2");
        names.Add("コイントス");

        texts.Add("あなた以外の手札が3枚以上のプレイヤーは手札1枚をランダムに山札に戻す");
        names.Add("ハンデス");

        texts.Add("このカードの数字は、場に出したときのあなたの山札の枚数と同じになる");
        names.Add("デッキカウント");

        texts.Add("このカードの数字は、場に出したときのあなたの捨札の枚数と同じになる");
        names.Add("トラッシュカウント");

        texts.Add(null);
        names.Add(null);

        texts.Add("このカードの数字は、場に出したときのあなたの手札の枚数＋７になる");
        names.Add("ハンドカウント");

        texts.Add("このカードの数字は、場に出したときのあなたの場にあるカードの枚数＋５になる");
        names.Add("フィールドカウント");
    }

    public string GetThisName()
    {
        return names[Id];
    }

    public string GetThisText()
    {
        return texts[Id];
    }

    private void Start()
    {
    }

    private void Update()
    {
        
    }
}
