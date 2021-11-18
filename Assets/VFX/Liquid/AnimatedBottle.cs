using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBottle : MonoBehaviour
{
    [SerializeField] private Vector3 initRotation;

    [SerializeField] private Vector3 rangeOfMoving;

    [SerializeField] private float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(initRotation + rangeOfMoving * Mathf.Sin(Time.time * speed));
    }
}
