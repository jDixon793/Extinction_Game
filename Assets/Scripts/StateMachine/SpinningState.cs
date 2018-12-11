using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpinningState : MonoBehaviour, IGameState
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
        Debug.Log("Spinning State: Started");
        introGUI.SetActive(true);
        introGUI.GetComponentInChildren<Text>().text = introMessage;


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
        Debug.Log("Spnning State: Ended");
        started = false;
        ended = false;
        introGUI.SetActive(false);
        curPhase = Phase.start;
        gameStateMachine.currentState = gameStateMachine.moveStates[ gameStateMachine.moveSet[gameStateMachine.curMoveNum]]; //spinner
    }

    public void NextPhase()
    {
        curPhase++;
        Debug.Log("Spinning State: Phase " + curPhase);
    }
}
