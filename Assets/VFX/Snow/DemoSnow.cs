using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSnow : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float speed;

    private Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time * speed), 0, Mathf.Cos(Time.time * speed)) * range + origin;
    }
}
