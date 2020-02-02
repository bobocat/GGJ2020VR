using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    DataManager dataManager;

    public string gameID;   // if this is set then it will use it
    public Game game;

    public GameObject artifactPrefab;
    public GameObject elderSignPrefab;
    public Transform[] artifactSpawnPoints;
    public Transform[] elderSignSpawnPoints;

    private Player player;

    public float gateTimer = 50;
    public bool timerRunning = false;

    public Slider gateLevelSlider;
    public GameObject failScreen;
    public GameObject winScreen;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        dataManager = FindObjectOfType<DataManager>();
        player = FindObjectOfType<Player>();

        game = new Game();
        CreateNewGame(gameID);
        CreateArtifact();

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

        game.artifact1 = "";
        game.artifact2 = "";
        game.artifact3 = "";
        game.gateLevel = 50;

        dataManager.WriteGameDataToFirebase();

        // instantiate the player
//        player = Instantiate(playerPrefab);
//        player.transform.position = game.playerPosition;
    }

    public void GrabArtifact(string name)
    {
        AudioBank.instance.PlayClip(0);
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

        if (count == 0)
        {
            CreateElderSign();
        }
    }

    public void CreateElderSign()
    {
        int r = Random.Range(0, elderSignSpawnPoints.Length);
        Instantiate(elderSignPrefab, elderSignSpawnPoints[r].position, Quaternion.identity);
    }

    public void CreateArtifact()
    {
        int r = Random.Range(0, artifactSpawnPoints.Length);
        Instantiate(artifactPrefab, artifactSpawnPoints[r].position, Quaternion.identity);
    }
}


