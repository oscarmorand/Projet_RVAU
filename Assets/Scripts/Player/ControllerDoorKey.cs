using UnityEngine;

public class ControllerDoorKey : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject key;
    public enum Side
    {
        Left,
        Right
    };
    public Side side;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    string SideToString()
    {
        return side == Side.Left ? "LController" : "RController";
    }

    public void OnPickup()
    {
        mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(SideToString()));
        key.SetActive(true);
    }
}
