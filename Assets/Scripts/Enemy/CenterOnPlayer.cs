using UnityEngine;

public class CenterOnPlayer : MonoBehaviour

{

    Transform playerTransform;
    [SerializeField] float heightOverPlayer;
    Vector3 heightDelta;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        heightDelta = new Vector3(0, heightOverPlayer, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + heightDelta;
    }
}
