using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class LidarGunPickable : MonoBehaviour
{
    public GameObject pickupUI;

    bool isPickedUp = false;

    InputAction pickAction;

    public GameObject leftController;
    public GameObject rightController;
    private GameObject usedController;

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
                usedController.GetComponent<ControllerLidarGun>().OnPickup();
                isPickedUp = false;

                GameObject.Destroy(gameObject);
            }
        }
    }

    public void OnPickupEnter(SelectEnterEventArgs args)
    {
        var controller = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor;

        if (controller.gameObject.transform.parent.name.Contains("Left"))
        {
            usedController = leftController;
        }
        else
        {
            usedController = rightController;
        }

        pickupUI.SetActive(true);
        isPickedUp = true;
    }

    public void OnPickupExit(SelectExitEventArgs args)
    {
        pickupUI.SetActive(false);
        isPickedUp = false;
    }
}
