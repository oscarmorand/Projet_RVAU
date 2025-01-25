using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LidarRayCasting : MonoBehaviourPun
{
    public ParticleSystem myParticleSystem;

    private LayerMask ignoreMask;

    public int packetSize = 10;
    float[] positions;
    byte[] types;
    int index;

    void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        ignoreMask = LayerMask.GetMask("IgnoreRaycast");

        positions = new float[packetSize * 3];
        types = new byte[packetSize];
        index = 0;
    }

    void Update()
    {

    }

    public void EmitParticle(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~ignoreMask))
        {
            var hitTag = hit.collider.tag;

            Vector3 position = hit.point;
            float color = 1f;
            float lifetime = 100f;
            byte type = 0;

            if (hitTag == "Danger")
            {
                type = 1;
                color = 0f;
                lifetime = 2f;
            }
            else if (hitTag == "Interactable")
            {
                type = 2;
                color = 0.1f;
                lifetime = 3f;
            }
            else if (hitTag == "Static Interactable")
            {
                type = 3;
                color = 0.1f;
                lifetime = 100f;
            }
            else if (hitTag == "Player")
            {
                type = 4;
                color = 0.2f;
                lifetime = 2f;
            }
            else if (hitTag == "Electrical")
            {
                type = 5;
                color = 0.3f;
                lifetime = 100f;
            }
            else if (hitTag == "Dynamic Env")
            {
                type = 6;
                color = 1f;
                lifetime = 4f;
            }

            // Emit particle to other clients
            //photonView.RPC("RPC_EmitParticle", RpcTarget.Others, position, type);
            if (index < packetSize)
            {
                positions[index * 3] = position.x;
                positions[index * 3 + 1] = position.y;
                positions[index * 3 + 2] = position.z;
                types[index] = type;
                index++;
            }
            else
            {
                photonView.RPC("RPC_EmitParticles", RpcTarget.Others, positions, types);
                index = 0;
            }

            // Emit particle locally
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();

            emitParams.applyShapeToPosition = false;
            emitParams.position = position;
            emitParams.startColor = new Color(0, 0, 0, color);
            emitParams.startLifetime = lifetime;

            myParticleSystem.Emit(emitParams, 1);
        }
    }

    [PunRPC]
    void RPC_EmitParticles(float[] positions, byte[] types)
    {
        for (int i = 0; i < packetSize; i++)
        {
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();

            emitParams.applyShapeToPosition = false;
            emitParams.position = new Vector3(
                positions[i * 3], 
                positions[i * 3 + 1],
                positions[i * 3 + 2]
            );

            byte type = types[i];

            // Environement
            float color = 1f;
            float lifetime = 100f;

            if (type == 1) // Danger
            {
                color = 0f;
                lifetime = 2f;
            }
            else if (type == 2) // Interactable
            {
                color = 0.1f;
                lifetime = 3f;
            }
            else if (type == 3) // Static Interactable
            {
                color = 0.1f;
                lifetime = 100f;
            }
            else if (type == 4) // Player
            {
                color = 0.2f;
                lifetime = 2f;
            }
            else if (type == 5) // Electrical
            {
                color = 0.3f;
                lifetime = 100f;
            }
            else if (type == 6) // Dynamic Env
            {
                color = 1f;
                lifetime = 4f;
            }

            emitParams.startColor = new Color(0, 0, 0, color);
            emitParams.startLifetime = lifetime;

            myParticleSystem.Emit(emitParams, 1);
        }
    }
}
