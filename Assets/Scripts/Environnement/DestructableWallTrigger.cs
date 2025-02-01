using UnityEngine;

public class DestructableWallTrigger : MonoBehaviour
{

    public DestructableWall destructableWall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (destructableWall.destroyed) return;

        if (other.gameObject.CompareTag("Explosive") || other.gameObject.name == "Dynamite")
        {
            Debug.Log("An explosive ented the wall");

            WickExplosive wick = other.gameObject.transform.parent.GetChild(0).gameObject.GetComponent<WickExplosive>();
            if (wick.isOn)
            {
                Debug.Log("Explode wall");

                wick.Explode();

                destructableWall.DestroyWall();
            }
        }
    }
}
