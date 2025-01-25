using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    void Update()
    {
        transform.position = Vector3.Lerp (transform.position, target.position, Time.deltaTime * speed);
    }
}
