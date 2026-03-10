using UnityEngine;

public class SimpleMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool shouldMove = false;
    public Vector3 moveDistance = new Vector3(5, 0, 0); // How far to go
    public float moveSpeed = 2f;

    [Header("Rotation Settings")]
    public bool shouldRotate = false;
    public Vector3 rotationSpeed = new Vector3 (0, 50, 0);

    private Vector3 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            float moveFactor = Mathf.PingPong(Time.time * moveSpeed, 1f);
            transform.position = Vector3.Lerp(startPos, startPos + moveDistance, moveFactor);
        }
        if (shouldRotate)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }
    }
}
