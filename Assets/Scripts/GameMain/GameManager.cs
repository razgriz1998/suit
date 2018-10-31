﻿using System.Collections;
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
    private bool turnEndButton = false;
    private int turn = 0;
    private int turnNum = 5;
    private bool gameEnd = false;
    private bool nextPlayer = false;
    private bool decide = false;
    public int[,] Score { get; private set; }
    private int playerNum = 4;
    private int turnCount = 0;
    private Animator playing = null;
    private int frameCount=0;
    public bool pause { get; private set; }
    public int pauseSelected{ get; private set; }
    private const int numPauseText = 2;
    [SerializeField]
    GameObject panel;

    [SerializeField]
    private Sprite drawSprite, inflationSprite, deflationSprite, shuffleSprite, cointssSprite,
        handeathSprite, deckcountSprite, trashcountSprite, vaniraSprite,handcountSprite,fieldcountSprite;
    private List<Sprite> cardSprites;
    [SerializeField]
    private string nextScene;

    private bool isKeyDown;
    AxisKeyManager axiskeymanager;

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
        pauseSelected = 0;
        axiskeymanager = new AxisKeyManager();
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
            
            if(playing!=null && !playing.GetCurrentAnimatorStateInfo(0).IsName("Animation")&&frameCount!=0 )
            {
                playing = null;
                panel.GetComponent<Image>().enabled = false;
            }
            if (!pause)
            {
                if (playing == null && Key())
                {
                    if (decide)
                    {
                        Play();
                    }
                    if (Players[turnPlayer].HandsList.Count == 0)
                    {
                        nextPlayer = true;
                    }
                    if (nextPlayer)
                    {
                        turnCount++;
                        turnEndButton = false;
                        Players[turnPlayer].HideHands();
                        turnPlayer = (turnPlayer + 1) % Players.Count;

                        if (turnCount == playerNum)//ターンチェンジ判定
                        {
                            TurnChange();
                            if (turn == turnNum)//ゲーム終了
                            {
                                gameEnd = true;
                                GameObject go = GameObject.Find("ScoreBoard");
                                for (int i = 0; i < playerNum; i++)
                                {
                                    Text text = go.transform.Find((i + 1) + "PTotal").GetComponent<Text>();
                                    text.text = Players[i].TotalScore.ToString();
                                    Score[i, turnNum] = Players[i].TotalScore;
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
                    if ((decide || nextPlayer) && !gameEnd)
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
            else if (pause)
            {
                Debug.Log(pauseSelected);
                if (Key())
                {
                    if (decide)
                    {
                        switch (pauseSelected)
                        {
                            case 0: pause = false;break;
                            case 1: SceneManager.LoadScene("title"); break;
                        }
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E)|| Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(nextScene);
            }
        }
        frameCount++;
	}

    bool Key()
    {
        nextPlayer = false;
        decide = false;
        int handCount = Players[turnPlayer].HandsList.Count;

        if (Input.GetAxis("Vertical" + (turnPlayer + 1).ToString() ) == 0 && Input.GetAxis("Horizontal" + (turnPlayer + 1).ToString()) == 0) {
            isKeyDown = false;
        }

        if (!pause)
        {   
            //カーソル右
            if (Input.GetKeyDown(KeyCode.D) || axiskeymanager.GetHorizontalKeyDown(ref isKeyDown, (turnPlayer + 1).ToString()) == 1)
            {
                selectedHand = (++selectedHand) % handCount;
                return true;
            }
            //カーソル右
            else if (Input.GetKeyDown(KeyCode.A) || axiskeymanager.GetHorizontalKeyDown(ref isKeyDown, turnPlayer.ToString()) == -1)
            {
                --selectedHand;
                if (selectedHand == -1)
                {
                    selectedHand = handCount - 1;
                }
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit" + (turnPlayer + 1).ToString()))
            {
                if (turnEndButton)
                {
                    nextPlayer = true;
                }
                else
                {
                    decide = true;
                }

                return true;
            }
            else if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.S) ||
                    axiskeymanager.GetVerticalKeyDown(ref isKeyDown, (turnPlayer + 1).ToString()) == 1 ||
                    axiskeymanager.GetVerticalKeyDown(ref isKeyDown, (turnPlayer + 1).ToString()) == -1)
            {
                turnEndButton = !turnEndButton;
                Debug.Log(turnEndButton);
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause" + (turnPlayer + 1).ToString()))
            {
                pause = true;
                pauseSelected = 0;
                return true;
            }
        }
        //ポーズ中
        else
        {
            if (Input.GetKeyDown(KeyCode.W) || axiskeymanager.GetVerticalKeyDown(ref isKeyDown, (turnPlayer + 1).ToString()) == 1)
            {
                pauseSelected = (++pauseSelected) % numPauseText;
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.S) || axiskeymanager.GetVerticalKeyDown(ref isKeyDown, (turnPlayer + 1).ToString()) == -1)
            {
                --pauseSelected;
                if (pauseSelected == -1)
                {
                    pauseSelected = numPauseText - 1;
                }
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit" + (turnPlayer + 1).ToString())) 
            {
                decide = true;
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel" + (turnPlayer + 1).ToString()))
            {
                pause = false; 
                return true;
            }
        }
        return false;
}
    void InfoUpdate()
    {
        GameObject fieldPoint = GameObject.Find("FieldPoint");
        Text[] hands = { fieldPoint.transform.Find("hand1").GetComponent<Text>() ,
            fieldPoint.transform.Find("hand2").GetComponent<Text>() ,
        fieldPoint.transform.Find("hand3").GetComponent<Text>(),
        fieldPoint.transform.Find("hand4").GetComponent<Text>()
        };
        Text[] points = { fieldPoint.transform.Find("Point1").GetComponent<Text>() ,
            fieldPoint.transform.Find("Point2").GetComponent<Text>() ,
        fieldPoint.transform.Find("Point3").GetComponent<Text>(),
        fieldPoint.transform.Find("Point4").GetComponent<Text>()
        };
        fieldPoint.transform.Find("Cursor").GetComponent<RectTransform>().anchoredPosition = new Vector2(-334, 169.5f - 30.1f * (turnPlayer));

        for (int i = 0; i < playerNum; i++) {
            hands[i].text = Players[i].HandsList.Count+"";
            points[i].text= Players[i].NormalPoint+"";
        }

        GameObject playerInfo = GameObject.Find("PlayerInfo");
        playerInfo.transform.Find("Deck").GetComponent<Text>().text= Players[turnPlayer].DeckList.Count+"";
        playerInfo.transform.Find("Trash").GetComponent<Text>().text = Players[turnPlayer].TrashList.Count + "";
        Text buff = playerInfo.transform.Find("Buff").GetComponent<Text>();
        if (Player.SpecialPoint > 0)
        {
            buff.text = "+" + Player.SpecialPoint;
        }
        else if (Player.SpecialPoint < 0)
        {
            buff.text = "" + Player.SpecialPoint;
        }
        else
        {
            buff.text = "";
        }
        playerInfo.transform.Find("Point").GetComponent<Text>().text = Players[turnPlayer].NormalPoint + "";
       // playerInfo.transform.Find("Field").GetComponent<Text>().text = "場 "+Players[turnPlayer].FieldList.Count + "枚";
        //GameObject gameInfo = GameObject.Find("GameInfo");
        //Text gametext = gameInfo.transform.Find("Text").GetComponent<Text>();
        //gametext.text = (turn + 1) + "ターン目 " + (turnPlayer+1) + "Pの番";

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
            Card card = Players[turnPlayer].HandsList[i].GetComponent<Card>();
            if (card.Id >= 6 && card.Id <= 10)
            {
                switch (card.Id)
                {
                    case 6:
                        card.CalcNum = card.Num + Players[turnPlayer].DeckList.Count; break;
                    case 7:
                        card.CalcNum = card.Num + Players[turnPlayer].TrashList.Count; break;
                    case 9:
                        card.CalcNum = card.Num + Players[turnPlayer].HandsList.Count-1; break;
                    case 10:
                        card.CalcNum = card.Num + Players[turnPlayer].FieldList.Count; break;
                }
                Text num = Players[turnPlayer].HandsList[i].transform.Find("Card").transform.Find("Number").GetComponent<Text>(); ;
                num.text = card.CalcNum.ToString();
            }
            if (turnEndButton || i != selectedHand)
            {
                Players[turnPlayer].HandsList[i].transform.Find("Card").transform.Find("Arrow").GetComponent<Image>().enabled = false;
                Players[turnPlayer].HandsList[i].transform.Find("Card").transform.localScale = new Vector3(0.4f, 0.4f);
                
                if (i < selectedHand)
                {
                    Players[turnPlayer].HandsList[i].transform.SetSiblingIndex(i);
                }
                else
                {
                    Players[turnPlayer].HandsList[i].transform.SetSiblingIndex(i-1);
                }
            }
            else
            {
                Players[turnPlayer].HandsList[selectedHand].transform.Find("Card").transform.Find("Arrow").GetComponent<Image>().enabled = true;
                Players[turnPlayer].HandsList[selectedHand].transform.Find("Card").transform.localScale = new Vector3(0.45f, 0.45f);
                Players[turnPlayer].HandsList[selectedHand].transform.Find("Card").transform.SetAsLastSibling();
            }
        }
        if (turnEndButton)
        {
            GameObject.Find("TurnChange").transform.Find("Cursor").GetComponent<Image>().enabled = true;

        }
        else
        {
            GameObject.Find("TurnChange").transform.Find("Cursor").GetComponent<Image>().enabled = false;
        }
       
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
        Player.SpecialPoint+=i;
    }
    public void Handeath()
    {
        for(int i=0;i<Players.Count;i++)
        {
            if (i!=turnPlayer && Players[i].HandsList.Count >= 3)
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
            }
        Players[turnPlayer].NormalPoint += Players[turnPlayer].FieldList[Players[turnPlayer].FieldList.Count - 1].GetComponent<Card>().CalcNum;

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
        GameObject go = GameObject.Find("TurnChange");
        playing = go.transform.Find("Image" + (turnPlayer + 1)).GetComponent<Animator>();
        playing.SetTrigger("start");
        panel.GetComponent<Image>().enabled = true;
    }
    void TurnChange()
    {
        turnEndButton = false;
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
            for (int i = 0; i < playerNum; i++)
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

    }
}
