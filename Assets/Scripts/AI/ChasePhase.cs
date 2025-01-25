using UnityEngine;
using Photon.Pun;

public class ChasePhase : MonoBehaviourPun
{
    public enum ChaseState
    {
        Inactive,
        NotStarted,
        Chasing,
        Ended
    }

    public GameObject spawnPoint;
    public GameObject monsterPrefab;

    public GameObject monster;

    public ChaseState state = ChaseState.Inactive;

    public ChasePhase next;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartChase()
    {
        photonView.RPC("RPC_StartChase", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void RPC_StartChase()
    {
        if (state != ChaseState.NotStarted) return;

        state = ChaseState.Chasing;

        Debug.Log("Chase started");

        monster = PhotonNetwork.Instantiate(monsterPrefab.name, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    public void EndChase()
    {
        photonView.RPC("RPC_EndChase", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void RPC_EndChase()
    {
        if (state != ChaseState.Chasing) return;

        state = ChaseState.Ended;

        Debug.Log("Chase ended");

        PhotonNetwork.Destroy(monster);

        if (next != null)
        {
            if (next.state == ChaseState.Inactive)
            next.state = ChaseState.NotStarted;
        }
    }
}
