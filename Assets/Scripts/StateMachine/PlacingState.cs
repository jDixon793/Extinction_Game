using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlacingState : MonoBehaviour, IGameState
{
    private GameStateMachine gameStateMachine;
    public GameObject introGUI;
    bool started = false;
    string introMessage = "";
    bool ended = false;
    List<Habitat> selectedHabitats;
    Habitat selectedHabitat;
    private RaycastHit2D hit;
    Vector3 worldPoint;
    bool touched;
    public GameObject placingControls;
    public Text numLeftToPlaceGUI;
    enum Phase { start, placing, end }
    Phase curPhase;

    void Awake()
    {
        gameStateMachine = GetComponent<GameStateMachine>();
        selectedHabitat = new Habitat();
        selectedHabitats = new List<Habitat>();
        curPhase = Phase.start;
    }

    //begins everytime  the StateUpdate() is called
    public void BeginState()
    {
        Debug.Log("Placing State: Started");
        started = true;
        //show an intro gui
        introGUI.SetActive(true);
        introMessage = "It's " + PlayerManager.instance.currentPlayer.pName + "'s turn.";
        introGUI.GetComponentInChildren<Text>().text = introMessage;
        numLeftToPlaceGUI.text = (30 - PlayerManager.instance.currentPlayer.numAnimals).ToString();
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
            case Phase.placing:
                if (placingControls.activeSelf)
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {

                        worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        if (Input.GetTouch(0).phase == TouchPhase.Ended)
                        {
                            touched = true;
                        }
                    }
                    //PC mouse controls
                    else
                    {
                        worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        if (Input.GetMouseButtonUp(0))
                        {
                            touched = true;
                        }
                    }
                    //if there is clicked/touched data
                    //make sure to turn touched to off everytime we deal with it
                    if (touched)
                    {
                        hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                        if (hit && hit.collider != null)
                        {

                            selectedHabitat = hit.collider.GetComponent<Habitat>();

                            //if the selected habitat is not owned by an other player, select it
                            if (selectedHabitat.owner == null || selectedHabitat.owner == PlayerManager.instance.currentPlayer)
                            {
                                //makes the currently clicked on habititat selected if its not currently selected
                                //and makes it unselected if it was currently unselected
                                selectedHabitat.selected = !selectedHabitat.selected;
                                if (selectedHabitats.Contains(selectedHabitat))
                                {
                                    selectedHabitats.Remove(selectedHabitat);
                                }
                                else
                                {
                                    selectedHabitats.Add(selectedHabitat);
                                }
                            }
                        }
                        touched = false;
                    }
                }
                break;
            case Phase.end:
                EndState();
                break;
            default:
                Debug.Log("Entered an invalid phase number");
                ended = true;
                break;

        }



        //if the placing controls are active
       

        if (ended)
        {

            EndState();
        }
    }

    //this should move us to a new state

    //increment the number of animals on the selected habitats
    public void AddAnimalsToSelected()
    {
        foreach(Habitat h in selectedHabitats)
        {
            if(PlayerManager.instance.currentPlayer.numAnimals + 1 <= 30)
            {
                h.AddAnimals(1, PlayerManager.instance.currentPlayer);
            }
            if (PlayerManager.instance.currentPlayer.numAnimals >= 30)
            {
                curPhase = Phase.end;
                break;
            }

        }
        numLeftToPlaceGUI.text = (30 - PlayerManager.instance.currentPlayer.numAnimals).ToString();
    }
    public void RemoveAnimalsFromSelected()
    {
        foreach (Habitat h in selectedHabitats)
        {
            h.RemoveAnimals(1, PlayerManager.instance.currentPlayer);
        }
        numLeftToPlaceGUI.text = (30 - PlayerManager.instance.currentPlayer.numAnimals).ToString();
    }


    public void EndState()
    {
        Debug.Log("Placing State: Ended");
        started = false;
        ended = false;
        ClearSelection();
        placingControls.SetActive(false);
        curPhase = Phase.start;

        //if the current player is the last player then go to the next state
        if (PlayerManager.instance.IsLastPlayer())
        {
            gameStateMachine.currentState = gameStateMachine.introTurnState;
        }
        //else increment players
        else
        {
            gameStateMachine.currentState = gameStateMachine.placingState;
        }
        PlayerManager.instance.NextPlayer();

    }

    private void ClearSelection()
    {
        foreach (Habitat h in selectedHabitats)
        {
            h.selected = false;
        }
        selectedHabitats.Clear();
    }

    public void NextPhase()
    {
        curPhase++;
        Debug.Log("Placing State: Phase " + curPhase);
    }
}
