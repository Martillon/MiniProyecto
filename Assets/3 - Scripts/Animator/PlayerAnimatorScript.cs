using CharacterMovement;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public class PlayerAnimatorScript : MonoBehaviour
{
    
    [Header("Player animator settings")]
    public string horizontalAxisAnimator = "Horizontal";
    public string verticalAxisAnimator = "Vertical";
    public string speedAnimator = "Speed";
    
    [Header("IK settings")]
    public TwoBoneIKConstraint leftHandIK;
    public TwoBoneIKConstraint rightHandIK;
    
    private Animator animator;
    private RigBuilder rigBuilder;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigBuilder = GetComponent<RigBuilder>();
    }
    
    private void Update()
    {
        animator.SetFloat(speedAnimator, GetSpeed());
        animator.SetFloat(horizontalAxisAnimator, Input.GetAxis("Horizontal"));
        animator.SetFloat(verticalAxisAnimator, Input.GetAxis("Vertical"));
    }
    
    public void AssignIK (RightHandIkTarget rightHandTarget, LeftHandIkTarget leftHandTarget)
    {
        if (rightHandTarget != null)
        {
            rightHandIK.data.target = rightHandTarget.transform;
            rightHandIK.weight = 1f;
        }

        if (leftHandTarget != null)
        {
            leftHandIK.data.target = leftHandTarget.transform;
            leftHandIK.weight = 1f;
        }
        rigBuilder.Build();
    }
    
    private float GetSpeed()
    {
        return Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));
    }
    
}
