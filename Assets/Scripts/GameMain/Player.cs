using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Player:MonoBehaviour
{
    [SerializeField]
    private int Num;
    [SerializeField]
    private int NumDeck;
    public bool TurnPlayer { get; set; }
    public bool EndThisTurn { get; set; }
    public int NormalPoint { get; set; }
    static public int SpecialPoint { get; set; }
    public List<int> Score { get; set; }
    public int TotalScore { get; set; }
    public List<GameObject> DeckList { get; private set; }
    public List<GameObject> HandsList { get; private set; }
    public List<GameObject> FieldList { get; private set; }
    public List<GameObject> TrashList { get; private set; }
    public bool drawing { get; private set; }//ドローのアニメーション中かどうか
    private float drawTime = 0f;
    private List<float> handsStartPos=new List<float>();//ドローアニメーション開始位置の記録
    [SerializeField]
    private GameObject Hands,Deck,Field,Trash,gameManager;
    private List<Sprite> miniCardSprites, miniCardSprites_i, miniCardSprites_d;
    private void Start()
    {
    }

    private void Awake()
    {
        if (GameState.Instance.isSetDeck == 4) {

            NumDeck = GameState.Instance.deck_type[Num - 1];
        }
        else {
            if (Num == 1 || Num == 3) {
                NumDeck = 0;
            }
            else {
                NumDeck = 1;
            }
        }
        Score = new List<int>();
        TotalScore = 0;
        NormalPoint = 0;
        SpecialPoint = 0;
        DeckList = new List<GameObject>();
        HandsList = new List<GameObject>();
        FieldList = new List<GameObject>();
        TrashList = new List<GameObject>();
        TurnPlayer = false;
        drawing = false;
        TextAsset csvFile = Resources.Load("DeckLists/Deck" + NumDeck) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] datas = line.Split(','); // リストに入れる
            List<int> intDatas = new List<int>();
            foreach (string data in datas)
            {
                intDatas.Add(int.Parse(data));
            }
            GameObject prefab = (GameObject)Resources.Load("Prefabs/GameMain/Card");
            GameObject cloneObject = Instantiate(prefab, this.transform.position, Quaternion.identity);
            GameObject cardObject = cloneObject.transform.Find("Card").gameObject;
            Card card = cloneObject.GetComponent<Card>();
            card.Id = intDatas[0];
            card.Num = intDatas[1];
            card.CalcNum = intDatas[1];
            if (card.Id != 1 && card.Id != 2)
            {
                card.Buf = 0;
            }
            else if(card.Id == 1)
            {
                card.Buf = 1;
            }
            else
            {
                card.Buf = -1;
            }

            card.Count = card.GetThisName().Contains("カウント");
            cloneObject.transform.parent = Deck.transform;
            Text num = cardObject.transform.Find("Number").GetComponent<Text>();
            if (card.Buf != 0)
            {
                if (card.Buf > 0)
                {
                    num.text = "+"+card.Buf.ToString();
                }
                else
                {
                    num.text = card.Buf.ToString();
                }
            }
            else
            {
                num.text = card.Num.ToString();
            }
            if (card.Id > 10 || card.Id < 6)
            {
                num.color = new Color(1, 1, 1);
            }
            Text name = cardObject.transform.Find("Name").GetComponent<Text>();
            name.text = card.GetThisName();
            Text text = cardObject.transform.Find("Text").GetComponent<Text>();
            text.text = card.GetThisText();

            /*if (cloneObject.GetComponent<Card>().Num >= 10)
            {
                num.rectTransform.sizeDelta = new Vector2(148.5f, 145.8f);
                num.rectTransform.anchoredPosition = new Vector2(0f, 86.7f);
            }
            else
            {
                num.rectTransform.sizeDelta = new Vector2(75.3f, 145.8f);
                num.rectTransform.anchoredPosition = new Vector2(0f, 86.7f);
            }*/
            cardObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(494, -140);
            cardObject.GetComponent<RectTransform>().localScale = new Vector3(0.45f, 0.45f, 0.45f);
            //cloneObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            //cloneObject.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            DeckList.Add(cloneObject);

        }
        DeckShuffle();
        Draw();
        Draw();
        Draw();
        HideAllCards();
    }

    private void Update()
    {
        if (miniCardSprites == null)
        {
            miniCardSprites = gameManager.GetComponent<SpriteReader>().miniCardSprites;
        }
        if (miniCardSprites_i == null)
        {
            miniCardSprites_i = gameManager.GetComponent<SpriteReader>().miniCardSprites_i;
        }
        if (miniCardSprites_d == null)
        {
            miniCardSprites_d = gameManager.GetComponent<SpriteReader>().miniCardSprites_d;
        }
        if (drawing)
        {
            
            HandsUpdateAnime();
        }
    }

    public void DeckShuffle()
    {
        if (DeckList.Count > 1)
        {
            GameObject myGo;

            int n = DeckList.Count;
            for (int i = 0; i < n; i++)
            {
                int r = Random.Range(i,n);
                myGo = DeckList[r];
                DeckList[r] = DeckList[i];
                DeckList[i] = myGo;
            }
            if (TurnPlayer)
            {
                HandsAnimeStart();
            }
            else
            {
                HandsUpdate();
            }
        }
    }

    public void Draw()
    {
        if (DeckList.Count != 0)
        {

            DeckList[0].transform.Find("Card").GetComponent<RectTransform>().anchoredPosition = new Vector2(494, -140);
            DeckList[0].transform.parent = Hands.transform;
            if (TurnPlayer)
            {
                DeckList[0].transform.Find("Card").gameObject.SetActive(true);
            }
            
            HandsList.Add(DeckList[0]);

            
            DeckList.RemoveAt(0);
            if (TurnPlayer)
            {
                HandsAnimeStart();
            }
            else
            {
                HandsUpdate();
            }
        }
    }

    public void Play(int n)
    {
        HandsList[n].transform.parent = Field.transform;
        GameObject miniCard = HandsList[n].transform.Find("MiniCard").gameObject;
        miniCard.SetActive(true);
        /*if (HandsList[n].GetComponent<Card>().Buf > 0)
        {
            miniCard.GetComponent<Image>().sprite = miniCardSprites_i[HandsList[n].GetComponent<Card>().Buf];
        }
        else if (HandsList[n].GetComponent<Card>().Buf < 0)
        {
            miniCard.GetComponent<Image>().sprite = miniCardSprites_d[-HandsList[n].GetComponent<Card>().Buf];
        }
        else
        {*/
        if (HandsList[n].GetComponent<Card>().CalcNum < miniCardSprites.Count)
        {
            miniCard.GetComponent<Image>().sprite = miniCardSprites[HandsList[n].GetComponent<Card>().CalcNum];
        }
        else
        {
            miniCard.GetComponent<Image>().sprite = miniCardSprites[miniCardSprites.Count-1];
        }
        //}
        int space=20;
        int maxMini = 5;
        if (FieldList.Count < maxMini)
        {
            miniCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-159.4f + space * FieldList.Count, 167f - 29.5f * (Num - 1));
        }
        else
        {
            int x = space * (maxMini - 1) / (FieldList.Count - 1) - space * (maxMini - 1) / FieldList.Count;
            miniCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-159.4f + space * (maxMini-1), 167f - 29.5f * (Num - 1));
            for(int i=0;i< FieldList.Count; i++)
            {
                RectTransform rt = FieldList[i].transform.Find("MiniCard").GetComponent<RectTransform>();
                rt.anchoredPosition -= new Vector2((float)i*x, 0);
            }
        }
        if (TurnPlayer)
        {
            HideHand(n);
        }
        FieldList.Add(HandsList[n]);
        HandsList.RemoveAt(n);
        if (TurnPlayer)
        {
            HandsAnimeStart();
        }
        else
        {
            HandsUpdate();
        }

        EndThisTurn = true;
    }

    public void TurnChange()
    {
        foreach(GameObject go in FieldList)
        {
            go.transform.parent = Trash.transform;
            go.transform.Find("MiniCard").gameObject.SetActive(false);
        }
        TrashList.AddRange(FieldList);
        FieldList.Clear();
        Draw();
        NormalPoint = 0;
        SpecialPoint = 0;
        if (TurnPlayer)
        {
            HandsAnimeStart();
        }
        else
        {
            HandsUpdate();
        }
        EndThisTurn = false;
    }

    public void ScoreUpdate()
    {
        Score.Add(System.Math.Abs(-10 + NormalPoint + SpecialPoint));
        TotalScore += System.Math.Abs(-10 + NormalPoint + SpecialPoint);
    }

    public void AllHandsToDeck()
    {
        foreach(GameObject go in HandsList)
        {
            go.transform.parent = Deck.transform;
            go.transform.Find("Card").GetComponent<RectTransform>().anchoredPosition = new Vector2(494, -140);
        }

        if (TurnPlayer)
        {
            HideHands();
        }
        DeckList.AddRange(HandsList);
        HandsList.Clear();
    }

    public void Handeath()
    {
        if (HandsList.Count != 0)
        { 
            int card = Random.Range(0, HandsList.Count);
            HandsList[card].transform.parent = Deck.transform;
            if (TurnPlayer)
            {
                HideHand(card);
            }
            DeckList.Add(HandsList[card]);
            DeckShuffle();
            HandsList.RemoveAt(card);
            if (TurnPlayer)
            {
                HandsAnimeStart();
            }
            else
            {
                HandsUpdate();
            }
        }
    }

    public void HandsAnimeStart()
    {
        drawTime = Time.time;
        drawing = true;
        handsStartPos.Clear();
        for (int i = 0; i < HandsList.Count; i++)
        {
            Debug.Log(HandsList[i].transform.Find("Card").GetComponent<RectTransform>().anchoredPosition.x);
            handsStartPos.Add(HandsList[i].transform.Find("Card").GetComponent<RectTransform>().anchoredPosition.x);
        }
    }

    public void HandsUpdateAnime()
    {
        if (HandsList.Count != 0)
        {
            float width = HandsList[0].transform.Find("Card").GetComponent<RectTransform>().sizeDelta.x;
                drawing = false;

                for (int i = 0; i < HandsList.Count; i++)
                {
                    float target;
                    if (HandsList.Count <= 3)
                    {
                        target = 82f + (width * 0.4f + 10) * i;
                    }
                    else
                    {
                        target = 82f + i * (width * 0.4f + 10) * 3 / (HandsList.Count);
                    }
                    if (handsStartPos.Count <= i)
                    {
                    
                    handsStartPos.Add(HandsList[i].transform.Find("Card").GetComponent<RectTransform>().anchoredPosition.x);
                    }
                    float newPosition = Mathf.SmoothStep(handsStartPos[i],
                         target, (Time.time - drawTime) * 1.3f);
                    HandsList[i].transform.Find("Card").GetComponent<RectTransform>().anchoredPosition = new Vector2(newPosition, -140f);

                    if (target != newPosition)
                    {
                        drawing = true;
                    }
                }
            
        }
    }

    public void HandsUpdate()
    {
        if (HandsList.Count != 0)
        {
            float width = HandsList[0].transform.Find("Card").GetComponent<RectTransform>().sizeDelta.x;
            for (int i = 0; i < HandsList.Count; i++)
            {
                float target;
                if (HandsList.Count <= 3)
                {
                    target = 82f + (width * 0.4f + 10) * i;
                }
                else
                {
                    target = 82f + i * (width * 0.4f + 10) * 3 / (HandsList.Count);
                }
                HandsList[i].transform.Find("Card").GetComponent<RectTransform>().anchoredPosition = new Vector2(target, -140f);
            }
        }
    }

    public void HideAllCards()
    {
        foreach(GameObject go in DeckList) {
            go.transform.Find("Card").gameObject.SetActive(false);
        }
        foreach (GameObject go in HandsList)
        {
            go.transform.Find("Card").gameObject.SetActive(false);
        }
        foreach (GameObject go in FieldList)
        {
            go.transform.Find("Card").gameObject.SetActive(false);
        }
        foreach (GameObject go in TrashList)
        {
            go.transform.Find("Card").gameObject.SetActive(false);
        }
    }
    public void HideHands()
    {
        foreach (GameObject go in HandsList)
        {
            go.transform.Find("Card").gameObject.SetActive(false);
        }
    }

    public void HideHand(int n)
    {
        HandsList[n].transform.Find("Card").gameObject.SetActive(false);
    }

    public void ShowHands()
    {

        foreach (GameObject go in HandsList)
        {
            go.transform.Find("Card").gameObject.SetActive(true);
        }
    }

    /*public void setBuf(int b)
    {
        FieldList[FieldList.Count-1].GetComponent<Card>().Buf += b;
        if (b > 0)
        {
            FieldList[FieldList.Count - 1].transform.Find("MiniCard").GetComponent<Image>().sprite = miniCardSprites_i[b];
        }
        else if (b < 0)
        {
            FieldList[FieldList.Count - 1].transform.Find("MiniCard").GetComponent<Image>().sprite = miniCardSprites_i[-b];
        }
    }
    */
}

