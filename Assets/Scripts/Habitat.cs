using UnityEngine;
using System.Collections;
using System;

public class Habitat : MonoBehaviour {
	public int habitat;
	public GameObject[] touching;
	public GameObject dice;
	public Color diceColor;
	public int playerOwned;
	public Sprite[] numbers;
	public bool selected;
	public int numAnimals;
	public bool isBarrier;
	public int barrierType;
	object loadedObject;
	Sprite spriteSelected;
	Sprite habitatSprite;
	string selectedSpriteName;
    public TextMesh displayNum;
    public Player owner;
    float fadeAlpha;

    // Use this for initialization
    void Start () {
		isBarrier=false;
		barrierType = -1;
		playerOwned=-1;
		numAnimals=0;
		habitatSprite = GetComponent<SpriteRenderer>().sprite;
		selectedSpriteName= GetComponent<SpriteRenderer>().sprite.name+"_Selected";
		loadedObject = Resources.Load<Sprite>(selectedSpriteName);
		spriteSelected = (Sprite)loadedObject;
        displayNum.gameObject.SetActive(false);
        dice.gameObject.SetActive(false);
        owner = null;
        fadeAlpha = 0.2f;
    }
	
	// Update is called once per frame
	void Update () {
		if(selected)
		{
			GetComponent<SpriteRenderer>().sprite = spriteSelected;
		}
		else if(!selected&&GetComponent<SpriteRenderer>().sprite.name==spriteSelected.name)
		{
			GetComponent<SpriteRenderer>().sprite = habitatSprite;
		}
		
		if(numAnimals>0&&(Int32.Parse(displayNum.text)!=numAnimals)|| dice.GetComponent<SpriteRenderer>().color != diceColor)
		{
            displayNum.gameObject.SetActive(true);
            dice.gameObject.SetActive(true);
            displayNum.text = numAnimals.ToString();
			dice.GetComponent<SpriteRenderer>().color = diceColor; 
			dice.GetComponent<SpriteRenderer>().enabled=true;
			
		}
		if(numAnimals<=0 && (displayNum.gameObject.activeSelf || dice.gameObject.activeSelf))
		{
            displayNum.gameObject.SetActive(false);
            dice.gameObject.SetActive(false);
            owner = null;
        }
	}

    public void AddAnimals(int num, Player p)
    {
        if (numAnimals < 9)
        {
            numAnimals += num;
            owner = p;
            owner.numAnimals += num;
            diceColor = p.color;
        }
    }
    public void RemoveAnimals(int num, Player p)
    {
        if (numAnimals > 0)
        {
            numAnimals -= num;
            owner = p;
            owner.numAnimals -= num;
            diceColor = p.color;
        }
    }
    public void FadeOut()
    {
        GameObject g = this.gameObject;
        g.GetComponent<SpriteRenderer>().color = new Color(g.GetComponent<SpriteRenderer>().color.r, g.GetComponent<SpriteRenderer>().color.g, g.GetComponent<SpriteRenderer>().color.b, fadeAlpha);
        displayNum.color = new Color(displayNum.color.r, displayNum.color.g, displayNum.color.b, fadeAlpha);
        diceColor = new Color(diceColor.r, diceColor.g, diceColor.b, fadeAlpha);

    }
    public void FadeIn()
    {
        GameObject g = this.gameObject;
        g.GetComponent<SpriteRenderer>().color = new Color(g.GetComponent<SpriteRenderer>().color.r, g.GetComponent<SpriteRenderer>().color.g, g.GetComponent<SpriteRenderer>().color.b, 1);
        displayNum.color = new Color(displayNum.color.r, displayNum.color.g, displayNum.color.b, 1);
        diceColor = new Color(diceColor.r, diceColor.g, diceColor.b, 1);
    }

}



			