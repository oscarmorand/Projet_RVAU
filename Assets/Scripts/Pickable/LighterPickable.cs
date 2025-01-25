using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class LighterPickable : MonoBehaviourPun
{
    public GameObject useLighterUI;
    public GameObject flame;
    public GameObject flameVFX;
    public GameObject mesh;

    bool isPickedUp = false;
    public bool isOn = false;

    InputAction useAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        useAction = InputSystem.actions.FindAction("Pick");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            if (useAction.IsPressed())
            {
                if (!isOn)
                {
                    useLighterUI.SetActive(false);
                    photonView.RPC("RPC_LighterOn", RpcTarget.All);
                }
            }
            else
            {
                if (isOn)
                {
                    useLighterUI.SetActive(true);
                    photonView.RPC("RPC_LighterOff", RpcTarget.All);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOn)
        {
            if (other.gameObject.CompareTag("Explosive"))
            {
                WickExplosive explosive = other.gameObject.GetComponent<WickExplosive>();
                explosive.StartExplosion();
            }
        }
    }

    public void OnPickupEnter(SelectEnterEventArgs args)
    {
        isPickedUp = true;
        useLighterUI.SetActive(true);

        photonView.RPC("RPC_ShowLighter", RpcTarget.All);
    }

    public void OnPickupExit(SelectExitEventArgs args)
    {
        isPickedUp = false;
        useLighterUI.SetActive(false);

        photonView.RPC("RPC_HideLighter", RpcTarget.All);
        photonView.RPC("RPC_LighterOff", RpcTarget.All);
    }

    [PunRPC]
    void RPC_ShowLighter()
    {
        foreach (Transform child in mesh.transform)
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
    void RPC_HideLighter()
    {
        foreach (Transform child in mesh.transform)
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
    void RPC_LighterOn()
    {
        flame.SetActive(true);
        flameVFX.GetComponent<ParticleSystem>().Play();
        isOn = true;
    }

    [PunRPC]
    void RPC_LighterOff()
    {
        flame.SetActive(false);
        isOn = false;
    }
}
