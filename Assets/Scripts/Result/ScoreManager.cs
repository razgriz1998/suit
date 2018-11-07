using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour {
    private int[,] score;
    private List<int> winners=new List<int>();
    // Use this for initialization
    void Start() {
        GameManager gameManager = GameManager.Instance;
        score = gameManager.Score;
        for (int i = 0; i < 4; i++)
        {
            for( int j = 0; j < 6; j++)
            {
                Text scoreText = transform.Find("score" + i.ToString() + j.ToString()).GetComponent<Text>();
                scoreText.text = score[i, j].ToString();
            }
        }


        int min=10000;
        for (int i = 0; i < score.GetLength(0); i++)
        {
            int finalScore= score[i, score.GetLength(1) - 1];
            if (min > finalScore)
            {
                min = finalScore;
                winners.Clear();
                winners.Add(i + 1);
            }
            else if(min == finalScore)
            {
                winners.Add(i + 1);
            }
        }
        Text winnerText = transform.Find("Winner").GetComponent<Text>();
        foreach (int winner in winners) {
            winnerText.text += winner + "P ";
        }
        winnerText.text += " WIN!";

        gameManager.DestroyThis();
    }
	// Update is called once per frame
	void Update () {
	}
}
