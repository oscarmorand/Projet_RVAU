using UnityEngine;
using System.Collections;

public class Jumpscare : MonoBehaviour
{
    public GameObject monster;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartJumpscare()
    {
        monster.SetActive(true);
    }

    public void StopJumpscare()
    {
        monster.SetActive(false);
    }
}
