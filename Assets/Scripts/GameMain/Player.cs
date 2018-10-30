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
    [SerializeField]
    private GameObject Hands,Deck,Field,Trash,gameManager;
    private List<Sprite> miniCardSprites;
    private void Start()
    {
       
        
    }

    private void Awake()
    {
        Score = new List<int>();
        TotalScore = 0;
        NormalPoint = 0;
        SpecialPoint = 0;
        DeckList = new List<GameObject>();
        HandsList = new List<GameObject>();
        FieldList = new List<GameObject>();
        TrashList = new List<GameObject>();
        TurnPlayer = false;
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
            cloneObject.transform.parent = Deck.transform;
            Text num = cardObject.transform.Find("Number").GetComponent<Text>();
            num.text = card.Num.ToString();
            if (card.Id > 10 || card.Id < 6)
            {
                num.color = new Color(1, 1, 1);
            }
            Text name = cardObject.transform.Find("Name").GetComponent<Text>();
            name.text = Card.Names[card.Id];
            Text text = cardObject.transform.Find("Text").GetComponent<Text>();
            text.text = Card.Texts[card.Id];
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
            HandsUpdate();
        }
    }

    public void Draw()
    {
        if (DeckList.Count != 0)
        {
            
            DeckList[0].transform.parent = Hands.transform;
            if (TurnPlayer)
            {
                DeckList[0].transform.Find("Card").gameObject.SetActive(true);
                Debug.Log(DeckList[0].GetComponent<Card>().Num);
            }
            HandsList.Add(DeckList[0]);
            DeckList.RemoveAt(0);
            HandsUpdate();
        }
    }

    public void Play(int n)
    {
        HandsList[n].transform.parent = Field.transform;
        GameObject miniCard = HandsList[n].transform.Find("MiniCard").gameObject;
        miniCard.SetActive(true);
        miniCard.GetComponent<Image>().sprite = miniCardSprites[HandsList[n].GetComponent<Card>().CalcNum];
        miniCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(-159.4f+20*FieldList.Count-1, 167f - 29.5f * (Num-1));
        if (TurnPlayer)
        {
            HideHand(n);
        }
        FieldList.Add(HandsList[n]);
        HandsList.RemoveAt(n);
        HandsUpdate();

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
        HandsUpdate();
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
            HandsUpdate();  
        }
    }

    public void HandsUpdate()
    {
        if (HandsList.Count != 0)
        {
            float width = HandsList[0].transform.Find("Card").GetComponent<RectTransform>().sizeDelta.x;
            if (HandsList.Count <= 3)
            {
                for (int i = 0; i < HandsList.Count; i++)
                {
                    HandsList[i].transform.Find("Card").GetComponent<RectTransform>().anchoredPosition = new Vector2(82f + (width * 0.4f + 10) * i, -140f);
                }
            }
            else
            {
                
                for (int i = 0; i < HandsList.Count; i++)
                {
                    HandsList[i].transform.Find("Card").GetComponent<RectTransform>().anchoredPosition = new Vector2(82f + i * (width*0.4f+10)  * 3 / (HandsList.Count), -140f);
                    
                }
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
}

