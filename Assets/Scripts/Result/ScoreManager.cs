using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour {
    private int[,] score;
    private int[] finalScore;
    private int[] rank;
    // Use this for initialization
    void Start() {
        GameManager gameManager = GameManager.Instance;
        score = gameManager.Score;
        finalScore = new int[score.GetLength(0)];
        rank = new int[score.GetLength(0)];
        for (int i = 0; i < score.GetLength(0); i++)
        {
            finalScore[i] = score[i, score.GetLength(1) - 1];
        }
        List<int> top = new List<int>();
        bool[] comp = new bool[score.GetLength(0)];
        for (int i = 0; i < comp.Length; i++)
        {
            comp[i] = false;
        }
        for (int r = 0; r < score.GetLength(0);)
        {
            int min = 1000;
            for (int i = 0; i < score.GetLength(0); i++)
            {
                if (!comp[i])
                {
                    if (min > finalScore[i])
                    {
                        top.Clear();
                        top.Add(i);
                        min = finalScore[i];
                    }
                    else if (min == finalScore[i])
                    {
                        top.Add(i);
                    }
                }
            }
            foreach (int t in top)
            {
                rank[t] = r + 1;
                comp[t] = true;
            }
            r += top.Count;
        }

        

        Text text = GetComponent<Text>();
        text.text = "";
        for (int i = 0; i < score.GetLength(0); i++)
        {
            text.text += rank[i] + "位 " + (i + 1) + "P " + finalScore[i] + "ポイント\n";
        }
    }
	// Update is called once per frame
	void Update () {
	}
}
