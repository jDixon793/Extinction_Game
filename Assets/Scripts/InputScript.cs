using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InputScript : MonoBehaviour {

	Object loadedObject;
	Sprite spriteObject;
	SpriteRenderer spriteRenderer;
	public GameObject GUInum;
	public GameObject GUIonesLeft;
	public GameObject GUItensLeft;
	public GameObject turnChangeText;
	public Sprite[] numbers;
	public GameObject[] players;
	public GameObject[] barGraph;
	public GameObject[] BarrierText;
	public GameObject[] BarrierGUI;
	public GameObject[] BarrierButton;
	public int[] animalsPerPlayer;
	public int[] numBarriers=  {2,2,2,2,2};
	Player currentPlayer;
	public GameObject dice;
	public GameObject[] darwinia;
	public GameObject[] buttons;
	public int numPlayers;
	//GUI controls
	public GameObject AttackButton;
	public GameObject BarrierPlace;
	public GameObject DonePlace;
	public GameObject geneChangeGUI;
	public GameObject doneButton;
	public GameObject selection;
	public GameObject placingControls;
	public GameObject moveDescription;
	public GameObject spinner;
	public GameObject moveAnimals;
	public GameObject turnSwitch;
	public GameObject playerName;
	public GameObject DNAbutton;
	public GameObject habiatStat;
	public GameObject reproductionStat;
	public GameObject crossingStat;
	public GameObject defenseStat;
	public GameObject predatorStat;
	public GameObject toleranceStat;
	public GameObject habiatStatC;
	public GameObject reproductionStatC;
	public GameObject crossingStatC;
	public GameObject defenseStatC;
	public GameObject predatorStatC;
	public GameObject toleranceStatC;
	public GameObject moveDescriptionText;
	public GameObject geneLeftText;
	public List<GameObject> valid;
	public List<GameObject> fadeValid;
	public GameObject fromHabitat;
	public GameObject[] names;
	public GameObject[] nums;
	public GameObject lostScreen;
	public Spinner spinInfo;
	public bool placingAnimals;
	public bool isPaused;
	public int numBabys;
	public int startNumAnimals;
	int geneChangeLeft;
	public float fadeAlpha;
	int numMoves;
	public int moveNum;
	public int rounds;
	bool touched;
	public bool doneWithMoves;
	public bool fadeModeStarted;
	bool firstTurn;
	bool attackButtonPushed;
	bool relocating;
	bool reproducing;
	bool killing;
	bool startTurn;
	bool moveStarted;
	bool doneWithCurrentMove;
	bool doneClicked;
	bool faded;
	bool goAgain;
	public bool isSubscreen;
	bool subscreenClicked;
	public int overPop;
	public bool turnOver;
	int curNum;
	int envCard;
	int barrierType;
	public GameObject lastGUI;
	Vector3 fwd;
	RaycastHit2D hit;
	Vector3 worldPoint;

	// initialization
	void Awake () 
	{
		attackButtonPushed=false;
		subscreenClicked = false;
		isSubscreen=false;
		envCard = -1;
		barrierType=-1;
		overPop = 7;
		goAgain = false;
		faded = false;
		doneClicked=false;
		moveStarted=false;
		relocating=false;
		reproducing = false;
		killing = false;
		fwd = transform.TransformDirection(Vector3.forward);
		moveNum=0;
		geneChangeLeft=4;
		isPaused=false;
		doneWithMoves=false;
		lastGUI=null;
		numMoves =0;
		valid = new List<GameObject>();
		fadeModeStarted = false;
		fadeAlpha = 0.2f;
		curNum =0;
		turnOver = false;
		placingAnimals=false;
		touched=false;
		firstTurn = true;
		numbers = new Sprite[10];
		players = new GameObject[5];
		darwinia = new GameObject[84];

		numbers = Resources.LoadAll<Sprite>("numbers"); 
		DNAbutton.SetActive(false);
		int num =1;
		GameObject player = GameObject.Find("player"+num);
		while(player!=null)
		{
			players[num-1] = player;
			num++;
			player = GameObject.Find("player"+num);
		}
		for(int i=1;i<=84;i++)
		{
			darwinia[i-1] =  GameObject.Find("Habitat"+i);
		}
		numPlayers = num-1;
		animalsPerPlayer= new int[numPlayers];
		currentPlayer =  (players[curNum]).GetComponent<Player>();
		//turnChangeText.GetComponent<Text>().text = "It's "+ currentPlayer.pName+"'s turn";
		playerName.GetComponent<Text>().text =currentPlayer.pName;
		playerName.GetComponent<Text>().color =currentPlayer.color;
		//Start the screen for first player
		
		turnSwitch.SetActive(true);
		DNAbutton.SetActive(false);
		
		
		
		

	}
	
	// Update is called once per frame
	void Update () 
	{
		//pause the game
		if(isPaused)
		{	
			//if the game is paused do nothing in update
			return;
		}
		
		//shows the appropriate UI for the current turn
		if(startTurn)
		{
			//first turn
			if(rounds==0)
			{
				//show the place animal screen w/o DNA
				//moveDescription.SetActive(true);
				moveDescriptionText.GetComponent<Text>().text= "Place 30 animals in Darwinia";
				DNAbutton.SetActive(false);
			}
			
			//every other round
			else if(rounds>=1)
			{
				//this is where you will show the spinner UI
				//when you have a value for the current move... a Vector2 or array..
				//set the text to that move
				//when the move is finished increment moveNum
				DNAbutton.SetActive(true);
				spinner.SetActive(true);
			}
			
			//reset trigger
			startTurn=false;
		}
		
		//Ends the currents players turn
		if(turnOver)
		{
			//if the player gets to take another turn then non of the values need to change
			if(!goAgain)
			{	
				if(curNum+1==numPlayers){
				rounds++;
				}
				//iterator for current player
				curNum = curNum+1<numPlayers?curNum+1:0;
				
				//change values to the next players
				currentPlayer =  (players[curNum]).GetComponent<Player>();
				turnChangeText.GetComponent<Text>().text = "It's "+ currentPlayer.pName+"'s turn";
				playerName.GetComponent<Text>().text =currentPlayer.pName;
				playerName.GetComponent<Text>().color =currentPlayer.color;
			}
			
			//show turn start screen
			turnSwitch.SetActive(true);
			DNAbutton.SetActive(false);
			turnOver = false;
		}
		
		//Get the input based on the runtime platform
		//Android touch controls
		if (Application.platform == RuntimePlatform.Android)
		{
			
			worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			if(Input.GetTouch(0).phase==TouchPhase.Ended)
			{
				touched = true;
			}
		}
		//PC mouse controls
		else
		{
			worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if(Input.GetMouseButtonUp(0))
			{
				touched =true;
			}
		}

		//Spinner Moves
		if(spinInfo.hasMoveSet)
		{
		//psudocode
		//while we have a move set 
		//!doneWithMove
				//do the move[i=0]
				//have a function with a switch so we can just pass it a move number;
		//	when done with move 
		//		i++ and doneWithMove =false
		//		if i==2
		//			turnOver = true;
		//			HasMoveSet =false;
			int[] moveSet = spinInfo.moves;//sloppy making a new one every time
			if(!doneWithMoves)
			{
				if(moveNum<=1)
				{
					//doMove method should handle all the selecting and manipulation as well as incrementing moveNum when finished
					if(moveStarted)
					{
						doMove(moveSet[moveNum]);
					}
					else if(moveDescriptionText.GetComponent<Text>().text!=getMoveText(moveSet[moveNum]))
					{
					
						SetMoveDescription(getMoveText(moveSet[moveNum]));//this needs to be done only once
						moveDescription.SetActive(true);
					}
					
				}
				else
				{
					doneWithMoves=true;
					
				}

			}
			else
			{
				spinInfo.hasMoveSet = false;
				moveNum=0;
				doneWithMoves=false;
				endTurn();
			}
			
			//if done with move set set move set to false
			//spinInfo.hasMoveSet=false;
		}
		
		//for free-form placing animals
		if(placingAnimals)
		{
			placingControls.SetActive(true);
			doneButton.SetActive(false);
			if(currentPlayer.numAnimals-90>=0)
			{
				UpdateLeftNum(currentPlayer.numAnimals-90);
			}
			if(currentPlayer.numAnimals-90==0)
			{
				//show done button done button should start next player
				placingAnimals=false;
				endTurn();
			}
			if(touched)
			{
				
				hit = Physics2D.Raycast(worldPoint, Vector2.zero);
				if (hit && hit.collider != null) 
				{
					
					if(selection!=null)
					{
						selection.GetComponent<Habitat>().selected=false;
					}
					hit.collider.GetComponent<Habitat>().selected = !hit.collider.GetComponent<Habitat>().selected;
					selection = hit.collider.gameObject;
					Habitat sHabitat = selection.GetComponent<Habitat>();
					Sprite num = (Sprite)numbers[selection.GetComponent<Habitat>().numAnimals];
					GUInum.GetComponent<Image>().sprite=num;
				}
				touched = false;
			}
		}
		
		//turn of placingAnimals if not using controls
		if(!placingAnimals&&!relocating&&!reproducing&&!killing)
		{
			placingControls.active=false;
			
		}
	}
	public bool isOptimal(Habitat cH)
	{
		for(int i=0;i<currentPlayer.habitat.Length;i++)
		{
			if(cH.habitat==currentPlayer.habitat[i])
			{
				return true;
			}
		}
		return false;
	}
	public void GetNumBabys()
	{
		//this should probably be global
		numBabys=0;
		valid = new List<GameObject>();
		for(int i=0;i<84;i++)
		{
			GameObject cGO = darwinia[i];
			Habitat cH = cGO.GetComponent<Habitat>();
			if(cH.playerOwned == curNum && isOptimal(cH) )
			{
				numBabys +=cH.numAnimals;
				valid.Add(cGO);
				for(int v=0;v<cH.touching.Length;v++)
				{
					if(!cH.touching[v].GetComponent<Habitat>().isBarrier)
					{
						valid.Add(cH.touching[v]);
					}
				}
			}
		}
		
	}
	public void Unfade()
	{
		for(int i=0;i<84;i++)
		{
			GameObject cH = darwinia[i];
				cH.GetComponent<SpriteRenderer>().color= new Color(cH.GetComponent<SpriteRenderer>().color.r, cH.GetComponent<SpriteRenderer>().color.g, cH.GetComponent<SpriteRenderer>().color.b, 1);
				SpriteRenderer[] all = cH.GetComponentsInChildren<SpriteRenderer>();
				for(int v=0; v<all.Length; v++){
					all[v].color = new Color(all[v].color.r, all[v].color.g, all[v].color.b, 1);
				}
			
		}
	}
	public void ReproduceFadeSelect()
	{
							//do the rest of the stuff
					//make a array called valid of all the habitats that you can move to
		valid = new List<GameObject>();
		valid.Add(selection);
		GameObject[] touchingS = selection.GetComponent<Habitat>().touching;
		
		for(int i=0;i<touchingS.Length;i++)
		{
			if(canCross(touchingS[i].GetComponent<Habitat>()))//can cross barrier
			{
				//add all of Barriers touching to valid dont add barrier
				GameObject[] touchingB = touchingS[i].GetComponent<Habitat>().touching;
				for(int v=0;v<touchingB.Length;v++)
				{
					valid.Add(touchingB[v]);
				}
			}
			else if(!touchingS[i].GetComponent<Habitat>().isBarrier)
			{
				
				valid.Add(touchingS[i]);
			}
		}
		for(int i=0;i<84;i++)
		{
			GameObject cH = darwinia[i];
			if(!valid.Contains(cH))
			{
				greyOut(cH);
			}
		}
	}

	public bool CanInvade(Habitat fromHabitat,Habitat toHabitat)
	{
		//players[fH.playerOwned].predator
		if(toHabitat.playerOwned==-1||toHabitat.playerOwned==fromHabitat.playerOwned)
		{
			return false;
		}
		if(players[fromHabitat.playerOwned].GetComponent<Player>().predator!=5&&!currentPlayer.Contains(players[toHabitat.playerOwned].GetComponent<Player>().defense,players[fromHabitat.playerOwned].GetComponent<Player>().predator))//5 is not a predator
		{
			return true;
			//minNum=1;
		}
		else if(fromHabitat.numAnimals>toHabitat.numAnimals)
		{
			return true;
			//minNum = toHabitat.numAnimals+1;
		}
		return false;
	}

	public void InvadeFadeSelect()
	{
		//do the rest of the stuff
		//make a array called valid of all the habitats that you can move to
		fadeValid = new List<GameObject>();
		fadeValid.Add(selection);
		GameObject[] touchingS = selection.GetComponent<Habitat>().touching;
		
		for(int i=0;i<touchingS.Length;i++)
		{
			if(canCross(touchingS[i].GetComponent<Habitat>()))//can cross barrier
			{
				//add all of Barriers touching to valid dont add barrier
				GameObject[] touchingB = touchingS[i].GetComponent<Habitat>().touching;
				for(int v=0;v<touchingB.Length;v++)
				{
					if(CanInvade(selection.GetComponent<Habitat>(),touchingB[v].GetComponent<Habitat>()))//can invade returns true if you can compete or prey
					{
						fadeValid.Add(touchingB[v]);
					}
				}
			}
			else if(!touchingS[i].GetComponent<Habitat>().isBarrier)
			{
				if(CanInvade(selection.GetComponent<Habitat>(),touchingS[i].GetComponent<Habitat>()))
				{
					fadeValid.Add(touchingS[i]);
				}
			}
		}
		for(int i=0;i<84;i++)
		{
			GameObject cH = darwinia[i];
			if(!fadeValid.Contains(cH))
			{
				greyOut(cH);
			}
		}
	}
	public void greyOut(GameObject g)
	{
		SpriteRenderer[] all = g.GetComponentsInChildren<SpriteRenderer>();
		for(int i=0; i<all.Length; i++){
			Material cMat = all[i].GetComponent<Renderer>().material;                // Get currentMaterial
			all[i].color = new Color(all[i].color.r, all[i].color.g, all[i].color.b, fadeAlpha);
		}
		g.GetComponent<SpriteRenderer>().color= new Color(g.GetComponent<SpriteRenderer>().color.r, g.GetComponent<SpriteRenderer>().color.g, g.GetComponent<SpriteRenderer>().color.b, fadeAlpha);
		
	}
	public void AddAnimals()
	{
		Habitat sHabitat = selection.GetComponent<Habitat>();
		if(sHabitat.numAnimals<9&&playerOwned()&&!killing)
		{

			if(fadeModeStarted)
			{
				if(fromHabitat.GetComponent<Habitat>().numAnimals>0&&relocating)
				{
					
					sHabitat.numAnimals++;
					fromHabitat.GetComponent<Habitat>().numAnimals--;
					sHabitat.playerOwned=curNum;
					sHabitat.diceColor=currentPlayer.color;
					numMoves++;
					//something to change the number of moves you have left for migration
				}
				else
				{
					Unfade();
					fadeModeStarted =false;
				}
				
				if(reproducing)
				{
					currentPlayer.numAnimals--;
					animalsPerPlayer[curNum]++;
					sHabitat.numAnimals++;
					sHabitat.playerOwned=curNum;
					sHabitat.diceColor=currentPlayer.color;
					numMoves++;
				}
			}
			else
			{
				currentPlayer.numAnimals--;
				animalsPerPlayer[curNum]++;
				sHabitat.numAnimals++;
				sHabitat.playerOwned=curNum;
				sHabitat.diceColor=currentPlayer.color;
			}
			Sprite num = (Sprite)numbers[sHabitat.numAnimals];
			GUInum.GetComponent<Image>().sprite=num;
		}
	}
	public void RemoveAnimals()
	{
		Habitat sHabitat = selection.GetComponent<Habitat>();
		if(sHabitat.numAnimals>0&&playerOwned()&&!reproducing)
		{
			if(fadeModeStarted&&fromHabitat.GetComponent<Habitat>().numAnimals<9&&!killing)
			{
				sHabitat.numAnimals--;
				fromHabitat.GetComponent<Habitat>().numAnimals++;
				fromHabitat.GetComponent<Habitat>().playerOwned=curNum;
				fromHabitat.GetComponent<Habitat>().diceColor=currentPlayer.color;
				numMoves++;
				//something to change the number of moves you have left for migration
			}
			else//used for killing and regular
			{
				currentPlayer.numAnimals++;
				sHabitat.numAnimals--;
				animalsPerPlayer[curNum]--;
			}
			
			Sprite num = (Sprite)numbers[sHabitat.numAnimals];
			GUInum.GetComponent<Image>().sprite=num;
		}
	}
	void EnableButtons(GameObject[] b)
	{
		for(int i=0;i<b.Length;i++)
		{
			b[i].GetComponent<Button>().enabled = true;
		}
	}
	void DisableButtons(GameObject[] b)
	{
		for(int i=0;i<b.Length;i++)
		{
			b[i].GetComponent<Button>().enabled = false;
		}
	}
	bool canCross(Habitat h)
	{
		bool canCross=false;
		if(h.isBarrier)
		{
			for(int i=0;i<currentPlayer.crossing.Length;i++)
			{
				if(currentPlayer.crossing[i]==h.barrierType)
				{
					canCross=true;
				}
			}
		}
		else
		{
			canCross = false;
		}
		return canCross;
	}
	bool playerOwned()
	{
		Habitat sHabitat = selection.GetComponent<Habitat>();
		if(sHabitat.playerOwned==-1||sHabitat.playerOwned==curNum)
		{
			sHabitat.playerOwned = curNum;
			selection.GetComponent<Habitat>().diceColor = currentPlayer.color;
			return true;
		}
		return false;
	}
	void UpdateLeftNum(int num)
	{
		Sprite onesNum = (Sprite)numbers[num%10];
		Sprite tensNum = (Sprite)numbers[(num%100)/10];

		GUIonesLeft.GetComponent<Image>().sprite=onesNum;
		GUItensLeft.GetComponent<Image>().sprite=tensNum;
	}
	public void endTurn()
	{
		//at the end of the turn recalculate the population chart numbers
		makeChart();
		turnOver = true;
	}
	public void beginTurn()
	{
		startTurn = true;
	}
	public void makeChart()
	{
		for(int i=0 ;i<numPlayers;i++)
		{
			
			GameObject bar = barGraph[i];
			Vector3 scale = barGraph[i].transform.localScale;
			barGraph[i].transform.localScale = new Vector3(animalsPerPlayer[i]/120.0f, scale.y, scale.z);
			barGraph[i].GetComponent<Image>().color = players[i].GetComponent<Player>().color;
		}
		
	}
	public void startAction()
	{
		if(rounds==0)
		{
			placingAnimals = true;
		}
		else if(isSubscreen)
		{
			subscreenClicked=true;
			isSubscreen =false;
		}
		else
		{
			moveStarted=true;
			moveDescription.SetActive(false);
		}
		
	}
	public void pause(bool p)
	{

		isPaused=p;

	}
	public void togglePause()
	{
		isPaused= !isPaused;
	}
	public void SetMoveDescription(string s)
	{
		moveDescriptionText.GetComponent<Text>().text= s;
		//moveDescription.SetActive(true);
				
	}
	public void ClickDone()
	{
		doneClicked=true;
	}
	public void SetStatGUI()
	{
		string habitatGUI="";
		switch(currentPlayer.habitat.Length)
		{
		case 1:
			habitatGUI=getHabitat(currentPlayer.habitat[0])+"\r\n40";
			break;
		case 2:
			habitatGUI=getHabitat(currentPlayer.habitat[0])+" & "+getHabitat(currentPlayer.habitat[1])+"\r\n20";
			break;
		case 3:
			habitatGUI=(getHabitat(currentPlayer.habitat[0])+", "+getHabitat(currentPlayer.habitat[1])+" & "+getHabitat(currentPlayer.habitat[2])+"\r\n0");
			break;
		}
		habiatStat.GetComponent<Text>().text = habitatGUI;
		habiatStatC.GetComponent<Text>().text = habitatGUI;
		
		string crossingGUI="";
		switch(currentPlayer.crossing.Length)
		{
		case 1:
			crossingGUI=getCrossing(currentPlayer.crossing[0]);
			break;
		case 2:
			crossingGUI=getCrossing(currentPlayer.crossing[0])+" & "+getCrossing(currentPlayer.crossing[1]);
			break;
		}
		crossingStat.GetComponent<Text>().text = crossingGUI;
		crossingStatC.GetComponent<Text>().text = crossingGUI;
		
		string defenseGUI="";
		switch(currentPlayer.defense.Length)
		{
		case 2:
			defenseGUI=getPred(currentPlayer.defense[0])+" & "+getPred(currentPlayer.defense[1]);
			break;
		case 3:
			defenseGUI=(getPred(currentPlayer.defense[0])+", "+getPred(currentPlayer.defense[1])+" & "+getPred(currentPlayer.defense[2]));
			break;
		}
		defenseStat.GetComponent<Text>().text = defenseGUI;
		defenseStatC.GetComponent<Text>().text = defenseGUI;
		
		string toleranceGUI="";
		switch(currentPlayer.tolerance.Length)
		{
		case 1:
			toleranceGUI=getTolerance(currentPlayer.tolerance[0]);
			break;
		case 2:
			toleranceGUI=getTolerance(currentPlayer.tolerance[0])+" & "+getTolerance(currentPlayer.tolerance[1]);
			break;
		}
		toleranceStat.GetComponent<Text>().text = toleranceGUI;
		toleranceStatC.GetComponent<Text>().text = toleranceGUI;
		
		predatorStat.GetComponent<Text>().text = getPred(currentPlayer.predator);
		predatorStatC.GetComponent<Text>().text = getPred(currentPlayer.predator);
		int percent = (int)(currentPlayer.rate*100.0f);
		reproductionStat.GetComponent<Text>().text = percent.ToString()+"%";
		reproductionStatC.GetComponent<Text>().text = percent.ToString()+"%";
	}
	string getHabitat(int num)
	{
		string r ="";
		switch(num)
		{
			case 0:
				r= "Marshes";
				break;
			case 1:
				r="Swamps";
				break;	
			case 2:
				r="Brush";
				break;
			case 3:
				r="Forests";
				break;
			case 4:
				r="Meadows";
				break;
			case 5:
				r="Lakes";
				break;
		}
		return r;
	}
	string getCrossing(int num)
	{
		string r ="";
		switch(num)
		{
			case 0:
				r= "Airports";
				break;
			case 1:
				r="Cities";
				break;	
			case 2:
				r="Deserts";
				break;
			case 3:
				r="Mountains";
				break;
			case 4:
				r="Dams";
				break;
		}
		return r;
	}
	string getPred(int num)
	{
		string r ="";
		switch(num)
		{
			case 0:
				r= "Nocturnal";
				break;
			case 1:
				r="Swift";
				break;	
			case 2:
				r="Crafty";
				break;
			case 3:
				r="Camouflaged";
				break;
			case 4:
				r="Strong";
				break;
			case 5:
				r="Not a Predator";
				break;
		}
		return r;
	}
	string getTolerance(int num)
	{
		string r ="";
		switch(num)
		{
			case 0:
				r= "Cold";
				break;
			case 1:
				r="Drought";
				break;	
			case 2:
				r="Fire";
				break;
			case 3:
				r="Water";
				break;
			case 4:
				r="Air";
				break;
		}
		return r;
	}
	
	public void saveBackground()
	{
		if(placingControls.activeSelf)
			lastGUI=placingControls;
		if(moveDescription.activeSelf)
			lastGUI=moveDescription;
		if(spinner.activeSelf)
			lastGUI=spinner;
		if(BarrierPlace.activeSelf)
			lastGUI=BarrierPlace;
		if(lastGUI!=null)
		{
		
		lastGUI.SetActive(false);
		}
	}
	public void loadBackground()
	{
		if(lastGUI!=null)
		{
			lastGUI.SetActive(true);
		}
	}
	public string getMoveText(int mN)
	{
		string d="Some Move";
		switch(mN)
		{
			case 0:
				GetNumBabys();
				int percent = (int)(currentPlayer.rate*100.0f);
				d ="Reproduce\r\nAdd new animals.\r\n"+percent.ToString()+"% of "+numBabys+" is "+ (int)(numBabys * currentPlayer.rate);
				numBabys =(int)(numBabys * currentPlayer.rate);
				break;
			case 1:
				d ="Environmental Change\r\nCause a random enivronmital change to occur.";
				break;
			case 2:
				d ="Place Barrier\r\nChoose a barrier and place it somewhere on the board.";
				SetBarrierNumText();
				break;	
			case 3:
				if(currentPlayer.mobility!=0)
				{
					d ="Relocate\r\nMove your animals to a new habitat. Number of moves is based on mobility.";
				}
				else
				{
					d ="Relocate\r\nYou have a mobility of 0. Press start to finish move.";
				}
				
				break;
			case 4:
				d ="Compete/Prey\r\nYou can take lower population habitats or try to prey on large ones.";
				break;
			case 5:
				d ="Change Genes\r\nYou can redraw up to 4 genes.";
				break;
			default:
				break;
		}
		return d;
	}
	public void changeGene(int gNum)
	{
	//make pressing each of the genes change them then fade the stat
	//make pressing one make the number in left go down
	//make a variable that gets reset each time for num left
		
		
		switch(gNum)
		{
			case 0:
				currentPlayer.GenerateHabitatMobility();
				break;
			case 1:
				currentPlayer.GenerateRate();
				break;
			case 2:
				currentPlayer.GenerateCrossing();
				break;
			case 3:
				currentPlayer.GenerateDefense();
				break;
			case 4:
				currentPlayer.GeneratePredator();
				break;
			case 5:
				currentPlayer.GenerateTolerance();
				break;
		}
		geneChangeLeft--;
		geneLeftText.GetComponent<Text>().text = geneChangeLeft.ToString()+"";
		
		SetStatGUI();
	}
	public void DoneChangeGene()
	{
		
		geneChangeLeft=-1;
		doneWithCurrentMove=true;
	}
	void ResetChangeGene()
	{
			geneChangeLeft=4;
			geneChangeGUI.SetActive(false);
			EnableButtons(buttons);
			doneWithCurrentMove=true;
	}
						//doMove method should handle all the sellecting and manipulation
	public void doMove(int mN)
	{
		doneWithCurrentMove=false;
		switch(mN)
		{
			case 0://Reproduce
				if(!faded)
				{
					for(int i=0;i<84;i++)
					{
						GameObject cH = darwinia[i];
						if(!valid.Contains(cH))
						{
							greyOut(cH);
						}
					}
					startNumAnimals= currentPlayer.numAnimals;
					faded = true;
				}
				if(valid.Count==0)
				{
					//There is no where left to place your animals.
					doneWithCurrentMove=true;
				}
				else
				{
					Reproduce();
				}
				break;
			case 1://Environmental Change
				//Draw a card and show it to the player
				//Do the thing on that card
				
				if(envCard==-1)//no card yet
				{
					envCard = DrawChangeCard();
				}
				else
				{
					DoEnvironmentalChange(envCard);//this switch will have to end the move
				}
				break;
			case 2://Place Barrier
				if(NoMoreBarriers())
				{
					if(!isSubscreen)
					{
						SetMoveDescription("There are no more barriers left to place.");
						moveDescription.SetActive(true);
						isSubscreen=true;
					}
					if(subscreenClicked)
					{
						doneWithCurrentMove=true;
						moveDescription.SetActive(false);
						subscreenClicked=false;
						Unfade();
					}
				}
				else if(barrierType!=-1)
				{
					PlaceBarrier();
					/*
					//This should be a selection loop
					//first set it to nothing
					//fill valid with anything that is not a barrier or has the last remaining survivors of a players animals (last hex)
					//when they click on a habitat set that habitat to a barrier by changing its image and variables
					//also make the last habitat they selected (lastH) revert to its previous form just have a temp GameObject var
					//show the done button
					
					//if the done button is pressed end the turn
					PlaceBarrier();
					*/
				}
				else
				{
					BarrierPlace.SetActive(true);
				}
				break;
			case 3://Relocate
				Relocate();
				break;
			case 4://Compete and prey
				Invade();
				break;
			case 5://Gene Change
				geneLeftText.GetComponent<Text>().text = geneChangeLeft.ToString()+"";
				SetStatGUI();
				if(geneChangeLeft==-1)
				{
					ResetChangeGene();
					
				}
				if(geneChangeLeft==0)
				{
					DisableButtons(buttons);
				}
				if(!doneWithCurrentMove)//if ! done
				{
					geneChangeGUI.SetActive(true);
				}
				break;
			default:
				Debug.Log("Doing a move");
				doneWithCurrentMove=true;
				break;
		}
		if(doneWithCurrentMove)//if done
		{		
			doneWithCurrentMove=false;
			geneChangeGUI.SetActive(false);
			BarrierPlace.SetActive(false);
			isSubscreen=false;
			envCard=-1;
			moveNum++;
			moveStarted=false;
			if(faded)
			{
				faded = false;
				Unfade();
			}
			
		}
	}
	void PlaceBarrier()
	{
		//valid is set up on button click
		
		//doneButton.SetActive(false);
		//endcase if we have selected a habitat and clicked done
		if(doneClicked)
		{
			doneClicked = false;
			//change the values of selection 
			Habitat sHabitat = selection.GetComponent<Habitat>();
			sHabitat.isBarrier=true;
			sHabitat.barrierType = barrierType;
			
			//remove animals as in method if owned by player
			int pNum = sHabitat.playerOwned;
			if(pNum!=-1)
			{		
				players[pNum].GetComponent<Player>().numAnimals+=sHabitat.numAnimals;//numAnimals in hand
				animalsPerPlayer[pNum]-=sHabitat.numAnimals;//numAnimals on board
				sHabitat.numAnimals=0;//set animals on habitat to 0
			}
			barrierType = -1;
			doneWithCurrentMove = true;
			//the image on the habitat needs to be changed to reflect the barrier
		}
		
		if(touched)
		{
			
			hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			if (hit != null && hit.collider != null && valid.Contains(hit.collider.gameObject)) 
			{
				
				if(selection!=null)
				{
					selection.GetComponent<Habitat>().selected=false;
				}
				hit.collider.GetComponent<Habitat>().selected = !hit.collider.GetComponent<Habitat>().selected;
				selection = hit.collider.gameObject;
				Habitat sHabitat = selection.GetComponent<Habitat>();
				Sprite num = (Sprite)numbers[selection.GetComponent<Habitat>().numAnimals];
				GUInum.GetComponent<Image>().sprite=num;
				DonePlace.SetActive(true);
			}
			touched = false;
		}
	}
	void Reproduce()
	{
			
			placingControls.SetActive(true);
			reproducing = true;
			doneButton.SetActive(false);
			if(currentPlayer.numAnimals>startNumAnimals-numBabys)
			{
				UpdateLeftNum(currentPlayer.numAnimals-(startNumAnimals-numBabys));
			}
			if(startNumAnimals-numBabys==currentPlayer.numAnimals)
			{
				//show done button done button should start next player;
				//Debug.Log("Start:"+startNumAnimals+" NumBabys: "+numBabys+" Player: "+currentPlayer.numAnimals);
				doneWithCurrentMove = true;
				reproducing = false;
			}
			if(touched)
			{
				
				hit = Physics2D.Raycast(worldPoint, Vector2.zero);
				if (hit != null && hit.collider != null && valid.Contains(hit.collider.gameObject)) 
				{
					
					if(selection!=null)
					{
						selection.GetComponent<Habitat>().selected=false;
					}
					hit.collider.GetComponent<Habitat>().selected = !hit.collider.GetComponent<Habitat>().selected;
					selection = hit.collider.gameObject;
					Habitat sHabitat = selection.GetComponent<Habitat>();
					Sprite num = (Sprite)numbers[selection.GetComponent<Habitat>().numAnimals];
					GUInum.GetComponent<Image>().sprite=num;
				}
				touched = false;
			}
	}
	void Relocate()
	{
		placingControls.SetActive(true);
		doneButton.SetActive(true);
		relocating=true;
		bool movingAnimals=true;
		if(currentPlayer.mobility==0||doneClicked) //we need to show a screen that tells you you have no mobility
		{
			movingAnimals=false;
			doneClicked=false;
		}
		if(currentPlayer.mobility-numMoves>0&&currentPlayer.mobility-numMoves<=99)
		{
			UpdateLeftNum(currentPlayer.mobility-numMoves);
		
		}
		else
		{
		fadeModeStarted = false;
		selection.GetComponent<Habitat>().selected = false;
		movingAnimals=false;
		numMoves=0;
		Unfade();
		}
			
		if(touched)
		{
				
			hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			//if we hit something with a collider
		
			if (hit != null && hit.collider != null) 
			{
				//check to see if it belongs to the current player
				Habitat cH = hit.collider.gameObject.GetComponent<Habitat>();
				//if there was a previous selection turn it off
				//set the currently clicke object to selection
				if(fadeModeStarted)
				{
					placingControls.SetActive(true);
					//enable plus sign
					//left num becomes the number on fromHabitat
					//fromHabitat //gameObject
					//if cH is in valid
							
					if(cH==fromHabitat.GetComponent<Habitat>()||fromHabitat.GetComponent<Habitat>().numAnimals<=0)
					{
						fadeModeStarted = false;
						selection.GetComponent<Habitat>().selected = false;
						Unfade();
					}
					else
					{
						for(int i=0;i<valid.Count;i++)
						{
							if(valid[i].GetComponent<Habitat>()==cH&&(curNum==valid[i].GetComponent<Habitat>().playerOwned||valid[i].GetComponent<Habitat>().playerOwned==-1))
							{
								selection.GetComponent<Habitat>().selected=false;
								selection = hit.collider.gameObject;
								Sprite num = (Sprite)numbers[selection.GetComponent<Habitat>().numAnimals];
								GUInum.GetComponent<Image>().sprite=num;
								selection.GetComponent<Habitat>().selected=true;
							}
						}
					}
				}
				else
				{
					placingControls.SetActive(false);
					if(cH.playerOwned == curNum)
					{
						Unfade();
						if(selection!=null)
						{
							selection.GetComponent<Habitat>().selected = false;
						}
						selection = hit.collider.gameObject;
						cH.selected = !cH.selected;
						Sprite num = (Sprite)numbers[selection.GetComponent<Habitat>().numAnimals];
						GUInum.GetComponent<Image>().sprite=num;
						ReproduceFadeSelect();
						fromHabitat = hit.collider.gameObject;
						fadeModeStarted=true;
						//Sprite num = (Sprite)numbers[cH.numAnimals];
						//GUInum.GetComponent<Image>().sprite=num;
					}
					else
					{
						if(selection!=null)
						{
							selection.GetComponent<Habitat>().selected = false;
						}
						selection = null;
						Sprite num = (Sprite)numbers[0];
						GUInum.GetComponent<Image>().sprite=num;
						Unfade();
						fadeModeStarted=false;
					}
				}
			}
			touched = false;
		}
		if(!movingAnimals)
		{
			relocating=false;
			Unfade();
			doneWithCurrentMove=true;
			placingControls.SetActive(false);
		}
	}

	
	/*
	Psudocode for drawing a environmental change card...
	DrawCard()
	int[] changeDeck //stores the number of each type of card in the deck
	//generate a random int between 0 and the sum of the contents of changeDeck
	int card = Random.Range(0, numCards)

	int sum=0;
	for(int i =0;i<changeDeck.Length;i++)
	{
		if(card>sum&&card<sum+changeDeck[i])
		{
			DoEnvironmentalChange(i); //function with switch for each card type
		}
		sum+=changeDeck[i];
	}

	



	*/
	void DoEnvironmentalChange(int card)
	{
		if(card==0)
		{
			Midseason(); //extra turn
				
		}
		else
		{
			Kill(card);
		}
	}
	
	int DrawChangeCard()
	{
		//generate a number of change numbered in the manual
		return Random.Range(0,7);//3 or 4 works right now
	}
	
	void Kill(int changeType)
	{
		int aHabitat=0;
		int hNum=0;
		int pNum=0;
		int killCount=0;
		string habitat="";
		string s="";
		int[] lost;
		float p=.0f;
		switch(changeType)
		{
			case 1://Cold
			Debug.Log("Cold");
				//Done once at the begining
				if(!isSubscreen)
				{
					killCount =0;
					pNum = Random.Range(0,3);
					
					//set the percentage and word
					switch(pNum)
					{
						case 0://10%
							s="Slight";
							p=.1f;
							break;
						case 1://30%
							s="Mild";
							p=.3f;
							break;
							
						case 2://50%
							s = "Extreme";
							p=.5f;
							break;
					}
					//set number of moves for removal of animals
					numMoves = (int)(animalsPerPlayer[curNum] * p);
					killCount = numMoves;
					s = s+" Cold kills "+p*100+"%\r\nKills "+ killCount+(killCount==1?" animal":" animals");
					//show the subscreen and reset the switch
					SetMoveDescription(s);
					moveDescription.SetActive(true);
					isSubscreen=true;
					
					//calculate the number of valid habitats
					valid = new List<GameObject>();
					for(int i=0;i<84;i++)
					{
						GameObject cGO = darwinia[i];
						Habitat cH = cGO.GetComponent<Habitat>();
						if(cH.playerOwned == curNum)
						{
							valid.Add(cGO);
						}
					}
					startNumAnimals= animalsPerPlayer[curNum];
					
				}
				if(subscreenClicked)
				{
					//turn off the subscreen
					moveDescription.SetActive(false);
					
					//grey out all habitats not owned by the player
					for(int i=0;i<84;i++)
					{
						GameObject cGO = darwinia[i];
						Habitat cH = cGO.GetComponent<Habitat>();
						if(cH.playerOwned != curNum)
						{
							greyOut(cGO);
						}
					}
					
					//let the player select and remove animals
					KillNumAnimals(numMoves);
					
				}
			break;
			case 2://Drought
			Debug.Log("Drought");
				//Done once at the begining
				if(!isSubscreen)
				{
					killCount =0;
					pNum = Random.Range(0,3);
					
					//set the percentage and word
					switch(pNum)
					{
						case 0://10%
							s="Slight";
							p=.1f;
							break;
						case 1://30%
							s="Mild";
							p=.3f;
							break;
							
						case 2://50%
							s = "Extreme";
							p=.5f;
							break;
					}
					//set number of moves for removal of animals
					numMoves = (int)(animalsPerPlayer[curNum] * p);
					killCount = numMoves;
					s = s+" Drought kills "+p*100+"%\r\nKills "+ killCount+(killCount==1?" animal":" animals");
					//show the subscreen and reset the switch
					SetMoveDescription(s);
					moveDescription.SetActive(true);
					isSubscreen=true;
					
					//calculate the number of valid habitats
					valid = new List<GameObject>();
					for(int i=0;i<84;i++)
					{
						GameObject cGO = darwinia[i];
						Habitat cH = cGO.GetComponent<Habitat>();
						if(cH.playerOwned == curNum)
						{
							valid.Add(cGO);
						}
					}
					startNumAnimals= animalsPerPlayer[curNum];
					
				}
				if(subscreenClicked)
				{
					//turn off the subscreen
					moveDescription.SetActive(false);
					
					//grey out all habitats not owned by the player
					for(int i=0;i<84;i++)
					{
						GameObject cGO = darwinia[i];
						Habitat cH = cGO.GetComponent<Habitat>();
						if(cH.playerOwned != curNum)
						{
							greyOut(cGO);
						}
					}
					
					//let the player select and remove animals
					KillNumAnimals(killCount);		
				}
				break;
				
			//Fire
			//Kill all nontolerant species on affected habitat
			case 3:
				Debug.Log("Fire");
				if(!isSubscreen)
				{
					killCount =0;
					aHabitat = Random.Range(0,3);
					hNum=-1;
					habitat="";
					//pick habitat
					switch(aHabitat)
					{
						case 0://forrest
							habitat="forrests";
							hNum=3;
							break;
						case 1://meadows
							habitat="meadows";
							hNum=4;
							break;
							
						case 2://brush
							habitat="brush";
							hNum=2;
							break;
					}
					//if your not tolerant kill all on habitat
					if(!currentPlayer.Contains(currentPlayer.tolerance,2))
					{
						for(int i=0;i<84;i++)
						{
							GameObject cGO = darwinia[i];
							Habitat cH = cGO.GetComponent<Habitat>();
							
							//if the habitat is owned by the player and is of the affected type
							if(cH.playerOwned == curNum && cH.habitat == hNum)
							{
								//kill all animals
								killCount+=cH.numAnimals;
								//remove animals from player like in removeAnimals
								currentPlayer.numAnimals+=cH.numAnimals;//numAnimals in hand
								animalsPerPlayer[curNum]-=cH.numAnimals;//numAnimals on board
								cH.numAnimals=0;//set animals on habitat to 0
							}
						}
						s = "Fire ravages "+habitat+" and kills "+ killCount+(killCount==1?" animal":" animals");
					}
					else
					{
						s = "Fire ravages "+habitat+"\r\nYou are tolerant to fire!";
					}
					SetMoveDescription(s);
					moveDescription.SetActive(true);
					isSubscreen=true;
				}
				if(subscreenClicked)
				{
					doneWithCurrentMove=true;
					subscreenClicked=false;
					Unfade();
				}
			
				
				break;
				
			//Water
			//Kill all nontolerant species on affected habitat
			case 4:
			Debug.Log("Water");
				if(!isSubscreen)
				{
					//pick habitat to effect
					aHabitat = Random.Range(0,3);
					hNum=-1;
					killCount =0;
					habitat="";
					//pick habitat
					switch(aHabitat)
					{
						case 0://marshes
							hNum=0;
							habitat="marshes";
							break;
						case 1://lakes
							hNum=5;
							habitat="lakes";
							break;
						case 2://swamps
							hNum=1;
							habitat="swamps";
							break;
					}
					//if your not tolerant kill all on habitat
					if(!currentPlayer.Contains(currentPlayer.tolerance,3))
					{
						for(int i=0;i<84;i++)
						{
							GameObject cGO = darwinia[i];
							Habitat cH = cGO.GetComponent<Habitat>();
							//if the habitat is owned by the player and is of the affected type
							if(cH.playerOwned == curNum && cH.habitat == hNum)
							{
								//kill all animals
								killCount+=cH.numAnimals;
								//remove animals from player like in removeAnimals
								currentPlayer.numAnimals+=cH.numAnimals;//numAnimals in hand
								animalsPerPlayer[curNum]-=cH.numAnimals;//numAnimals on board
								cH.numAnimals=0;//set animals on habitat to 0
							}
						}	
						s = "Water pollution contaminates "+habitat+" and kills "+ killCount+(killCount==1?" animal":" animals");
					}
					else
					{
						s = "Water pollution strikes "+habitat+"\r\nYou are tolerant to water pollution!";
					}
					
					SetMoveDescription(s);
					moveDescription.SetActive(true);
					isSubscreen=true;
				}
				if(subscreenClicked)
				{
					doneWithCurrentMove=true;
					subscreenClicked=false;
				}
				break;
				
				
			//Air
			//Effects the nontolerant animals near the City and Airport barriers 
			case 5:
			Debug.Log("Air");
				killCount =0;
				lost = new int[4];
				s="";
				if(!isSubscreen)
				{
					for(int i=0;i<84;i++)
					{
						GameObject cGO = darwinia[i];
						Habitat cH = cGO.GetComponent<Habitat>();
						//if the habitat is barrier and city or airport
						if(cH.isBarrier && (cH.barrierType == 0 || cH.barrierType == 1))
						{
							//each touching
							for(int v=0;v<cH.touching.Length;v++)
							{
								Habitat tH = cH.touching[v].GetComponent<Habitat>();
								//if numAnimals on touching habitat is greater than 0 and the player who owns it is nontolerant
								if(tH.numAnimals>0&&!currentPlayer.Contains(players[tH.playerOwned].GetComponent<Player>().tolerance,4)) //just using currentPlayer to access the method
								{
									//Kill the animals
									pNum = tH.playerOwned;
									players[pNum].GetComponent<Player>().numAnimals+=tH.numAnimals;//numAnimals in hand
									animalsPerPlayer[pNum]-=tH.numAnimals;//numAnimals on board
									killCount +=tH.numAnimals;
									lost[pNum]+=tH.numAnimals;
									tH.numAnimals=0;//set animals on habitat to 0
								}
							}
						}
						
					}
					s = "Air pollution strikes habitats near cities and airports "+habitat+"\r\nKills "+ killCount+(killCount==1?" animal":" animals");
					SetMoveDescription(s);
					moveDescription.SetActive(true);
					isSubscreen=true;
				}
				if(subscreenClicked)
				{
					subscreenClicked=false;
					//show the number each player lost
					//turn on gui gameobject
					//when it is clicked end the turn
					//assign each text field with the number lost and the players names
					moveDescription.SetActive(false);
					lostScreen.SetActive(true);
					for(int i=0;i<4;i++)
					{
						if(i<numPlayers)
						{
							//create a names and num [] with text ui elements &gui lostScreen
							names[i].GetComponent<Text>().text = players[i].GetComponent<Player>().pName;
							nums[i].GetComponent<Text>().text = lost[i].ToString()+"";
							names[i].GetComponent<Text>().color = players[i].GetComponent<Player>().color;
							nums[i].GetComponent<Text>().color = players[i].GetComponent<Player>().color;
							Debug.Log(players[i].GetComponent<Player>().pName+" : "+lost[i].ToString()+"");
						}
						else
						{
							names[i].GetComponent<Text>().text = "";
							nums[i].GetComponent<Text>().text = "";
						}
					}
				}
				if(doneClicked)
				{
					doneWithCurrentMove=true;
					doneClicked=false;
				}
				break;
				
			//Disease
			//Kills over populated habitats owned by the current player
			case 6:
			Debug.Log("Disease");
				killCount =0;
				if(!isSubscreen)
				{
					for(int i=0;i<84;i++)
					{
						GameObject cGO = darwinia[i];
						Habitat cH = cGO.GetComponent<Habitat>();
						// if the habitat is owned by the player and
						// numAnimals is equal to or more than the over Population amount
						if(cH.playerOwned == curNum && cH.numAnimals >= overPop)
						{
							// kill all animals
							// remove animals from player like in removeAnimals
							pNum = cH.playerOwned;
							players[pNum].GetComponent<Player>().numAnimals+=cH.numAnimals;//numAnimals in hand
							animalsPerPlayer[pNum]-=cH.numAnimals;//numAnimals on board
							killCount += cH.numAnimals;
							cH.numAnimals=0;//set animals on habitat to 0
						}
					}		
					s = "Disease wipes out high population habitats "+habitat+"\r\nKills "+ killCount+(killCount==1?" animal":" animals");
					SetMoveDescription(s);
					moveDescription.SetActive(true);
					isSubscreen=true;
				}


				if(subscreenClicked)
				{
					subscreenClicked=false;
					doneWithCurrentMove=true;
				}
	
				break;
				
			//man-made event
			//Kills all animals on the selected habitat
			case 7:
			Debug.Log("Man");
				//has an equal chance to affect every habitat
				aHabitat = Random.Range(0,6);
				//may want a switch to show a screen describing what happened to each player
				string r="";
				killCount =0;
				lost = new int[4];
				if(!isSubscreen)
				{
					for(int i=0;i<84;i++)
					{
						GameObject cGO = darwinia[i];
						Habitat cH = cGO.GetComponent<Habitat>();
						//if the habitat is owned and is the right type of habitat
						if(cH.playerOwned != -1 && cH.habitat == aHabitat)
						{
							//kill all animals
							//remove animals from player like in removeAnimals
							pNum = cH.playerOwned;
							players[pNum].GetComponent<Player>().numAnimals+=cH.numAnimals;//numAnimals in hand
							animalsPerPlayer[pNum]-=cH.numAnimals;//numAnimals on board
							lost[pNum]+=cH.numAnimals;//number of animals killed for stat screen
							killCount+=cH.numAnimals;
							cH.numAnimals=0;//set animals on habitat to 0
							
						}
					}
					
					switch(aHabitat)
					{
					//Use words that show the need for the man made event but provoke sadness for losing the animals
						case 0:
							r= "Marshes are used for sanitary disposal\n\rKills "+ killCount+(killCount==1?" animal":" animals");
							break;
						case 1:
							r="Swamps are drained for irrigation\n\rKills "+ killCount+(killCount==1?" animal":" animals");
							break;	
						case 2:
							r="Brush is cleared for housing developments\n\rKills "+ killCount+(killCount==1?" animal":" animals");
							break;
						case 3:
							r="Forests are cut down for lumber\n\rKills "+ killCount+(killCount==1?" animal":" animals");
							break;
						case 4:
							r="Meadows turned to pasture for agriculture\n\rKills "+ killCount+(killCount==1?" animal":" animals");
							break;
						case 5:
							r="Lakes are damed for hydroelectricity\n\rKills "+ killCount+(killCount==1?" animal":" animals");
							break;
					}
					
					SetMoveDescription(r);
					moveDescription.SetActive(true);
					isSubscreen=true;
					
				}
				
				if(subscreenClicked)
				{

					subscreenClicked=false;
					//see how many each player lost..show to others?
					//show the number each player lost
					//turn on gui gameobject
					//when it is clicked end the turn
					//assign each text field with the number lost and the players names
					if(killCount!=0)
					{
						moveDescription.SetActive(false);
						lostScreen.SetActive(true);
						for(int i=0;i<4;i++)
						{
							if(i<numPlayers)
							{
								//create a names and num [] with text ui elements &gui lostScreen
								names[i].GetComponent<Text>().text = players[i].GetComponent<Player>().pName;
								nums[i].GetComponent<Text>().text = lost[i].ToString()+"";
								names[i].GetComponent<Text>().color = players[i].GetComponent<Player>().color;
								nums[i].GetComponent<Text>().color = players[i].GetComponent<Player>().color;
								Debug.Log(players[i].GetComponent<Player>().pName+" : "+lost[i].ToString()+"");
							}
							else
							{
								names[i].GetComponent<Text>().text = "";
								nums[i].GetComponent<Text>().text = "";
							}
						}
					}

				}
				if(doneClicked)
				{
					doneWithCurrentMove=true;
					doneClicked=false;
				}
				break;
				default:
					Debug.Log(pNum);
				break;
		}
		if(doneWithCurrentMove)
		{
		}
		
	}

	//remove the animals that have been killed
	void KillNumAnimals(int killCount)
	{
		placingControls.SetActive(true);
		killing = true;
		doneButton.SetActive(false);
		if(animalsPerPlayer[curNum]>startNumAnimals-killCount)
		{
			UpdateLeftNum(animalsPerPlayer[curNum]-(startNumAnimals-killCount));
		}
		if(startNumAnimals-killCount==animalsPerPlayer[curNum])
		{
			//show done button done button should start next player;
			//Debug.Log("Start:"+startNumAnimals+" NumBabys: "+numBabys+" Player: "+currentPlayer.numAnimals);
			doneWithCurrentMove = true;
			subscreenClicked=false;
			killing = false;
			numMoves=0;
			Unfade();
		}
		if(touched)
		{
			
			hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			if (hit.collider != null && valid.Contains(hit.collider.gameObject)) 
			{
				
				if(selection!=null)
				{
					selection.GetComponent<Habitat>().selected=false;
				}
				hit.collider.GetComponent<Habitat>().selected = !hit.collider.GetComponent<Habitat>().selected;
				selection = hit.collider.gameObject;
				Habitat sHabitat = selection.GetComponent<Habitat>();
				Sprite num = (Sprite)numbers[selection.GetComponent<Habitat>().numAnimals];
				GUInum.GetComponent<Image>().sprite=num;
			}
			touched = false;
		}
	}
	//get another turn and end turn
	void Midseason()
	{
		//boolean switch to get another turn
		if(!isSubscreen)
		{
			goAgain = true;
			SetMoveDescription("Mild Season\r\nYou get an extra turn!");
			moveDescription.SetActive(true);
			isSubscreen=true;
		}
		if(subscreenClicked)
		{
			doneWithCurrentMove=true;
			subscreenClicked=false;
		}
		
	}
	
	//This sets the barrier type when the types button is clicked
	//the barrier type is later used when selecting the habitat to destroy
	//barrier num key 0-ap 1-city 2-desert 3-mount 4-dams
	public void SetBarrierType(int bT)
	{
		barrierType = bT;
		numBarriers[bT]--;
		BarrierPlace.SetActive(false);
		//set up valid because this is a one time call
		valid = new List<GameObject>();
		for(int i=0;i<84;i++)
		{
			
			GameObject cGO = darwinia[i];
			Habitat cH = cGO.GetComponent<Habitat>();
			Debug.Log("cH.playerOwned = " + cH.playerOwned);
			//h is valid if not a barrier and not owned by a player
			if(!cH.isBarrier&& cH.playerOwned==-1)
			{
				valid.Add(cGO);
			}
			//if owned by player it cannot be there last habitat
			else if(cH.playerOwned!=-1&&!(cH.numAnimals == animalsPerPlayer[cH.playerOwned]))
			{
				valid.Add(cGO);
			}
		}
	}
	//Set up counting barrier cards
	/*
	Need a array starting with 2 for each barrier
	helper mthods
	SetBarrierNumText - set the text in the ui to the correct numbers in the array and
						if the number is 0 then fade the barrier type in the ui if they all = 0
						
	*/
	//incharge of the barrier GUI
	void SetBarrierNumText()
	{
		for(int i=0;i<5;i++)
		{
			//set the current number of barriers in the gui
			BarrierText[i].GetComponent<Text>().text = " x"+numBarriers[i].ToString();
			
			//if non of that type of barrier remain then make it uninteractable this makes it have its disabled color
			if(numBarriers[i]==0)
			{
				
				BarrierButton[i].GetComponent<Button>().interactable = false;
			}
		}

		
	}
	bool NoMoreBarriers()
	{
		for(int i=0;i<numBarriers.Length;i++)
		{
			if(numBarriers[i]!=0) //we have a barrier left
				return false;
		}
		return true; //will only return true if they are all 0's
	}
	void Invade()
	{
			//first select fro
		doneButton.SetActive(true);
		if(doneClicked) //done invading finish turn
		{
			doneClicked=false;
		}
		
		if(touched)
		{
			
			hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			//if we hit something with a collider
			
			if (hit != null && hit.collider != null) 
			{
				//check to see if it belongs to the current player
				Habitat cH = hit.collider.gameObject.GetComponent<Habitat>();
				//if there was a previous selection turn it off
				//set the currently clicke object to selection
				if(fadeModeStarted)
				{
					//enable plus sign
					//left num becomes the number on fromHabitat
					//fromHabitat //gameObject
					//if cH is in valid
					
					if(cH==fromHabitat.GetComponent<Habitat>()||fromHabitat.GetComponent<Habitat>().numAnimals<=0)
					{
						fadeModeStarted = false;
						selection.GetComponent<Habitat>().selected = false;
						Unfade();
						AttackButton.SetActive(false);
					}
					else
					{
						for(int i=0;i<fadeValid.Count;i++)
						{
							if(fadeValid[i].GetComponent<Habitat>()==cH)
							{
								selection.GetComponent<Habitat>().selected=false;
								selection = hit.collider.gameObject;
								Sprite num = (Sprite)numbers[selection.GetComponent<Habitat>().numAnimals];
								GUInum.GetComponent<Image>().sprite=num;
								selection.GetComponent<Habitat>().selected=true;
								AttackButton.SetActive(true);
							}
						}
					}

				}
				else //this is where you select the from habitat
				{
					//set up valid with each habitat touching habitats that can be invaded
					//for(int i=0;i<84;i++)
					//{

					//}
					//not here

					if(cH.playerOwned == curNum) //if the habitat is owned by the current player and has habitats it can invade
					{
						Unfade();
						if(selection!=null)
						{
							selection.GetComponent<Habitat>().selected = false;
						}
						selection = hit.collider.gameObject;
						cH.selected = !cH.selected;
						Sprite num = (Sprite)numbers[selection.GetComponent<Habitat>().numAnimals];
						GUInum.GetComponent<Image>().sprite=num;
						InvadeFadeSelect();//fade needs to be changed to include only those habitats that can be invade
						fromHabitat = hit.collider.gameObject;
						fadeModeStarted=true;
						//Sprite num = (Sprite)numbers[cH.numAnimals];
						//GUInum.GetComponent<Image>().sprite=num;
					}
					else
					{
						if(selection!=null)
						{
							selection.GetComponent<Habitat>().selected = false;
						}
						selection = null;
						Sprite num = (Sprite)numbers[0];
						GUInum.GetComponent<Image>().sprite=num;
						Unfade();
						fadeModeStarted=false;
					}
				}
			}
			touched = false;
		}
		if(attackButtonPushed)//
		{
			//method to move animals and add more
			doneWithCurrentMove=true;
			attackButtonPushed=false;
			Unfade();
		}
		/*if(!movingAnimals)
		{
			relocating=false;
			Unfade();
			doneWithCurrentMove=true;
			placingControls.SetActive(false);
		}*/
	}
	public void DebugButton(string s)
	{
		Debug.Log(s);
	}
	public void StartAttack()
	{
		attackButtonPushed=true;
	}
}		