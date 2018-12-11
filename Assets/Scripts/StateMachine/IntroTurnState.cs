using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class IntroTurnState : MonoBehaviour, IGameState
{
    private GameStateMachine gameStateMachine;
    public GameObject introGUI;
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
        Debug.Log("Introduction to Turn: Started");
        introGUI.SetActive(true);
        introMessage = "It's " + PlayerManager.instance.currentPlayer.pName + "'s turn.";
        introGUI.GetComponentInChildren<Text>().text = introMessage; ;


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
        Debug.Log("Introduction to Turn State: Ended");
        started = false;
        ended = false;
        introGUI.SetActive(false);
        curPhase = Phase.start;
        gameStateMachine.currentState = gameStateMachine.spinningState; //spinner
    }

    public void NextPhase()
    {
        curPhase++;
        Debug.Log("IntroTurn State: Phase " + curPhase);
    }
}
