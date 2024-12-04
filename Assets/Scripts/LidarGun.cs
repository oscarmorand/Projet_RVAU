using UnityEngine;
using UnityEngine.InputSystem;

public class LidarGun : MonoBehaviour
{
    public enum LidarGunSide
    {
        Left,
        Right
    };
    public LidarGunSide side;
    public int particleCount = 10;
    public Transform emissionPointTransform;

    public ParticleSystem myParticleSystem;
    private ParticleSystem.EmitParams emitParams;

    InputAction castRayAction;
    InputAction castModeSwitchAction;

    public float sparseLineAngle = 60.0f;
    public float denseCircleAngle = 20.0f;

    private LayerMask ignoreMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (side == LidarGunSide.Left)
        {
            castRayAction = InputSystem.actions.FindAction("CastRayLeft");
            castModeSwitchAction = InputSystem.actions.FindAction("CastModeSwitchLeft");
        }
        else
        {
            castRayAction = InputSystem.actions.FindAction("CastRayRight");
            castModeSwitchAction = InputSystem.actions.FindAction("CastModeSwitchRight");
        }

        ignoreMask = LayerMask.GetMask("IgnoreRaycast");
    }

    // Update is called once per frame
    void Update()
    {
        if (castRayAction.IsPressed())
        {
            Vector3 emissionPoint = emissionPointTransform.position;
            Vector3 forward = emissionPointTransform.forward;

            if (castModeSwitchAction.IsPressed())
            {
                CastSparseLine(emissionPoint, forward);
            }
            else
            {
                CastDenseCircle(emissionPoint, forward);
            }
        }
    }

    void CastSparseLine(Vector3 emissionPoint, Vector3 forward)
    {
        for (int i = 0; i < particleCount; i++)
        {
            float randomAngle = Random.Range(-sparseLineAngle, sparseLineAngle);

            Quaternion rotation = Quaternion.AngleAxis(randomAngle, emissionPointTransform.up);
            Vector3 direction = rotation * forward;

            Ray ray = new Ray(emissionPoint, direction);
            CastRayAndEmitParticle(ray);
        }
    }

    void CastDenseCircle(Vector3 emissionPoint, Vector3 forward)
    {
        for (int i = 0; i < particleCount; i++) 
        {
            float randomR = Mathf.Sqrt(Random.Range(0.0f, 1.0f)) * denseCircleAngle;
            float randomTheta = 360.0f * Random.Range(0.0f, 1.0f);

            Quaternion rRotation = Quaternion.AngleAxis(randomR, emissionPointTransform.right);
            Quaternion thetaRotation = Quaternion.AngleAxis(randomTheta, forward);

            Vector3 direction = thetaRotation * rRotation * forward;

            Ray ray = new Ray(emissionPoint, direction);
            CastRayAndEmitParticle(ray);
        }
    }

    public void CastRayAndEmitParticle(Ray ray)
    {
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
            else if (hitTag == "Player")
            {
                emitParams.startColor = new Color(0, 0, 0, 0.2f);
                emitParams.startLifetime = 2f;
            }
            else
            {
                emitParams.startColor = new Color(0, 0, 0, 1f);
                emitParams.startLifetime = 100f;
            }

            emitParams.position = hit.point;
            emitParams.applyShapeToPosition = false;

            myParticleSystem.Emit(emitParams, 1);
        }
    }
}
