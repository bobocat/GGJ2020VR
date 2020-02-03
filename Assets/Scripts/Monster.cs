using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Player player;
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform.position);

        if (CanSeePlayer())
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            Debug.Log("moving to player");
        }

    }

    bool CanSeePlayer()
    {
        bool result = true;

        if (Physics.Linecast(transform.position, player.transform.position))
        {
            result = false;
        }

        return result;
    }
}
