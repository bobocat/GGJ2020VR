using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Artifact>() != null)
        {
            GameManager.instance.GrabArtifact(other.gameObject.name);
            other.GetComponent<Artifact>().Destruct();
        }
        else if (other.GetComponent<ElderSign>() != null)
        {
            GameManager.instance.GrabElderSign(other.gameObject);
        }
        else if (other.GetComponent<Monster>() != null)
        {
            GameManager.instance.TriggerEndGameLoss();
        }


    }
}
