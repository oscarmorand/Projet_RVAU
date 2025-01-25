using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class ExplosivePickable : MonoBehaviourPun
{
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
        photonView.RPC("RPC_ShowExplosive", RpcTarget.All);
    }

    public void OnPickupExit(SelectExitEventArgs args)
    {
        photonView.RPC("RPC_HideExplosive", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_ShowExplosive()
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
    public void RPC_HideExplosive()
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
