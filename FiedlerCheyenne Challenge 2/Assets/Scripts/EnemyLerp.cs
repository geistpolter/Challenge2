using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLerp : MonoBehaviour
{
    public Transform start;
    public Transform end;

    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;

    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector2.Distance(start.position, end.position);
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;

        float fractionOfJourney = distCovered / journeyLength;

        transform.position = Vector2.Lerp(start.position, end.position, Mathf.PingPong(fractionOfJourney, 1));
    }
}
