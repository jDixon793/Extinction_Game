using UnityEngine;
using System.Collections;

//This is the interface that all of our game states are going to use
//The only functions or variables that need to be included are ones we want
//to be able to access from all States (using currentState)
public interface IGameState  {
    void StateUpdate();
    void BeginState();
    void EndState();
    void NextPhase();
}
