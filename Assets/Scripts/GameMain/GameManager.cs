using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
    public static GameManager Instance
    {
        get; private set;
    }
    public List<Player> Players { get; private set; }
    private int turnPlayer = 0;
    private int handMax = 7;
    private int selectedHand = 0;
    private int turn = 0;
    private int turnNum = 5;
    private bool gameEnd = false;
    private bool nextPlayer = false;
    private bool cardPlay = false;
    public int[,] Score { get; private set; }
    private int playerNum = 4;
    private int turnCount = 0;

    [SerializeField]
    private Sprite drawSprite, inflationSprite, deflationSprite, shuffleSprite, cointssSprite,
        handeathSprite, deckcountSprite, trashcountSprite, vaniraSprite,handcountSprite,fieldcountSprite;
    private List<Sprite> cardSprites;
    [SerializeField]
    private string nextScene;

    // Use this for initialization
    void Start () {
        cardSprites = new List<Sprite> {drawSprite, inflationSprite, deflationSprite, shuffleSprite, cointssSprite,
        handeathSprite, deckcountSprite, trashcountSprite, vaniraSprite ,handcountSprite,fieldcountSprite};
        Players = new List<Player>();
        Score = new int[playerNum,turnNum+1];
        for (int i = 0; i < playerNum; i++)
        {
            Players.Add(GameObject.Find("Player"+(i+1)).GetComponent<Player>());
        }
        turnPlayer = turn;
        //HandUpdate();
        Players[turnPlayer].ShowHands();
        ChangeTurnPlayer();
        HandUpdate();
        InfoUpdate();
    }



    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);


    }

    // Update is called once per frame
    void Update () {
        if (!gameEnd)
        {
            if (Key())
            {
                if (cardPlay)
                {
                    Play();
                }
                if (Players[turnPlayer].HandsList.Count==0)
                {
                    nextPlayer = true;
                }
                if (nextPlayer)
                {
                    turnCount++;
                    Players[turnPlayer].HideHands();
                    turnPlayer = (turnPlayer + 1) % Players.Count;
                    
                    if (turnCount == playerNum)//ターンチェンジ判定
                    {
                        ScoreUpdate();
                        turnCount = 0;
                        turn++;
                        if (turn < playerNum)//最終ターン以外
                        {
                            turnPlayer = turn;
                        }
                        else//最終ターン
                        {
                            int max = -1, index = 0;
                            for(int i = 0; i < playerNum; i++)
                            {
                                if (max < Players[i].TotalScore)
                                {
                                    max = Players[i].TotalScore;
                                    index = i;
                                }
                            }
                            turnPlayer = (index + 1) % playerNum;
                        }
                        ChangeTurnPlayer();
                        foreach (Player p in Players)
                        {
                            p.TurnChange();
                        }
                        
                        
                        if (turn == turnNum)//ゲーム終了
                        {
                            gameEnd = true;
                            GameObject go = GameObject.Find("ScoreBoard");
                            for (int i = 0; i < playerNum; i++) {
                                Text text = go.transform.Find((i + 1) + "PTotal").GetComponent<Text>();
                                text.text =Players[i].TotalScore.ToString();
                                Score[i,turnNum] = Players[i].TotalScore;
                            }
                        }
                        else
                        {
                            Players[turnPlayer].ShowHands();

                        }
                    }
                    else
                    {
                        Players[turnPlayer].ShowHands();
                        ChangeTurnPlayer();
                    }
                }
                if ((cardPlay || nextPlayer )&& !gameEnd)
                {
                    selectedHand = 0;
                    InfoUpdate();
                }
                if (!gameEnd)
                {
                    HandUpdate();
                }


                //HandUpdate();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E)|| Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(nextScene);
            }
        }
	}

    bool Key()
    {
        nextPlayer = false;
        cardPlay = false;
        int handCount = Players[turnPlayer].HandsList.Count;
            if (Input.GetKeyDown(KeyCode.D))
            {
                selectedHand = (++selectedHand) % handCount;
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                --selectedHand;
                if (selectedHand == -1)
                {
                    selectedHand = handCount - 1;
                }
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                cardPlay = true;
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                nextPlayer = true;
                return true;
            }
        return false;
}
    void InfoUpdate()
    {
        GameObject fieldPoint = GameObject.Find("FieldPoint");
        Text[] texts = { fieldPoint.transform.Find("1P").GetComponent<Text>() ,
            fieldPoint.transform.Find("2P").GetComponent<Text>() ,
        fieldPoint.transform.Find("3P").GetComponent<Text>()};
        Text[] hands = { fieldPoint.transform.Find("hand1").GetComponent<Text>() ,
            fieldPoint.transform.Find("hand2").GetComponent<Text>() ,
        fieldPoint.transform.Find("hand3").GetComponent<Text>()};
        Text[] points = { fieldPoint.transform.Find("Point1").GetComponent<Text>() ,
            fieldPoint.transform.Find("Point2").GetComponent<Text>() ,
        fieldPoint.transform.Find("Point3").GetComponent<Text>()};

        int index = 0;
        for (int i = 0; i < playerNum - 1; i++) {
            if (index == turnPlayer)
            {
                index++;
            }
            texts[i].text = (index+1) + "P";
            hands[i].text = Players[index].HandsList.Count+"";
            points[i].text= Players[index].NormalPoint+"";
            index++;
        }

        GameObject playerInfo = GameObject.Find("PlayerInfo");
        playerInfo.transform.Find("Deck").GetComponent<Text>().text= Players[turnPlayer].DeckList.Count+"";
        playerInfo.transform.Find("Trash").GetComponent<Text>().text = Players[turnPlayer].TrashList.Count + "";
        Text buff = playerInfo.transform.Find("Buff").GetComponent<Text>();
        if (Players[turnPlayer].SpecialPoint > 0)
        {
            buff.text = "+" + Players[turnPlayer].SpecialPoint;
        }
        else if (Players[turnPlayer].SpecialPoint < 0)
        {
            buff.text = "" + Players[turnPlayer].SpecialPoint;
        }
        else
        {
            buff.text = "±" + Players[turnPlayer].SpecialPoint;
        }
        playerInfo.transform.Find("Point").GetComponent<Text>().text = Players[turnPlayer].NormalPoint + "";
        GameObject gameInfo = GameObject.Find("GameInfo");
        Text gametext = gameInfo.transform.Find("Text").GetComponent<Text>();
        gametext.text = (turn + 1) + "ターン目 " + (turnPlayer+1) + "Pの番";

    }
    void ScoreUpdate()
    {
        GameObject go = GameObject.Find("ScoreBoard");
        for (int i = 0; i < playerNum; i++)
        {
            Players[i].ScoreUpdate();
            Text text = go.transform.Find("score"+i+""+turn).GetComponent<Text>();
            text.enabled = true;
            text.text = Players[i].Score[turn]+"";
            Score[i,turn] = Players[i].Score[turn];
            Text total = go.transform.Find((i + 1) + "PTotal").GetComponent<Text>();
            total.text = Players[i].TotalScore.ToString();
            Score[i, turnNum] = Players[i].TotalScore;
        }


    }
    void HandUpdate()
    {
        for (int i = 0; i < Players[turnPlayer].HandsList.Count; i++)
        {
            Players[turnPlayer].HandsList[i].transform.Find("Arrow").GetComponent<Image>().enabled = false;

        }
        Players[turnPlayer].HandsList[selectedHand].transform.Find("Arrow").GetComponent<Image>().enabled = true;
    }

        public void AllPlayerShaffle()
    {
        foreach(Player p in Players)
        {
            p.DeckShuffle();
        }
    }
    public void AllPlayerDraw()
    {
        foreach (Player p in Players)
        {
            p.Draw();
        }
    }
    public void AllBuff(int i)
    {
        foreach (Player p in Players)
        {
            p.SpecialPoint += i;
        }
    }
    public void Handeath()
    {
        for(int i=0;i<Players.Count;i++)
        {
            if (i!=turnPlayer && Players[i].HandsList.Count != 0)
            {
                Players[i].Handeath();
            }
        }
    }

    public void Play()
    {
        int id = Players[turnPlayer].HandsList[selectedHand].GetComponent<Card>().Id;
        Players[turnPlayer].Play(selectedHand);
        switch (id)
        {
            case 0: Players[turnPlayer].Draw(); break;//ドロー
            case 1:
                Players[turnPlayer].Draw();//インフレ
                AllBuff(1);
                break;
            case 2:
                Players[turnPlayer].Draw();//デフレ
                AllBuff(-1); break;
            case 3:

                foreach (Player p in Players)
                {
                    p.AllHandsToDeck();
                }
                AllPlayerShaffle();//シャッフル
                for (int i = 0; i < 3; i++)
                {
                    AllPlayerDraw();
                }
                break;
            case 4://コイントス
                if (Random.Range(0, 2) == 0)
                {
                    AllBuff(2);
                    //Players[turnPlayer].SpecialPoint += 2;
                }
                else
                {
                    AllBuff(-2);
                    //Players[turnPlayer].SpecialPoint -= 2;
                }
                break;
            case 5: Handeath(); break;//ハンデス
            case 6:
                Players[turnPlayer].FieldList[Players[turnPlayer].FieldList.Count - 1].GetComponent<Card>().Num += Players[turnPlayer].DeckList.Count; break;
            case 7:
                Players[turnPlayer].FieldList[Players[turnPlayer].FieldList.Count - 1].GetComponent<Card>().Num += Players[turnPlayer].TrashList.Count; break;
            case 8: Players[turnPlayer].FieldList[Players[turnPlayer].FieldList.Count - 1].GetComponent<Card>().Num += Players[turnPlayer].HandsList.Count; break;
            case 9: Players[turnPlayer].FieldList[Players[turnPlayer].FieldList.Count - 1].GetComponent<Card>().Num += Players[turnPlayer].FieldList.Count; break;
        }
        Players[turnPlayer].NormalPoint += Players[turnPlayer].FieldList[Players[turnPlayer].FieldList.Count - 1].GetComponent<Card>().Num;

    }

    public void ChangeTurnPlayer()
    {
        foreach (Player p in Players)
        {
            p.TurnPlayer = false;
        }
        if (turnPlayer >= 0 && turnPlayer < Players.Count) {
            Players[turnPlayer].TurnPlayer = true;
        }
    }
}
