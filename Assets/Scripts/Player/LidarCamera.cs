using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class LidarCamera : MonoBehaviourPun
{
    public int particleCount = 10; // Number of particles to spawn per update

    public Camera cam;

    InputAction castRayAction;

    public LidarRayCasting lidarRayCasting;

    void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        castRayAction = InputSystem.actions.FindAction("CastRay");
    }

    void Update()
    {
        // Cast rays and emit particles
        if (!photonView.IsMine)
        {
            return;
        }

        if (castRayAction.IsPressed())
        {
            CastRaysAndEmitParticles();
        }
    }

    void CastRaysAndEmitParticles()
    {
        for (int i = 0; i < particleCount; i++)
        {
            // Generate a random point in the camera's viewport space
            Vector3 randomPoint = new Vector3(Random.value, Random.value, 0);
            Ray ray = cam.ViewportPointToRay(randomPoint);

            lidarRayCasting.EmitParticle(ray);
        }
    }
}
