using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorKeyPickable : MonoBehaviour
{
    public GameObject pickupUI;

    bool isPickedUp = false;

    InputAction pickAction;

    private GameObject usedController;

    public BeginChaseEvent beginChaseEvent = null;

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

                if (beginChaseEvent != null)
                {
                    beginChaseEvent.OnEventStarted();
                }

                GameObject.Destroy(gameObject);
            }
        }
    }

    public void OnPickupEnter(SelectEnterEventArgs args)
    {
        var controller = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor;
        usedController = controller.gameObject.transform.parent.gameObject;

        pickupUI.SetActive(true);
        isPickedUp = true;
    }

    public void OnPickupExit(SelectExitEventArgs args)
    {
        pickupUI.SetActive(false);
        isPickedUp = false;
    }
}
