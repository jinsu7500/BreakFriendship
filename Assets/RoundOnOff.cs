using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundOnOff : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject RoundTriggerOn;
    [SerializeField]
    private GameObject RoundTriggerOff;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            RoundTriggerOn.SetActive(true);
            RoundTriggerOff.SetActive(false);
        }
    }
}
