using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GeneChangeState : MonoBehaviour, IGameState
{
    private GameStateMachine gameStateMachine;
    public GameObject infoGUI;
    bool started = false;
    [TextArea()]
    public string introMessage = "";
    bool ended = false;
    enum Phase { start, end }
    Phase curPhase;

    void Awake()
    {
        gameStateMachine = GetComponent<GameStateMachine>();
        curPhase = Phase.start;
    }

    //begins everytime  the StateUpdate() is called
    public void BeginState()
    {
        started = true;
        //show an intro gui
        Debug.Log("Gene Change State: Started");
        infoGUI.SetActive(true);
        infoGUI.GetComponentInChildren<Text>().text = introMessage;


    }

    //this acts as the main update untill  we change states
    public void StateUpdate()
    {
        switch (curPhase)
        {
            case Phase.start:
                if (!started)
                    BeginState();
                break;
            case Phase.end:
                EndState();
                break;
            default:
                Debug.Log("Entered an invalid phase number");
                ended = true;
                break;

        }
    }

    //this should move us to a new state
    public void EndState()
    {
        Debug.Log("Gene Change State: Ended");
        started = false;
        ended = false;
        infoGUI.SetActive(false);
        curPhase = Phase.start;
        //if this was the last move
        if (gameStateMachine.LastMove())
        {
            //show the end of turn summary

            //start the next players turn
            gameStateMachine.StartNextPlayersTurn();
        }
        //if there are more moves left to do
        else
        {
            gameStateMachine.StartNextMove();
        }
    }

    public void NextPhase()
    {
        curPhase++;
        Debug.Log("Gene State: Phase " + curPhase);
    }

    /*
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
    */
}
