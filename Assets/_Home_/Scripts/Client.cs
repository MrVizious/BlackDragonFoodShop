using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    public bool isSeen = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("unmask"))
        {
            isSeen = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("unmask"))
        {
            isSeen = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("unmask"))
        {
            isSeen = false;
        }
    }
}
