using UnityEngine;
using UnityEngine.EventSystems;

public class MoveAfterDelay : MonoBehaviour
{
    public Transform objectToMove = null;
    public float delayInSeconds = 0f;
    public Vector3 directionToMove = Vector3.zero;

    public bool useObjectLocalSpace = false;

    private float startTime = 0f;

    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        if (objectToMove == null)
        {
            objectToMove = this.transform;
        }

        startTime = Time.time;
        startPosition = objectToMove.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (delayInSeconds == 0f || Time.time - startTime > delayInSeconds)
        {
            objectToMove.Translate(directionToMove * Time.deltaTime, useObjectLocalSpace ? Space.Self : Space.World);
        } 
    }

    public void ResetStartTime()
    {
        startTime = Time.time;
    }

    public void ResetStartPosition()
    {
        objectToMove.transform.position = startPosition;
    }

    public void ResetStartPosition(Vector3 newStartPosition)
    {
        startPosition = newStartPosition;
        ResetStartPosition();
    }
    
}