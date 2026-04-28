using UnityEngine;

public class DisplayWeapon : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    void Start()
    {
        //get model here later
    }

    void Update()
    {
        transform.Rotate(Vector3.fwd, rotationSpeed * Time.deltaTime);
    }
}
