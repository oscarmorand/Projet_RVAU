using UnityEngine;

public class ControllerLidarGun : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject lidarGun;
    public enum LidarGunSide 
    {
        Left,
        Right
    };
    public LidarGunSide side;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string lidarGunSideToString()
    {
        return side == LidarGunSide.Left ? "Left" : "Right";
    }

    public void OnPickup()
    {
        mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(lidarGunSideToString()));
        lidarGun.SetActive(true);
    }
}
