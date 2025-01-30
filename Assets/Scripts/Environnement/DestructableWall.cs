using UnityEngine;
using Photon.Pun;

public class DestructableWall : MonoBehaviourPun
{
    public GameObject destructableWall;
    public GameObject fallingWall;

    public bool destroyed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destructableWall.SetActive(true);
        fallingWall.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyWall()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_DestroyWall", RpcTarget.All);
        }
        else
        {
            RPC_DestroyWall();
        }
    }

    [PunRPC]
    public void RPC_DestroyWall()
    {
        destroyed = true;
        GameObject.Destroy(destructableWall);
        fallingWall.SetActive(true);
    }
}
