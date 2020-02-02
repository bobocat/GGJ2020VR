using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [HideInInspector]
    public enum gameModeType { phase1, phase2};
    public gameModeType gameMode;

    public static GameManager instance;
    DataManager dataManager;

    public string gameID;   // if this is set then it will use it
    public Game game;

    public GameObject artifactPrefab;
    public GameObject elderSignPrefab;
    public Transform[] artifactSpawnPoints;
    public Transform[] elderSignSpawnPoints;

    private Player player;

    private int orbsFound = 0;

    public float gateTimer = 50;
    public bool timerRunning = false;

    public float messageDelay = 10;
    float messageTimer;
    bool playMessage = true;

    public Slider gateLevelSlider;
    public GameObject failScreen;
    public GameObject winScreen;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        dataManager = FindObjectOfType<DataManager>();
        player = FindObjectOfType<Player>();
        gameMode = gameModeType.phase1;

        game = new Game();
        CreateNewGame(gameID);
        CreateArtifact();

        // this is the first message to find orbs
        messageTimer = messageDelay;
        playMessage = true;

        timerRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning) UpdateGateTimer();
        if (game.gateLevel <= 0)
        {
            TriggerEndGameLoss();
        }

        if (playMessage)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0)
            {
                // play the orb message
                AudioBank.instance.PlayClip(2);
                messageTimer = 10f;             // reset the timer
            }

        }
        
    }

    public void UpdateGateTimer()
    {
        gateTimer -= Time.deltaTime;
        gateLevelSlider.value = gateTimer;
        int intGateTime = (int)gateTimer;
        if (intGateTime != game.gateLevel)
        {
            game.gateLevel = intGateTime;
            dataManager.WriteGateLevelToFirebase();
        }
    }

    void TriggerEndGameWin()
    {
        winScreen.SetActive(true);
    }

    void TriggerEndGameLoss()
    {
        failScreen.SetActive(true);
    }

    public void CreateNewGame(string id)
    {
        // create a new game
        if (id == "")
        {
            game.code = Random.Range(1000, 9999).ToString();
        }
        else game.code = id;

        game.foundMatchingArtifact = false;
        game.artifact1 = "";
        game.artifact2 = "";
        game.artifact3 = "";
        game.gateLevel = 70;

        dataManager.WriteGameDataToFirebase();

        // when the sane player finds a match it moves the artifact trigger
        DataManager.instance.ListenForSaneMatch();


        // instantiate the player
//        player = Instantiate(playerPrefab);
//        player.transform.position = game.playerPosition;
    }

    public void GrabArtifact(string name)
    {
        AudioBank.instance.PlayClip(1);
        game.artifact1 = name;
        DataManager.instance.WriteArtifact1ToFirebase();
        game.gateLevel = 100;
        UpdateGateTimer();
    }

    public void GrabElderSign(GameObject GO)
    {
        Destroy(GO);
        AudioBank.instance.PlayClip(0);
        gateTimer += 15;
        UpdateGateTimer();

        // ensure there is always one elder sign in the world
        int count = FindObjectsOfType<ElderSign>().Length;

        Debug.Log("found " + count + "orbs");

        if (count <= 2)
        {
            CreateElderSign();
        }

        orbsFound++;
        if (orbsFound == 2)
        {
            AudioBank.instance.PlayClip(3);
            playMessage = false;    // turn off the orb instruction message
        }
    }

    public void CreateElderSign()
    {
        Debug.Log("creating orb");
        int r = Random.Range(0, elderSignSpawnPoints.Length);
        Instantiate(elderSignPrefab, elderSignSpawnPoints[r].position, Quaternion.identity);
    }

    public void CreateArtifact()
    {
        int r = Random.Range(0, artifactSpawnPoints.Length);
        Instantiate(artifactPrefab, artifactSpawnPoints[r].position, Quaternion.identity);
    }
}


