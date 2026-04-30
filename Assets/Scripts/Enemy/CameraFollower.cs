using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject camera;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.position = this.transform.position;
    }
}
