using UnityEngine;
using UnityEngine.AI;

public class DroneTIlt : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] float tiltAmount;
    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }
    void Update()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);

        float tiltX = localVelocity.z * tiltAmount;
        float tiltZ = -localVelocity.x * tiltAmount;

        transform.rotation = Quaternion.Euler(tiltX, transform.eulerAngles.y, tiltZ);

    }

}
