using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class LighterPickable : MonoBehaviour
{
    public GameObject useLighterUI;
    public GameObject flame;

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
                useLighterUI.SetActive(false);
                flame.SetActive(true);
                if (!isOn)
                    flame.GetComponent<ParticleSystem>().Play();
                isOn = true;

            }
            else
            {
                useLighterUI.SetActive(true);
                flame.SetActive(false);
                isOn = false;
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
                StartCoroutine(explosive.WaitForExplosion());
            }
        }
    }

    public void OnPickupEnter(SelectEnterEventArgs args)
    {
        isPickedUp = true;
        useLighterUI.SetActive(true);

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

    public void OnPickupExit(SelectExitEventArgs args)
    {
        isPickedUp = false;
        useLighterUI.SetActive(false);

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
