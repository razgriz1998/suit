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
    public int SpecialPoint { get; set; }
    public List<int> Score { get; set; }
    public int TotalScore { get; set; }
    public List<GameObject> DeckList { get; private set; }
    public List<GameObject> HandsList { get; private set; }
    public List<GameObject> FieldList { get; private set; }
    public List<GameObject> TrashList { get; private set; }
    [SerializeField]
    private GameObject Hands,Deck,Field,Trash;
    [SerializeField]
    private Sprite drawSprite, inflationSprite, deflationSprite, shuffleSprite, cointssSprite,
        handeathSprite, deckcountSprite, trashcountSprite, vaniraSprite, handcountSprite, fieldcountSprite;
    private List<Sprite> cardSprites;
    
    private void Start()
    {
       
        
    }

    private void Awake()
    {
        Score = new List<int>();
        TotalScore = 0;
        NormalPoint = 0;
        SpecialPoint = 0;
        cardSprites = new List<Sprite> {drawSprite, inflationSprite, deflationSprite, shuffleSprite, cointssSprite,
        handeathSprite, deckcountSprite, trashcountSprite, vaniraSprite,handcountSprite,fieldcountSprite };
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
            cloneObject.GetComponent<Card>().Id = intDatas[0];
            cloneObject.GetComponent<Card>().Num = intDatas[1];
            cloneObject.transform.parent = Deck.transform;
            cloneObject.GetComponent<Image>().sprite = cardSprites[cloneObject.GetComponent<Card>().Id];
            Text num = cloneObject.transform.Find("Number").GetComponent<Text>();
            num.text = cloneObject.GetComponent<Card>().Num.ToString();
            if (cloneObject.GetComponent<Card>().Num >= 10)
            {
                num.rectTransform.sizeDelta = new Vector2(144.5f, 144.8f);
                num.rectTransform.anchoredPosition = new Vector2(0f, 86.7f);
            }
            else
            {
                num.rectTransform.sizeDelta = new Vector2(73.3f, 144.8f);
                num.rectTransform.anchoredPosition = new Vector2(0f, 86.7f);
            }
            cloneObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
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
                DeckList[0].SetActive(true);
            }
            HandsList.Add(DeckList[0]);
            DeckList.RemoveAt(0);
            HandsUpdate();
        }
    }

    public void Play(int n)
    {
        HandsList[n].transform.parent = Field.transform;
        if (TurnPlayer)
        {
            HandsList[n].SetActive(false);
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
            if (TurnPlayer)
            {
                go.SetActive(false);
            }
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
                HandsList[card].SetActive(false);
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
            if (HandsList.Count <= 3)
            {
                float width = HandsList[0].GetComponent<RectTransform>().sizeDelta.x;
                for (int i = 0; i < HandsList.Count; i++)
                {
                    HandsList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(288f + width * i, -312f);
                }
            }
            else
            {
                float width = HandsList[0].GetComponent<RectTransform>().sizeDelta.x;
                for (int i = 0; i < HandsList.Count; i++)
                {
                    HandsList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(288f + i * width * 3 / (HandsList.Count), -312f);
                    
                }
            }
        }
    }

    public void HideAllCards()
    {
        foreach(GameObject go in DeckList) {
            go.SetActive(false);
        }
        foreach (GameObject go in HandsList)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in FieldList)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in TrashList)
        {
            go.SetActive(false);
        }
    }
    public void HideHands()
    {
        foreach (GameObject go in HandsList)
        {
            go.SetActive(false);
        }
    }

    public void ShowHands()
    {

        foreach (GameObject go in HandsList)
        {
            go.SetActive(true);
        }
    }
}
