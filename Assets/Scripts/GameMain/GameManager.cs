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
    public bool TurnEndButton { get; private set; }
    private int turn = 0;
    private const int turnNum = 4;
    private bool gameEnd = false;
    private bool nextPlayer = false;
    private bool decide = false;
    public int[,] Score { get; private set; }
    private const int playerNum = 4;
    private int turnCount = 0;
    private Animator playing = null;
    private string playingName = null;
    private bool cardplaying = false;
    private float cardplayTime = 0f;
    private int frameCount=0;
    public bool Pause { get; private set; }
    public bool KeyInput { get; private set; }
    public int PauseSelected{ get; private set; }
    private const int numPauseText = 2;
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private AudioClip cancelAudio, cursolAudio, decisionAudio, cardPickAudio,turnChangeAudio;
    private AudioSource audioSource;
    [SerializeField]
    private Sprite drawSprite, inflationSprite, deflationSprite, shuffleSprite, cointssSprite,
        handeathSprite, deckcountSprite, trashcountSprite, vaniraSprite,handcountSprite,fieldcountSprite;
    private List<Sprite> cardSprites;
    [SerializeField]
    private string nextScene;

    [SerializeField]
    private ParticleSystem point_change_effect, inf_effect, def_effect;

    [SerializeField]
    private AudioClip point_change_Audio, inf_Audio, def_Audio;

    private bool isKeyDown;
    AxisKeyManager axiskeymanager;

    // Use this for initialization
    void Start () {
        TurnEndButton = false;
        cardSprites = new List<Sprite> {drawSprite, inflationSprite, deflationSprite, shuffleSprite, cointssSprite,
        handeathSprite, deckcountSprite, trashcountSprite, vaniraSprite ,handcountSprite,fieldcountSprite};
        Players = new List<Player>();
        Score = new int[playerNum,turnNum+1];
        for (int i = 0; i < playerNum; i++)
        {
            Players.Add(GameObject.Find("Player"+(i+1)).GetComponent<Player>());
        }
        audioSource = GetComponent<AudioSource>();
        turnPlayer = turn;
        //HandUpdate();
        Players[turnPlayer].ShowHands();
        ChangeTurnPlayer(false);
        HandUpdate();
        InfoUpdate();
        PauseSelected = 0;
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

        point_change_effect.Stop();
        inf_effect.Stop();
        def_effect.Stop();
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
        if (!gameEnd)
        {
            KeyInput = false;

            if (playing != null && !playing.GetCurrentAnimatorStateInfo(0).IsName("TurnChange")&&frameCount!=0 )
            {
                if (playingName == "PhaseChange")
                {
                    GameObject go = GameObject.Find("TurnChange");
                    playing = go.transform.Find("Image" + (turnPlayer + 1)).GetComponent<Animator>();
                    playing.SetTrigger("start");
                    playingName = "TurnChange";
                    panel.GetComponent<Image>().enabled = true;
                }
                else if (playingName == "TurnChange")
                {
                    playing = null;
                    panel.GetComponent<Image>().enabled = false;
                }
            }
            
            if (cardplaying)//カードの効果処理
            {
                PlayAnime();
            }
            if (!Pause)
            {
                if (!cardplaying)
                {

                    
                    if (playing == null && Key())
                    {
                        
                        KeyInput = true;
                        if (nextPlayer)
                        {
                            TurnEnd();
                        }
                        if (decide)
                        {
                            PlayAnime();
                        }
                        if (!gameEnd)
                        {
                            HandUpdate();
                        }
                        //HandUpdate();
                    }
                }
            }
            else if (Pause)
            {
                if (KeyInput = Key())
                {
                    if (decide)
                    {
                        switch (PauseSelected)
                        {
                            case 0: Pause = false;break;
                            case 1: DestroyThis(); SceneManager.LoadScene("title"); break;
                        }
                    }
                }
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

        int horizontal_value = axiskeymanager.GetHorizontalKeyDown(ref isKeyDown, (turnPlayer + 1).ToString());
        int vertical_value = axiskeymanager.GetVerticalKeyDown(ref isKeyDown, (turnPlayer + 1).ToString());

        if (!Pause)
        {   
            //カーソル右
            if (Input.GetKeyDown(KeyCode.D) || horizontal_value == 1)
            {
                audioSource.clip = cardPickAudio;
                audioSource.time = 0.3f;
                audioSource.Play();
                if (Players[turnPlayer].HandsList.Count != 0)
                {
                    selectedHand = (++selectedHand) % handCount;
                    return true;
                }
                return false;
            }
            //カーソル右
            else if (Input.GetKeyDown(KeyCode.A) || horizontal_value == -1)
            {
                playSE(cardPickAudio, 0.3f);
                if (Players[turnPlayer].HandsList.Count != 0)
                {
                    --selectedHand;
                    if (selectedHand == -1)
                    {
                        selectedHand = handCount - 1;
                    }
                    return true;
                }
                return false;
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit" + (turnPlayer + 1).ToString()))
            {
                playSE(cardPickAudio, 0.3f);
                if (TurnEndButton)
                {
                    nextPlayer = true;
                }
                else
                {
                    decide = true;
                }

                return true;
            }
            else if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.S) || vertical_value == 1 || vertical_value == -1)
            {
                playSE(cardPickAudio, 0.3f);
                if (Players[turnPlayer].HandsList.Count != 0)
                {
                    TurnEndButton = !TurnEndButton;
                    return true;
                }
                return false;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause" + (turnPlayer + 1).ToString()))
            {
                audioSource.PlayOneShot(cancelAudio);
                Pause = true;
                PauseSelected = 0;
                return true;
            }
        }
        //ポーズ中
        else
        {
            if (Input.GetKeyDown(KeyCode.W) || vertical_value == 1)
            {
                audioSource.PlayOneShot(cursolAudio);
                PauseSelected = (++PauseSelected) % numPauseText;
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.S) || vertical_value == -1)
            {
                audioSource.PlayOneShot(cursolAudio);
                --PauseSelected;
                if (PauseSelected == -1)
                {
                    PauseSelected = numPauseText - 1;
                }
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit" + (turnPlayer + 1).ToString())) 
            {
                audioSource.PlayOneShot(decisionAudio);
                decide = true;
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel" + (turnPlayer + 1).ToString()))
            {
                audioSource.PlayOneShot(cancelAudio);
                Pause = false; 
                return true;
            }
        }
        if (Input.GetKeyDown(KeyCode.End))
        {
            SceneManager.LoadScene(nextScene);
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
            if (TurnEndButton || i != selectedHand)
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
                Transform tf = GetSelectedCard().transform.Find("Card");
                tf.transform.Find("Arrow").GetComponent<Image>().enabled = true;
                tf.transform.localScale = new Vector3(0.45f, 0.45f);
                tf.transform.SetAsLastSibling();
            }
        }
        if (TurnEndButton)
        {
            GameObject.Find("TurnEndButton").transform.Find("Cursor").GetComponent<Image>().enabled = true;
        }
        else
        {
            GameObject.Find("TurnEndButton").transform.Find("Cursor").GetComponent<Image>().enabled = false;
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

    public void PlayAnime()
    {

        if (!cardplaying)
        {
            cardplaying = true;
            cardplayTime = Time.time;

        }
        else
        {
            float target = 300f;
            if (GetSelectedCard())
            {
                RectTransform rt = GetSelectedCard().transform.Find("Card").GetComponent<RectTransform>();
                float newPosition = Mathf.SmoothStep(-140f,
                             target, (Time.time - cardplayTime) * 1.3f);
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, newPosition);
                GetSelectedCard().transform.Find("Card").GetComponent<Image>().color = new Color(1, 1, 1, 1 - (Time.time - cardplayTime) * 1.3f);
                if (newPosition == target)
                {
                    cardplaying = false;
                    KeyInput = true;
                    Play();
                }
            }
        }
    }

    public void Play()
    {
        KeyInput = true;
        int id = GetSelectedCard().GetComponent<Card>().Id;
        Players[turnPlayer].Play(selectedHand);
        switch (id)
        {
            case 0: Players[turnPlayer].Draw(); break;//ドロー
            case 1:
                Players[turnPlayer].Draw();//インフレ
                inf_effect.Play();
                audioSource.PlayOneShot(inf_Audio);
                AllBuff(1);
                break;
            case 2:
                Players[turnPlayer].Draw();//デフレ
                def_effect.Play();
                audioSource.PlayOneShot(def_Audio);
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
                    inf_effect.Play();
                    audioSource.PlayOneShot(inf_Audio);
                }
                else
                {
                    AllBuff(-2);
                    def_effect.Play();
                    audioSource.PlayOneShot(def_Audio);
                }
                break;
            case 5: Handeath(); break;//ハンデス
        }

        //得点が変わったとき
        if (Players[turnPlayer].FieldList[Players[turnPlayer].FieldList.Count - 1].GetComponent<Card>().CalcNum != 0) 
        {
            point_change_effect.Play();
            audioSource.PlayOneShot(point_change_Audio);
            Players[turnPlayer].NormalPoint += Players[turnPlayer].FieldList[Players[turnPlayer].FieldList.Count - 1].GetComponent<Card>().CalcNum;
        }

        if (Players[turnPlayer].HandsList.Count == 0) 
        {
            TurnEndButton = true;
        }
        selectedHand = 0;
        InfoUpdate();
        HandUpdate();

    }

    public void ChangeTurnPlayer(bool phaseChange)
    {
        foreach (Player p in Players)
        {
            p.TurnPlayer = false;
        }
        if (turnPlayer >= 0 && turnPlayer < Players.Count) {
            Players[turnPlayer].TurnPlayer = true;
        }
        if (phaseChange)
        {
            GameObject go = GameObject.Find("TurnChange");
            playing = go.transform.Find("Image5").GetComponent<Animator>();
            playing.SetTrigger("start");
            playingName = "PhaseChange";
            panel.GetComponent<Image>().enabled = true;
        }
        else {
            GameObject go = GameObject.Find("TurnChange");
            playing = go.transform.Find("Image" + (turnPlayer + 1)).GetComponent<Animator>();
            playing.SetTrigger("start");
            playingName = "TurnChange";
            panel.GetComponent<Image>().enabled = true;
        }
    }
    void TurnChange()
    {
        TurnEndButton = false;
        ScoreUpdate();
        turnCount = 0;
        turn++;
        turnPlayer = turn;
        ChangeTurnPlayer(true);
        foreach (Player p in Players)
        {
            p.TurnChange();
        }
    }

    void TurnEnd()
    {
        turnCount++;
        TurnEndButton = false;
        selectedHand = 0;
        Players[turnPlayer].HideHands();
        turnPlayer = (turnPlayer + 1) % Players.Count;

        if (Players[turnPlayer].HandsList.Count == 0)
        {
            TurnEndButton = true;
        }
        HandUpdate();

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
                gameEnd = true;
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                Players[turnPlayer].ShowHands();

            }
        }
        else
        {
            Players[turnPlayer].ShowHands();
            ChangeTurnPlayer(false);
        }
        if (!gameEnd)
        {
            InfoUpdate();
            playSE(turnChangeAudio);
        }

    }

    public GameObject GetSelectedCard()
    {
        if (TurnEndButton || Players[turnPlayer].HandsList.Count==0)
        {
            return null;
        }
        return Players[turnPlayer].HandsList[selectedHand];
    }

    public void playSE(AudioClip ac,float s)
    {
        audioSource.clip = ac;
        audioSource.time = s;
        audioSource.Play();
    }
    public void playSE(AudioClip ac)
    {
        audioSource.PlayOneShot(ac);
    }
}
