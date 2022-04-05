using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private Transform hourPivot;
    [SerializeField] private Transform minutePivot;
    [SerializeField] private Transform secondPivot;

    private const float hoursToDegrees = -30;
    private const float minuteToDegrees = -6;
    private const float secondToDegrees = -6;

    void Update()
    {
        TimeSpan time = DateTime.Now.TimeOfDay;
        Debug.Log(time);
        hourPivot.localRotation = Quaternion.Euler(0, 0, hoursToDegrees * (float)time.TotalHours);
        minutePivot.localRotation = Quaternion.Euler(0, 0, minuteToDegrees * (float)time.TotalMinutes);
        secondPivot.localRotation = Quaternion.Euler(0, 0, secondToDegrees * (float)time.TotalSeconds);
    }
}
