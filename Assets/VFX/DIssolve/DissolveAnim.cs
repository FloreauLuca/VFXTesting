using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveAnim : MonoBehaviour
{
    private Material mat;

    [SerializeField] private float speed;

    [SerializeField] private float timeStop;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        float percent = Mathf.Sin(speed * Time.time);
        percent = Mathf.Clamp(percent * timeStop, 0, 1);
        mat.SetFloat("Dissolve", percent);
    }
}
