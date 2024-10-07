using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    bool hit = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.name == "Player")
        {
            if (!hit)
            {
                PlayerMovement player = GameObject.Find("Player").GetComponent<PlayerMovement>();
                player.addHp();
                hit = true;
            }
        }
    }
}
