using UnityEngine;
using Photon.Pun;

public class Ladder : MonoBehaviourPun
{
    public GameObject ladderClimable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GroundLadder()
    {
        Debug.Log("Ladder grounded");

        photonView.RPC("RPC_EnableClimableLadder", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_EnableClimableLadder()
    {
        ladderClimable.SetActive(true);
    }
}
