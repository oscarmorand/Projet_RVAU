using UnityEngine;
using Photon.Pun;

public class VRController : MonoBehaviourPun
{
    public Camera playerCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        playerCamera.gameObject.SetActive(true);

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvas.worldCamera = playerCamera;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
