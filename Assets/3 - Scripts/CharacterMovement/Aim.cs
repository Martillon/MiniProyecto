using System;
using Unity.Cinemachine;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [Header("Aim Settings")]
    [SerializeField] private KeyCode aimKey = KeyCode.Mouse1;
    //[SerializeField] private float aimSpeed = 0.1f;
    [SerializeField] private int priority = 10;
    
    private CinemachineCamera _aimCamera;

    private void Start()
    {
        _aimCamera = GetComponent<CinemachineCamera>();
    }
    
    private void Update()
    {
        if (Input.GetKey(aimKey))
        {
            _aimCamera.Priority = priority;
        }
        else
        {
            _aimCamera.Priority = 0;
        }
    }
}
