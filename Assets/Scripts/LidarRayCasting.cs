using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarRayCasting : MonoBehaviour
{
    public int particleCount = 10; // Number of particles to spawn per update
    public ParticleSystem myParticleSystem; // Renamed field to avoid shadowing

    private Camera cam;
    private ParticleSystem.EmitParams emitParams; // Stores particle properties

    private LayerMask ignoreMask;

    void Start()
    {
        // Get the camera component on the same GameObject
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.LogError("No Camera component found on this GameObject.");
            return;
        }

        // Set up the emit parameters (particles will remain at the emission point)
        emitParams = new ParticleSystem.EmitParams();

        ignoreMask = LayerMask.GetMask("IgnoreRaycast");
    }

    void Update()
    {
        // Cast rays and emit particles
        CastRaysAndEmitParticles();
    }

    void CastRaysAndEmitParticles()
    {
        for (int i = 0; i < particleCount; i++)
        {
            // Generate a random point in the camera's viewport space
            Vector3 randomPoint = new Vector3(Random.value, Random.value, 0);
            Ray ray = cam.ViewportPointToRay(randomPoint);

            // Cast a ray from the camera in the chosen direction
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~ignoreMask))
            {
                var hitTag = hit.collider.tag;

             
                if (hitTag == "Danger")
                {
                    emitParams.startColor = new Color(0, 0, 0, 0f);
                    emitParams.startLifetime = 2f;
                }
                else if (hitTag == "Interactable")
                {
                    emitParams.startColor = new Color(0, 0, 0, 0.1f);
                    emitParams.startLifetime = 3f;
                }
                else if (hitTag == "Static Interactable")
                {
                    emitParams.startColor = new Color(0, 0, 0, 0.1f);
                    emitParams.startLifetime = 100f;
                }
                else if (hitTag == "Player")
                {
                    emitParams.startColor = new Color(0, 0, 0, 0.2f);
                    emitParams.startLifetime = 2f;
                }
                else if (hitTag == "Electrical")
                {
                    emitParams.startColor = new Color(0, 0, 0, 0.3f);
                    emitParams.startLifetime = 100f;
                }
                else if (hitTag == "Dynamic Env")
                {
                    emitParams.startColor = new Color(0, 0, 0, 1f);
                    emitParams.startLifetime = 2f;
                }
                else
                {
                    emitParams.startColor = new Color(0, 0, 0, 1f);
                    emitParams.startLifetime = 100f;
                }

                emitParams.applyShapeToPosition = false;
                emitParams.position = hit.point;

                myParticleSystem.Emit(emitParams, 1);
            }
        }
    }
}
