using UnityEngine;
using System.Collections;

[System.Serializable]
public class Player : MonoBehaviour {

    public Color color;
    public string pName;
    public int numAnimals;
    public float minRate;
    public float maxRate;
    public float rate;
    public int mobility;
    public int predator;
    public int[] crossing;
    public int[] habitat;
    public int[] defense;
    public int[] tolerance;
    enum environment {marsh,swamp,brush,forest,meadow,lake };



    //constructor
    public Player(string n,Color c)
    {
        pName = n;
        color = c;
    }
    // Use this for initialization
    public void Start () 
	{
		//change the random generation to use weighted values to balance the game
		//if the random number is between 0 and num1=20
		//if the random number is between num1 and num1+num2
		//...
		//all the nums should add up to 100 to keep things pretty
		numAnimals = 0;
		minRate=0.6f;
		maxRate=1.6f;
		
		GeneratePredator();
		GenerateRate();
		GenerateHabitatMobility();
		GenerateDefense();
		GenerateCrossing();
		GenerateTolerance();
		
	}
	
	public bool Contains(int[] array,int num)
	{
		for(int i=0;i<array.Length;i++)
		{
			if(array[i]==num)
				return true;
		}
		return false;
	}
	public void GenerateRate()
	{
		rate = Random.Range(minRate,maxRate);
	}
	public void GeneratePredator()
	{
		predator = Random.Range(0,6);
	}
	public void GenerateDefense()
	{
		defense = new int[Random.Range(2,4)];
		for(int i =0;i<defense.Length;i++)
		{
			int num = Random.Range(0,5);
			while(Contains(defense,num))
			{
				num = Random.Range(0,5);
			}
			defense[i]= num;
		}
	}
	public void GenerateCrossing()
	{
		crossing = new int[Random.Range(1,3)];
		for(int i =0;i<crossing.Length;i++)
		{
			int num = Random.Range(0,5);
			while(Contains(crossing,num))
			{
				num = Random.Range(0,5);
			}
			crossing[i]= num;
		}
	}
	public void GenerateTolerance()
	{
		tolerance = new int[Random.Range(1,3)];
		for(int i =0;i<tolerance.Length;i++)
		{
			int num = Random.Range(0,5);
			while(Contains(tolerance,num))
			{
				num = Random.Range(0,5);
			}
			tolerance[i]= num;
		}
	}
	public void GenerateHabitatMobility()
	{
		int mobilitySwitch = Random.Range(0,3);
		switch(mobilitySwitch)
		{
			case 0:
				mobility = 0;
				break;
			case 1:
				mobility = 20;
				break;
			case 2:
				mobility = 40;
				break;
		}
		habitat = new int[3-mobilitySwitch];
		for(int i =0;i<habitat.Length;i++)
		{
			int num = Random.Range(0,6);
			while(Contains(habitat,num))
			{
				num = Random.Range(0,6);
			}
			habitat[i]= num;
		}
	}

    //determines if the player can cross the selected habitat
    public bool canCross(Habitat h)
    {
        bool canCross = false;
        if (h.isBarrier)
        {
            for (int i = 0; i < crossing.Length; i++)
            {
                if (crossing[i] == h.barrierType)
                {
                    canCross = true;
                }
            }
        }
        else
        {
            canCross = false;
        }
        return canCross;
    }

}
