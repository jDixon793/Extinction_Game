using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class PlayerManager : MonoBehaviour {
    private static PlayerManager _instance;
    public static PlayerManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PlayerManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public List<Player> allPlayers;
    public Player currentPlayer;
    int curPlayerNum = 0;
   
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
            {
                Destroy(this.gameObject);
            }
        }

        allPlayers = new List<Player>();

    }

    void Start()
    {
        //for debuging so we dont have to go
        //through the selection process every time
        if(transform.childCount>0)
        {
            SetupPlayersFromChildren();
        }
    }
    public void NextPlayer()
    {
        curPlayerNum = (curPlayerNum + 1) % allPlayers.Count;
        currentPlayer = allPlayers[curPlayerNum];
    }


    //Gets the name and color for the entered players
    //create array for management and
    //create gameobject for debug
    public void SetupPlayers()
    {
        for (int i = 1; i <= 4; i++)
        {
            
            Text playerName = GameObject.Find("p" + i + "Text").GetComponent<Text>();
            Color color = GameObject.Find("p" + i + "Color").GetComponent<Image>().color;
            if (playerName != null && playerName.text != "")
            {
                GameObject p = new GameObject("player" + (allPlayers.Count + 1));
                p.transform.parent = this.gameObject.transform;
                p.AddComponent<Player>();
                p.GetComponent<Player>().pName = playerName.text;
                p.GetComponent<Player>().color = color;
                allPlayers.Add(p.GetComponent<Player>());
            }
        }

        currentPlayer = allPlayers[0];
    }


    //for debuging so we dont have to go
    //through the selection process every time
    void SetupPlayersFromChildren()
    {
        foreach(Transform g in transform)
        {
            allPlayers.Add(g.GetComponent<Player>());
        }
        currentPlayer = allPlayers[0];
    }

    public bool IsLastPlayer()
    {
        return currentPlayer == allPlayers[allPlayers.Count-1];
    }
}
