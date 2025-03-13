using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyAnimator : MonoBehaviour
{
    [Header("Animator Settings")]
    public string speedAnimator = "Speed";
    
    private Animator animator;
    private NavMeshAgent agent;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    
    private void Update()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat(speedAnimator, speed);
    }
}
