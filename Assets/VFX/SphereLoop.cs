using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereLoop : MonoBehaviour
{
    [SerializeField] private float range = 1;
    [SerializeField] private float speed = 1;
    [SerializeField] private float verticalRange = 0;
    [SerializeField] private float verticalSpeed = 0;

    private Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time * speed) * range, Mathf.Sin(Time.time * verticalSpeed) * verticalRange, Mathf.Cos(Time.time * speed) * range)  + origin;
    }
}
