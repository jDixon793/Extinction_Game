using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartState : MonoBehaviour, IGameState
{
    private GameStateMachine gameStateMachine;
    [TextArea()]
    public string introMessage= "";
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

        //show an intro gui
        Debug.Log("Start State: Started");     

    }

    //this acts as the main update untill  we change states
    public void StateUpdate()
    {
        switch (curPhase)
        {
            case Phase.start:
                gameStateMachine.ShowMessageOnce(introMessage);
                break;
            case Phase.end:
                EndState();
                break;
            default:
                Debug.Log("Entered an invalid phase number");
                break;

        }
    }

    //this should move us to a new state and reset the current state
    public void EndState()
    {
        Debug.Log("Start State: Ended");
        curPhase = Phase.start;
        gameStateMachine.currentState = gameStateMachine.placingState;
    }
    public void NextPhase()
    {
        curPhase++;
        Debug.Log("Start State: Phase " + curPhase);
    }
}
