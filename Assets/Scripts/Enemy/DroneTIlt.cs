using UnityEngine;
using UnityEngine.AI;

public class DroneTIlt : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] float tiltAmount;
    [SerializeField] float rotationSpeed;
    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }
    void LateUpdate()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);

        float tiltX = localVelocity.z * tiltAmount;
        float tiltZ = -localVelocity.x * tiltAmount;

        Vector3 flatVelocity = new Vector3(agent.velocity.x, 0, agent.velocity.z);

        if (flatVelocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(flatVelocity);
            Quaternion smoothRot = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.Euler(
                tiltX,
                smoothRot.eulerAngles.y,
                tiltZ
            );
        }
    }


}
