using UnityEngine;
using System.Collections;

public class WickExplosive : MonoBehaviour
{
    public GameObject flame;
    public GameObject explosionPrefab;
    public bool isOn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator WaitForExplosion()
    {
        isOn = true;

        flame.SetActive(true);
        flame.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(3);

        Explode();
    }

    public void Explode()
    {
        GameObject explosiveObj = gameObject.transform.parent.gameObject;

        GameObject explosion = Instantiate(explosionPrefab, explosiveObj.transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();

        GameObject.Destroy(explosiveObj);
    }
}
