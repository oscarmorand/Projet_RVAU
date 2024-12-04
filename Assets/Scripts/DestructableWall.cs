using UnityEngine;

public class DestructableWall : MonoBehaviour
{
    public GameObject destructableWall;
    public GameObject fallingWall;

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
        if (other.gameObject.CompareTag("Explosive") || other.gameObject.name == "Dynamite")
        {
            WickExplosive wick = other.gameObject.GetComponent<WickExplosive>();
            if (wick.isOn)
            {
                Debug.Log("Explode wall");

                WickExplosive explosive = other.gameObject.GetComponent<WickExplosive>();
                explosive.Explode();
                GameObject.Destroy(destructableWall);

                fallingWall.SetActive(true);
            }
        }
    }
}
