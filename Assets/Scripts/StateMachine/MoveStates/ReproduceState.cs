using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
/*
Rules for reproduction from the game manual
"Reproduce. Only individuals in optimal habitats, as listed on the Habitat & Mobility card,
reproduce. To reproduce, count individuals (dots) in optimal habitats and multiply by the
number on the Reproduction card. It may be necessary to round off to the nearest
individual. Add as many of this number of new individuals as you can to your species.
Example: Your optimal habitats are Meadow and Swamp and you have 13 individuals in
these habitats. Your Reproduction card reads "0.6 x # Individuals in Optimal Habitats = #
New Individuals." Then, 0.6 x 13 = 7.8, which rounds off to 8.
You may add new cubes or turn your existing cubes to higher numbers in those hexagons
adjacent to your reproductive individuals (those in optimal habitats). Or you may turn
existing cubes in optimal habitats to higher numbers or combine these strategies."

Reproduce State
This state has multiple phases 

Find the number of individuals to place (babys)
Find the habiats the player can reproduce in as well as the adjacent habitats add them to validHabitats
FadeOut all habitats not in the optimalHabitats
Let the player place animals freely in the validHabitats untill numPlaced == numBabys
End the State
*/
public class ReproduceState : MonoBehaviour, IGameState
{
    private GameStateMachine gameStateMachine;
    public GameObject infoGUI;
    bool started = false;
    enum Phase {start, prompt, placing, end }
    Phase curPhase;
    [TextArea()]
    public string introMessage = "";
    bool ended = false;
    List<Habitat> selectedHabitats;
    Habitat selectedHabitat;
    private RaycastHit2D hit;
    Vector3 worldPoint;
    bool touched;
    public List<GameObject> validHabitats;
    public GameObject placingControls;
    public Text numLeftToPlaceGUI;
    int numParents;
    int numBabys;

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
        Debug.Log("Reproduce State: Started");
        infoGUI.SetActive(true);
        
        int percent = (int)(gameStateMachine.playerManager.currentPlayer.rate * 100.0f);
        numBabys = (int)(numParents * gameStateMachine.playerManager.currentPlayer.rate);
        introMessage = "Reproduce\r\nAdd new animals.\r\n" + percent.ToString() + "% of " + numParents + " is " + numBabys;
        infoGUI.GetComponentInChildren<Text>().text = introMessage;
    }

    //this acts as the main update untill  we change states
    public void StateUpdate()
    {

        //if it is not the begining phase and the state has not ended
        switch(curPhase)
        {
            case Phase.start:
                if (!started)
                {
                    GetParentsAndValidHabitats();
                    BeginState();
                    gameStateMachine.FadeOutInvalidHabitats(validHabitats);
                }
                    break;
            case Phase.prompt:
                //Click on the Habitats to place children
                gameStateMachine.ShowMessageOnce("Click on the Habitats to place children");
                
                break;
            case Phase.placing:
                //Find the habiats the player can reproduce in as well as the adjacent habitats add them to optimalHabitats
                //FadeOut all habitats not in the optimalHabitats


                //Currently working on selecting habitats and getting placing controls to work


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
                        if (validHabitats.Contains(selectedHabitat.gameObject))
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
        Debug.Log("Reproduce State: Ended");
        started = false;
        ended = false;
        infoGUI.SetActive(false);
        curPhase = Phase.start;
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
        Debug.Log("Reproduce State: Phase "+curPhase);
    }


    public bool isOptimal(Habitat cH)
    {
        for (int i = 0; i < gameStateMachine.playerManager.currentPlayer.habitat.Length; i++)
        {
            if (cH.habitat == gameStateMachine.playerManager.currentPlayer.habitat[i])
            {
                return true;
            }
        }
        return false;
    }
    public void GetParentsAndValidHabitats()
    {
        //Counts the number of parents that can reproduce
        //Adds all the habitats that are reproducing and there touching habitats to valid
        numParents = 0;
        validHabitats = new List<GameObject>();
        for (int i = 0; i < 84; i++)
        {
            GameObject currentGameObject = gameStateMachine.darwinia[i];
            Habitat currentHabitat = currentGameObject.GetComponent<Habitat>();
            if (currentHabitat.owner == gameStateMachine.playerManager.currentPlayer && isOptimal(currentHabitat))
            {
                numParents += currentHabitat.numAnimals;
                validHabitats.Add(currentGameObject);
                AddTouchingHabitats(currentHabitat);
            }
        }

    }
    private void AddTouchingHabitats(Habitat h)
    {
        for (int v = 0; v < h.touching.Length; v++)
        {
            if (h.touching[v].GetComponent<Habitat>().owner == gameStateMachine.playerManager.currentPlayer|| h.touching[v].GetComponent<Habitat>().owner==null)
            {
                //if the touching habitat is not a barrier then add it to valid
                if (!h.touching[v].GetComponent<Habitat>().isBarrier)
                {
                    validHabitats.Add(h.touching[v]);
                }
                //if the touching habitat is a barrier then see if the player can cross it
                // then add all of the barriers touching habitats to valid
                else if (gameStateMachine.playerManager.currentPlayer.canCross(h.touching[v].GetComponent<Habitat>()))//can cross barrier
                {
                    AddTouchingHabitats(h.touching[v].GetComponent<Habitat>());
                }
            }
        }
    }



}
