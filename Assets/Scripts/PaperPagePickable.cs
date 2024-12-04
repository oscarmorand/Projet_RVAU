using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class PaperPagePickable : MonoBehaviour
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
        pickupUI.SetActive(false);
        isPickedUp = false;

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
