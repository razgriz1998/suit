using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteReader : MonoBehaviour {

    [SerializeField]
    private Sprite mini0, mini1, mini2, min3, mini4, mini5, mini6, mini7, mini8, mini9, mini10, mini11, mini12,mini13,mini14,mini15,mini16,mini17, mini_d1, mini_d2,mini_i1,mini_i2;
    public　List<Sprite> miniCardSprites{ get;private set;}
    public List<Sprite> miniCardSprites_d { get; private set; }
    public List<Sprite> miniCardSprites_i { get; private set; }

    // Use this for initialization
    void Start () {
        miniCardSprites = new List<Sprite> { mini0, mini1, mini2, min3, mini4, mini5, mini6, mini7, mini8, mini9, mini10, mini11, mini12, mini13, mini14, mini15, mini16, mini17 };
        miniCardSprites_d = new List<Sprite> {null, mini_d1, mini_d2 };
        miniCardSprites_i = new List<Sprite> {null, mini_i1, mini_i2 };
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
