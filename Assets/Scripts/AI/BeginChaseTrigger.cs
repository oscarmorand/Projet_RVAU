using UnityEngine;
using Photon.Pun;

public class BeginChaseTrigger : MonoBehaviourPun
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            chasePhase.StartChase();
        }
    }
}
