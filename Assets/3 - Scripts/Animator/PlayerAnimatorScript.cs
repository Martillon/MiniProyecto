using CharacterMovement;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerInput), typeof(Animator))]
public class PlayerAnimatorScript : MonoBehaviour
{
    
    [Header("Player animator settings")]
    public string horizontalAxisAnimator = "Horizontal";
    public string verticalAxisAnimator = "Vertical";
    public string speedAnimator = "Speed";

    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    
    private void Update()
    {
        animator.SetFloat(speedAnimator, GetSpeed());
        animator.SetFloat(horizontalAxisAnimator, Input.GetAxis("Horizontal"));
        animator.SetFloat(verticalAxisAnimator, Input.GetAxis("Vertical"));
    }
    
    public float GetSpeed()
    {
        return Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));
    }
    
}
