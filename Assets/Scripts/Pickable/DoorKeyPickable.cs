using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class DoorKeyPickable : MonoBehaviourPun
{
    public GameObject pickupUI;

    bool isPickedUp = false;

    InputAction pickAction;

    private GameObject usedController;

    public BeginChaseEvent beginChaseEvent = null;

    public AudioSource pickedSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickAction = InputSystem.actions.FindAction("Pick");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            if (pickAction.IsPressed())
            {
                pickupUI.SetActive(false);
                usedController.GetComponent<ControllerDoorKey>().OnPickup();
                isPickedUp = false;

                StartCoroutine(StartChasing());

                if (PhotonNetwork.IsConnected)
                {
                    photonView.RPC("RPC_PlayPickedSound", RpcTarget.All);
                }
                else
                {
                    RPC_PlayPickedSound();
                }

                //GameObject.Destroy(gameObject);
            }
        }
    }

    IEnumerator StartChasing()
    {
        yield return new WaitForSeconds(2);
        if (beginChaseEvent != null)
        {
            beginChaseEvent.OnEventStarted();
        }
    }

    public void OnPickupEnter(SelectEnterEventArgs args)
    {
        var controller = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor;
        usedController = controller.gameObject.transform.parent.gameObject;

        pickupUI.SetActive(true);
        isPickedUp = true;

        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_ShowKey", RpcTarget.All);
        }
        else
        {
            RPC_ShowKey();
        }
    }

    public void OnPickupExit(SelectExitEventArgs args)
    {
        pickupUI.SetActive(false);
        isPickedUp = false;

        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_HideKey", RpcTarget.All);
        }
        else
        {
            RPC_HideKey();
        }
    }

    [PunRPC]
    public void RPC_ShowKey()
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
    public void RPC_HideKey()
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

    [PunRPC]
    public void RPC_PlayPickedSound()
    {
        pickedSound.Play();
    }
}
