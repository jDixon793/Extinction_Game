using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spinner : MonoBehaviour {
	public GameObject moveText;
	public GameObject moveTextBackground;
	public GameObject moveStartButton;
	public int[] moves;
	public float smoothMin;
	public float smoothMax;
	public float snapRate;
	public float randomSpeed;
	public float rate;
	public bool smoothStop;
	public bool hasMoveSet;
	public string spinText;

    bool spinning = false;
    public  GameStateMachine gameStateMachine;
    // Use this for initialization
    void Start () {
		rate = 120.0f;
		smoothMin=.15f;
		smoothMax=0.75f;
		snapRate = 20.0f;
		smoothStop = false;
		hasMoveSet=false;
	}
	
	// Update is called once per frame
	void Update () {
        if (spinning)
        {
            transform.Rotate(Vector3.back * Time.deltaTime * rate);
        }
		if(smoothStop)
		{
		
			if(rate<1)
			{
				rate =0;
				moveSet();
				smoothStop = false;
                spinning = false;
            }
			if(rate<20)
			{
				rate = Mathf.Lerp(rate, 0.0f, snapRate * Time.deltaTime);
			}
			else
			{
				rate = Mathf.Lerp(rate, 0.0f,  randomSpeed * Time.deltaTime);
			}
		}
		
	}
	public void moveSet() //return int[] of moves
	{
		string moveName = "";
		Debug.Log(transform.eulerAngles.z);
		int n=-1;
		int[] m = new int[2];
		//this just sees what multiple of 30 is closest to the end rotation
		int multThirty = 0;
		if((int)transform.eulerAngles.z!=0)
		{
			multThirty= (int)transform.eulerAngles.z/30;
		}
		if(Mathf.Abs((int)transform.eulerAngles.z-((multThirty)*30))<Mathf.Abs((int)transform.eulerAngles.z-((multThirty+1)*30)))
		{
			n=multThirty;
		}
		else
		{
			n=multThirty+1;
		}
		switch(n)
		{
			case 0:
				moveName = "Compete/Prey & Change Genes";
				Debug.Log("0: C/P & Genes");
				//we need to set the numbers to the numbers used in the book 0-reproduce 1- ev chg ect 
				m[0] = 4;
				m[1] = 5;
				break;
			case 1:
				moveName = "Reproduce & Relocate";
				Debug.Log("1:Reproduce & Relocate");
				m[0] = 0;
				m[1] = 3;
				break;
			case 2:
				moveName = "Environmental Change & Change Genes";
				Debug.Log("2:Change & Genes");
				m[0] = 1;
				m[1] = 5;
				break;
			case 3:
				moveName = "Compete/Prey & Place Barrier";
				Debug.Log("3:C/P & Barrier");
				m[0] = 4;
				m[1] = 2;
				break;
			case 4:
				moveName = "Place Barrier & Change Genes";
				Debug.Log("4:Barrier & Genes");
				m[0] = 2;
				m[1] = 5;
				break;
			case 5:
				moveName = "Reproduce & Compete/Prey";
				Debug.Log("5:Reproduce & Compete");
				m[0] = 0;
				m[1] = 4;
				break;
			case 6:
				moveName = "Environmental Change & Place Barrier";
				Debug.Log("6:Change & Barrier");
				m[0] = 1;
				m[1] = 2;
				break;
			case 7:
				moveName = "Reproduce & Relocate";
				Debug.Log("7:Reproduce & Relocate");
				m[0] = 0;
				m[1] = 3;
				break;
			case 8:
				moveName = "Place Barrier & Change Genes";
				Debug.Log("8:Barrier & genes");
				m[0] = 2;
				m[1] = 5;
				break;
			case 9:
				moveName = "Environmental Change & Change Genes";
				Debug.Log("9:Change & Genes");
				m[0] = 1;
				m[1] = 5;
				break;
			case 10:
				moveName = "Relocate & Compete/Prey";
				Debug.Log("10:Relocate & CompetePrey");
				m[0] = 3;
				m[1] = 4;
				break;
			case 11:
				moveName = "Reproduce & Relocate";
				Debug.Log("11:Reproduce & Relocate");
				m[0] = 0;
				m[1] = 3;
				break;
			case 12:
				moveName = "Compete/Prey & Change Genes";
				Debug.Log("12:Compete/prey & Genes");
				m[0] = 4;
				m[1] = 5;
				break;
			default:
				Debug.Log("Not Valid");
				break;
			
		}
		
		
		moveTextBackground.SetActive(true);
        moveText.GetComponent<Text>().text = moveName;
        gameStateMachine.moveSet = m;
		hasMoveSet = true;
		
	}
	public void SetRate(float r)
	{
		rate = r;
		smoothStop=false;
	}
	public void StopSpinner()
	{
		
		randomSpeed = 	Random.Range(smoothMin,smoothMax);
        
        smoothStop =true;
	}
    public void StartSpinner()
    {
        rate = 120;
        spinning = true;
    }

}
