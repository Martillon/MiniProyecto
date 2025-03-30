using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    #region Variables
    
    [Header("Enemy Settings")]
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public bool isRanged;
    public float stoppingDistance = 2f;
    public bool canAttack = true;
    public bool isAttacking;
    public GameObject[] weapons;
    
    [Header("Patrol Settings")]
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    
    [Header("Melee Attack Settings")]
    public float meleeAttackCooldown = 1f;
    public int meleeDamage = 10;
    
    [Header("Ranged Attack Settings")]
    public float rangedAttackCooldown = 1f;
    public int rangedDamage = 5;
    public float fireRange = 15f;
    public float accuracy = 0.1f;
    
    [Header("State Settings")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    
    [Header("Animator Settings")]
    public string gunAnimatorVariable = "Gun";
    public string attackAnimatorVariable = "Attack";
    public string movementAnimatorVariable = "Movement";
    
    private Animator _animator;
    private NavMeshAgent agent;
    
    private Vector3 originalWeaponPosition;
    private Quaternion originalWeaponRotation;
    
    #endregion
    
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        
        
    }

    private void Start()
    {
        if (isRanged)
        {
            _animator.SetBool(gunAnimatorVariable, true);
            weapons[0].SetActive(false);
            weapons[1].SetActive(true);
        }
        else if (!isRanged)
        {
            _animator.SetBool(gunAnimatorVariable, false);
            weapons[0].SetActive(true);
            weapons[1].SetActive(false);
        }
        else
        {
            Debug.LogWarning("Error while setting up the enemy for weapons. Please check the inspector.");
        }
        
        originalWeaponPosition = weapons[1].transform.localPosition;
        originalWeaponRotation = weapons[1].transform.localRotation;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        if (!playerInSightRange && !playerInAttackRange) Patrol();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
        
        if(_animator != null)
        {
            _animator.SetBool(movementAnimatorVariable, agent.velocity.magnitude > 0.1f);
        }
    }
    
    void LateUpdate()
    {
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.x = 0f; 
        currentRotation.z = 0f; 
        transform.eulerAngles = currentRotation;
        
        if (playerInSightRange && !playerInAttackRange) transform.LookAt(player);
    }
    
    private void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();
        
        if (walkPointSet)
            agent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }
    
    private void ChasePlayer()
    {
        Debug.Log("Chase Player");
        agent.SetDestination(player.position);
        transform.LookAt(player.position);
        
        if (isRanged)
        {
            _animator.SetBool(attackAnimatorVariable, false);
            weapons[1].transform.localPosition = originalWeaponPosition;
            weapons[1].transform.localRotation = originalWeaponRotation;
        }
        
    }
    
    private void AttackPlayer()
    {
        Debug.Log("Attack Player");
        
        transform.LookAt(player.position);

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        
        if (canAttack)
        {
            StartCoroutine(isRanged ? RangedAttack() : MeleeAttack());
        }

        if (isRanged)
        {
            _animator.SetBool(attackAnimatorVariable, true);
            weapons[1].transform.localPosition = new Vector3(34, -1, 20);
            weapons[1].transform.localRotation = Quaternion.Euler(1, -125, 120);
        }
        
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        canAttack = false;

        if (_animator != null)
        {
            _animator.SetBool(attackAnimatorVariable, true);
        }

        if (Vector3.Distance(transform.position, player.position) <= stoppingDistance)
        {
            IDamageable damageable = player.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(GetDamage());
            }
        }
        
        yield return new WaitForSeconds(meleeAttackCooldown);
        canAttack = true;
        isAttacking = false;
        agent.isStopped = false;
    }

    private IEnumerator RangedAttack()
    {
        isAttacking = true;
        canAttack = false;

        yield return new WaitForSeconds(0.2f);

        Vector3 direction = (player.position - transform.position).normalized;

        // Disperse the direction
        direction.x += Random.Range(-accuracy, accuracy);
        direction.y += Random.Range(-accuracy, accuracy);

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, fireRange, whatIsPlayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(GetDamage());
                }
            }
        }

        yield return new WaitForSeconds(rangedAttackCooldown);
        canAttack = true;
        isAttacking = false;
        if (_animator != null)
        {
            _animator.SetBool(attackAnimatorVariable, false);
        }
        agent.isStopped = false;
    }

    private int GetDamage()
    {
        if(isRanged) return rangedDamage;
        return meleeDamage;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
