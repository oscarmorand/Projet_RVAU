using UnityEngine;
using Photon.Pun;

public class BeginChaseEvent : MonoBehaviourPun
{
    public ChasePhase chasePhase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEventStarted()
    {
        chasePhase.StartChase();
    }
}
