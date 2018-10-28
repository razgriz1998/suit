using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteReader : MonoBehaviour {

    [SerializeField]
    private Sprite mini0, mini1, mini2, min3, mini4, mini5, mini6, mini7, mini8, mini9, mini10, mini11, mini12;
    public　List<Sprite> miniCardSprites{ get;private set;}
    // Use this for initialization
    void Start () {
        miniCardSprites = new List<Sprite> { mini0, mini1, mini2, min3, mini4, mini5, mini6, mini7, mini8, mini9, mini10, mini11, mini12 };
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
