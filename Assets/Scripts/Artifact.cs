using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{

    public float jumpDelay;     // after this time the artifact will respawn to a new location
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = jumpDelay;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;   
        if (timer <= 0)
        {
            GameManager.instance.CreateArtifact();
            Destruct();
        }
        
    }

    public void Destruct()
    {
        Destroy(gameObject);
    }
}
