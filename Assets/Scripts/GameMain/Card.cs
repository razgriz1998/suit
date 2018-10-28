using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int Id { get; set; }
    public int Num { get; set; }//素点
    public int CalcNum{ get; set; }//カウント計算後の点
    static public List<string> Texts { get; private set; }
    static public List<string> Names { get; private set; }

    static Card()
    {
        Texts = new List<string>();
        Names = new List<string>();
        Texts.Add("あなたは山札からカードを1枚引く");
        Names.Add("ドロー");

        Texts.Add("全てのプレイヤーの場の数字＋１\nあなたは山札からカードを1枚引く");
        Names.Add("インフレ");

        Texts.Add("全てのプレイヤーの場の数字ー１\nあなたは山札からカードを1枚引く");
        Names.Add("デフレ");

        Texts.Add("全てのプレイヤーは自分の手札をすべて山札に戻し、新たに山札からカードを3枚引く");
        Names.Add("シャッフル");

        Texts.Add("どちらかの効果がランダムで発動する\n1．全てのプレイヤーの場の数字＋２\n2．全てのプレイヤーの場の数字―2");
        Names.Add("コイントス");

        Texts.Add("あなた以外の手札が3枚以上のプレイヤーは手札1枚をランダムに山札に戻す");
        Names.Add("ハンデス");

        Texts.Add("このカードの数字は、場に出したときのあなたの山札の枚数と同じになる");
        Names.Add("デッキカウント");

        Texts.Add("このカードの数字は、場に出したときのあなたの捨札の枚数と同じになる");
        Names.Add("トラッシュカウント");

        Texts.Add(null);
        Names.Add(null);

        Texts.Add("このカードの数字は、場に出したときのあなたの手札の枚数＋７になる");
        Names.Add("ハンドカウント");

        Texts.Add("このカードの数字は、場に出したときのあなたの場にあるカードの枚数＋５になる");
        Names.Add("フィールドカウント");
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
