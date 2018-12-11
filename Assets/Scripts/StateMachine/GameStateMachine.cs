using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameStateMachine : MonoBehaviour {

    public PlayerManager playerManager;
    //all of our states and the current State holder
    public IGameState           currentState;
    public StartState           startState;
    public PlacingState         placingState;
    public IntroTurnState       introTurnState;
    public SpinningState        spinningState;
    public List<IGameState>     moveStates;

    public int[] moveSet;
    public int curMoveNum;
    //System for showing messages once in the games update loop
    // used in ShowMessageOnce (s)
    public GameObject messageGUI;
    bool messageShownOnce;
    

    public GameObject[] darwinia;

    //itialize all the states with references the this machine
    void Awake()
    {
        messageShownOnce = false;
        playerManager = FindObjectOfType<PlayerManager>();
        darwinia = new GameObject[84];
        moveStates = new List<IGameState>();
        moveStates.Add(GetComponent<ReproduceState>());
        moveStates.Add(GetComponent<EnvironmentChangeState>());
        moveStates.Add(GetComponent<BarrierState>());
        moveStates.Add(GetComponent<RelocateState>());
        moveStates.Add(GetComponent<PreyState>());
        moveStates.Add(GetComponent<GeneChangeState>());
        
    }

    //we want the game to begin in the start State
    void Start()
    {
        currentState = startState;
        for (int i = 1; i <= 84; i++)
        {
            darwinia[i - 1] = GameObject.Find("Habitat" + i);
        }

    }

    //this is the MonoBehavior that unity calls every frame
    void Update()
    {

        currentState.StateUpdate();
    }
    public void StartNextPhase()
    {
        currentState.NextPhase();
        
    }
    public void EndState()
    {
        currentState.EndState();
    }

    public void StartNextPlayersTurn()
    {
        curMoveNum = 0;
        PlayerManager.instance.NextPlayer();
        currentState = introTurnState; // this is the wrong state we want to show a turn switch GUI
    }

    public bool LastMove()
    {
        return curMoveNum == moveSet.Length-1;
    }

    public void StartNextMove()
    {
        curMoveNum++;
        currentState = moveStates[moveSet[curMoveNum]];
    }
    //the message GUI is responsible for reseting the messageShownOnce boolean
    public void ShowMessageOnce(String s)
    {
        if (!messageShownOnce)
        {
            messageGUI.SetActive(true);
            messageGUI.GetComponentInChildren<Text>().text = s;
            messageShownOnce = true;
        }
    }
    public void MessageShown()
    {
        messageShownOnce = false;
        messageGUI.SetActive(false);
        StartNextPhase();
       

    }
    public void FadeOutInvalidHabitats(List<GameObject> validHabitats)
    {
        for (int i = 0; i < 84; i++)
        {
            GameObject currentGameObject = darwinia[i];
            Habitat currentHabitat = currentGameObject.GetComponent<Habitat>();
            if (!validHabitats.Contains(currentGameObject))
            {

                currentGameObject.GetComponent<Habitat>().FadeOut();
            }
        }
    }
   
    public void FadeIn()
    {
        for (int i = 0; i < 84; i++)
        {
            GameObject currentGameObject = darwinia[i];
            currentGameObject.GetComponent<Habitat>().FadeIn();

        }
    }

}
