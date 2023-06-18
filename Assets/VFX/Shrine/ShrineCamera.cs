using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShrineCamera : MonoBehaviour
{
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private Camera _shrineCameraA;
    [SerializeField] private Transform _shrineAAnchor;
    [SerializeField] private Camera _shrineCameraB;
    [SerializeField] private Transform _shrineBAnchor;

    void Update()
    {
        _shrineCameraA.transform.position = (_playerCamera.transform.position - _shrineBAnchor.transform.position) + _shrineAAnchor.transform.position;
        _shrineCameraA.transform.rotation = _playerCamera.rotation;
        _shrineCameraB.transform.position = (_playerCamera.transform.position - _shrineAAnchor.transform.position) + _shrineBAnchor.transform.position;
        _shrineCameraB.transform.rotation = _playerCamera.rotation;
    }
}
