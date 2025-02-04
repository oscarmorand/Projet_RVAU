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
        if (state == ChaseState.Chasing || state == ChaseState.Ended)
        {
            return;
        }
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_StartChase", RpcTarget.MasterClient);
        }
        else
        {
            RPC_StartChase();
        }
    }

    [PunRPC]
    public void RPC_StartChase()
    {
        if (state != ChaseState.NotStarted) return;

        if (state == ChaseState.Chasing || state == ChaseState.Ended)
        {
            return;
        }

        state = ChaseState.Chasing;

        Debug.Log("Chase started");

        if (PhotonNetwork.IsConnected)
        {
            monster = PhotonNetwork.Instantiate(monsterPrefab.name, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
        else
        {
            monster = Instantiate(monsterPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }

    public void EndChase()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_EndChase", RpcTarget.MasterClient);
        }
        else
        {
            RPC_EndChase();
        }
    }

    [PunRPC]
    public void RPC_EndChase()
    {
        if (state != ChaseState.Chasing) return;

        state = ChaseState.Ended;

        Debug.Log("Chase ended");

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Destroy(monster);
        }
        else
        {
            Destroy(monster);
        }

        if (next != null)
        {
            if (next.state == ChaseState.Inactive)
            next.state = ChaseState.NotStarted;
        }
    }
}
