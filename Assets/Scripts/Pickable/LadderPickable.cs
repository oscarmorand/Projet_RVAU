using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class LadderPickable : MonoBehaviourPun
{
    public Ladder ladder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPickupEnter(SelectEnterEventArgs args)
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_ShowLadder", RpcTarget.All);
        }
        else
        {
            RPC_ShowLadder();
        }
    }

    public void OnPickupExit(SelectExitEventArgs args)
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_HideLadder", RpcTarget.All);
        }
        else
        {
            RPC_HideLadder();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            ladder.GroundLadder();

            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPC_HideLadder", RpcTarget.All);
            }
            else
            {
                RPC_HideLadder();
            }
        }
    }

    [PunRPC]
    public void RPC_ShowLadder()
    {
        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }
    }

    [PunRPC]
    public void RPC_HideLadder()
    {
        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }
    }
}
