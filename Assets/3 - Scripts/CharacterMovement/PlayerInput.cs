using UnityEngine;
using UnityEngine.InputSystem;

namespace CharacterMovement
{
    [RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
    public class PlayerInput : MonoBehaviour
    {
        #region Variables
        [Header("Movement")]
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float groundedDrag = 6f;
        
        [Header("Jumping")]
        [SerializeField] private float jumpForce = 5f;
        //[SerializeField] private float airMultiplier = 0.4f;
        [SerializeField] private float jumpCooldown = 2f;
        private bool _canJump = true;
        
        [Header("Dashing")]
        [SerializeField] private float dashForce = 10f;
        [SerializeField] private float dashCooldown = 2f;
        private bool _canDash = true;
        
        [Header("Keybindings")]
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;
        
        [Header("Ground Check")]
        [SerializeField] private float playerHeight;
        [SerializeField] private LayerMask groundMask;
        private bool _isGrounded;

        [Header("Weapons")] 
        [SerializeField] private WeaponManager weaponManager;
        [SerializeField] private KeyCode switchWeaponKey = KeyCode.Q;
        [SerializeField] private KeyCode reloadKey = KeyCode.R;
        
        [SerializeField] private Transform orientation;
        
        [Header("Audio")]
        private AudioSource audioSource;
        public AudioClip jumpSound;
        public AudioClip dashSound;
        
        private Camera _playerCamera;

        private float _horizontal;
        private float _vertical;
        
        private bool _speedControlActive = true;

        private Quaternion _camRotation;
        
        private Vector3 _moveDirection;
        private Vector3 _forward;
        private Vector3 _right;
        private Vector3 _rbRotation;

        private Rigidbody _rb;
        
        #endregion
        
        #region Updates & Start
        
        private void Start()
        {
            _playerCamera = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
            weaponManager = GetComponent<WeaponManager>();
            audioSource = GetComponent<AudioSource>();
        }
        
        private void Update()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
            MyInput();
            WeaponInput();
            RotationCam();
            if (_speedControlActive) SpeedControl();
            
            if (_isGrounded)
            {
                _rb.linearDamping = groundedDrag;
            }
            else
            {
                _rb.linearDamping = 0;
            }
        }

        private void FixedUpdate()
        {
            MovePLayer();

        }
        
        #endregion
        
        private void MyInput()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(jumpKey) && _canJump && _isGrounded)
            {
                _canJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }

            if (Input.GetKey(dashKey) && _canDash)
            {
                _speedControlActive = false;
                
                _canDash = false;

                Dash();

                Invoke(nameof(ResetDash), dashCooldown);
                Invoke(nameof(ResetSpeedControl), 0.5f);
            }
        }

        private void WeaponInput()
        {

            if (weaponManager != null)
            {
                // Booth shoot
                if (Input.GetKeyDown(KeyCode.Mouse0)) // First one
                {
                    weaponManager.SetShootingState(true);
                }

                // If automatic, keep shooting
                if (Input.GetKey(KeyCode.Mouse0) && weaponManager.GetCurrentWeapon())
                {
                    weaponManager.SetShootingState(true);
                }
                else
                {
                    weaponManager.SetShootingState(false);
                }

                // Change weapon
                if (Input.GetKeyDown(switchWeaponKey) || Input.mouseScrollDelta.y != 0)
                {
                    weaponManager.SwitchWeapon();
                }
                
                // Reload weapon
                if (Input.GetKeyDown(reloadKey))
                {
                    weaponManager.ReloadWeapon();
                }
            }
            else
            {
                Debug.LogWarning("No weapon manager found on player");
            }
        }

        private void RotationCam()
        {
            _camRotation = _playerCamera.transform.rotation;
            _camRotation.eulerAngles = new Vector3(0f, _camRotation.eulerAngles.y, 0);
            _rb.rotation = _camRotation;

            _rbRotation = _rb.transform.rotation.eulerAngles;
        }
        
        private void MovePLayer()
        {
            _forward = Quaternion.Euler(0f, _rbRotation.y, 0f)*Vector3.forward;
            _right = Quaternion.Euler(0f, _rbRotation.y, 0f) * Vector3.right;

            _moveDirection = _forward * _vertical + _right * _horizontal;

            _rb.AddForce(_moveDirection.normalized * (movementSpeed * 10f), ForceMode.Force);
        }

        private void SpeedControl()
        {
            Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

            if (flatVel.magnitude > movementSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * movementSpeed;
                _rb.linearVelocity = new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
            }
            
        }

        #region Actions
        
        private void Jump()
        {
            audioSource.volume = 1f;
            audioSource.PlayOneShot(jumpSound);
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void Dash()
        {
            audioSource.volume = 1f;
            audioSource.PlayOneShot(dashSound);
            
            _forward = Quaternion.Euler(0f, _rbRotation.y, 0f) * Vector3.forward;
            _right = Quaternion.Euler(0f, _rbRotation.y, 0f) * Vector3.right;

            _moveDirection = _forward * _vertical + _right * _horizontal;

            
            _rb.AddForce(_moveDirection.normalized* dashForce, ForceMode.Impulse);
        }
        
        #endregion

        #region Resets
        
        private void ResetJump()
        {
            _canJump = true;
        }

        private void ResetDash()
        {
            _canDash = true;
            
        }

        private void ResetSpeedControl()
        {
            _speedControlActive = true;
        }
        
        #endregion
            
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
