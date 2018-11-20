using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextGetter : MonoBehaviour {
    [SerializeField]
    string mode;
    private Text text;
    GameManager gm;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        gm = GameManager.Instance;

    }

	// Update is called once per frame
	void Update () {
        if (gm.KeyInput||text.text=="")
        {
            switch (mode)
            {
                case "text":
                    text.text = gm.GetSelectedCard().GetComponent<Card>().GetThisText();
                    break;
                case "num":
                    text.text = gm.GetSelectedCard().transform.Find("Card").Find("Number").GetComponent<Text>().text;
                    if (gm.GetSelectedCard().GetComponent<Card>().Count)
                    {
                        text.color= new Color(0.3389575f, 0.8113208f, 0.2104842f);
                    }
                    else
                    {
                        text.color = new Color(1, 1, 1);
                    }
                    break;
                case "name":
                    text.text = gm.GetSelectedCard().GetComponent<Card>().GetThisName();
                    break;
            }
        }
	}
}
