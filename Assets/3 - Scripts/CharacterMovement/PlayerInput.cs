using System;
using UnityEngine;

namespace CharacterMovement
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerInput : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float groundedDrag = 6f;
        
        [Header("Jumping")]
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float airMultiplier = 0.4f;
        [SerializeField] private float jumpCooldown = 2f;
        private bool _canJump = true;
        
        [Header("Keybindings")]
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        
        [Header("Ground Check")]
        [SerializeField] private float playerHeight;
        [SerializeField] private LayerMask groundMask;
        private bool _isGrounded;
        
        [SerializeField] private Transform orientation;
        
        private Camera _playerCamera;

        private float _horizontal;
        private float _vertical;
        
        private Vector3 _moveDirection;

        private Rigidbody _rb;

        private void Start()
        {
            _playerCamera = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }
        
        private void Update()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.2f, groundMask);
            
            MyInput();
            SpeedControl();
            AlignWithCamera();
            
            if(_isGrounded)
                _rb.linearDamping = groundedDrag;
            else
                _rb.linearDamping = 0;
        }

        private void MyInput()
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");

            if (Input.GetKey(jumpKey) && _canJump && _isGrounded)
            {
                Debug.Log("Jump");
                Jump(); 
                _canJump = false;
                Invoke(nameof(ResetJump), jumpCooldown);
            }
                
        }
        
        private void MovePlayer()
        {
            _moveDirection = orientation.forward * _vertical + orientation.right * _horizontal;
            
            if(_isGrounded) 
                _rb.AddForce(_moveDirection.normalized * (movementSpeed * 10f), ForceMode.Force);
            else if(!_isGrounded)
                _rb.AddForce(_moveDirection.normalized * (movementSpeed * airMultiplier * 10f), ForceMode.Force);
        }

        private void SpeedControl()
        {
            Vector3 flatVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            
            if(flatVelocity.magnitude > movementSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
                _rb.linearVelocity = new Vector3(limitedVelocity.x, _rb.linearVelocity.y, limitedVelocity.z);
            }
        }
        
        private void Jump()
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            _canJump = true;
        }
        
        private void AlignWithCamera()
        {
            if (_horizontal != 0 || _vertical != 0)
            {
                Vector3 cameraForward = _playerCamera.transform.forward;
                cameraForward.y = 0; 
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, Vector3.down * (playerHeight / 2 + 0.2f));

            if (_moveDirection != Vector3.zero)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, _rb.linearVelocity.normalized);
            }
            
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, orientation.forward);
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * 2);
        }
    }
}
