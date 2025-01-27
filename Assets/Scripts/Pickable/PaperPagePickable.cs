using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class PaperPagePickable : MonoBehaviourPun
{
    public GameObject pickupUI;
    public GameObject pageUI;
    public GameObject CancelUI;

    bool isPickedUp = false;
    bool isReading = false;

    InputAction pickAction;
    InputAction cancelAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickAction = InputSystem.actions.FindAction("Pick");
        cancelAction = InputSystem.actions.FindAction("Cancel");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            if (pickAction.IsPressed())
            {
                pageUI.SetActive(true);
                pickupUI.SetActive(false);
                CancelUI.SetActive(true);

                isReading = true;
            }
        }

        if (isReading)
        {
            if (cancelAction.IsPressed())
            {
                pageUI.SetActive(false);
                CancelUI.SetActive(false);
                isReading = false;
            }
        }
    }

    public void OnPickupEnter(SelectEnterEventArgs args)
    {
        pickupUI.SetActive(true);
        isPickedUp = true;

        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            photonView.RPC("RPC_ShowPaperPage", RpcTarget.All);
        }
        else
        {
            // Offline behavior
            RPC_ShowPaperPage();
        }
    }

    public void OnPickupExit(SelectExitEventArgs args)
    {
        pickupUI.SetActive(false);
        isPickedUp = false;

        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            photonView.RPC("RPC_HidePaperPage", RpcTarget.All);
        }
        else
        {
            // Offline behavior
            RPC_HidePaperPage();
        }
    }

    [PunRPC]
    public void RPC_ShowPaperPage()
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
    public void RPC_HidePaperPage()
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
