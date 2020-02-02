using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFirebaseUnity;

public class DataManager : MonoBehaviour
{

    public static DataManager instance;
    GameManager gameManager;

    Firebase firebase = Firebase.CreateNew("https://lal-ggj2020.firebaseio.com");

    private FirebaseObserver observer;  // this is the observer that has events attached to it and watches the database for changes. we can turn it on and off

    FirebaseQueue firebaseQueue;

    private void Awake()
    {
        instance = this;
        firebase.OnGetFailed += GetFailHandler;
        firebaseQueue = new FirebaseQueue(true, 3, 1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetFailHandler(Firebase sender, FirebaseError err)
    {
        Debug.LogError("[ERR] get from key <" + sender.FullKey + ">, " + err.Message + " (" + (int)err.Status + ")");
    }

    public void WriteGateLevelToFirebase()
    {

        Dictionary<string, object> fbGame = new Dictionary<string, object>();

        fbGame.Add("gateLevel", gameManager.game.gateLevel);

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(fbGame);
        firebaseQueue.AddQueueUpdate(firebase.Child(gameManager.game.code, true), json);

    }

    public void WriteArtifact1ToFirebase()
    {
        Dictionary<string, object> fbGame = new Dictionary<string, object>();

        fbGame.Add("artifact1", "found");

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(fbGame);
        firebaseQueue.AddQueueUpdate(firebase.Child(gameManager.game.code, true), json);
    }


    public void WriteGameDataToFirebase()
    {
        Dictionary<string, object> fbGame = new Dictionary<string, object>();

        //        fbGame.Add("code", game.code);
        //        fbGame.Add("playerPosition", gameManager.game.playerPosition);
        fbGame.Add("gateLevel", gameManager.game.gateLevel);
        fbGame.Add("artifact1", gameManager.game.artifact1);
        fbGame.Add("artifact2", gameManager.game.artifact2);
        fbGame.Add("artifact3", gameManager.game.artifact3);

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(fbGame);
        firebaseQueue.AddQueueUpdate(firebase.Child(gameManager.game.code, true), json);
    }

    public void GetPlayerPositionFromFB()
    {
        firebase.Child(gameManager.game.code + "/playerPosition", true).GetValue();
//        game.playerPosition = 
    }

    public void UpdatePlayerPosition()
    {

    }


    public void StartListening()
    {
        observer = new FirebaseObserver(firebase.Child(gameManager.game.code + "/playerPosition"), 1f);
        observer.OnChange += (Firebase sender, DataSnapshot snapshot) =>
        {
            Dictionary<string, object> dict = snapshot.Value<Dictionary<string, object>>();

            float x = System.Convert.ToSingle(dict["x"]);
            float y = System.Convert.ToSingle(dict["y"]);
            float z = System.Convert.ToSingle(dict["z"]);

            gameManager.game.playerPosition = new Vector3(x, y, z);

//            gameManager.MovePlayer();

            //Debug.Log("playerposx: " + game.playerPosition);


/*
                        // are all the bets in?
                        if (answerCount == dict.Count)
                        {
                            observer.Stop();
                        }
            */
        };

        observer.Start();
    }

    public void StopListening()
    {
        observer.Stop();
    }

}
