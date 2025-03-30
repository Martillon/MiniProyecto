using UnityEngine;

public class FallPlane : MonoBehaviour
{
    [Header("Fall plane")] 
    
    public bool isDeadly;
    public GameObject tpSpot;
    
    private void OnTriggerEnter(Collider other)
    {
        if(isDeadly){
            if (other.GetComponent<IDamageable>() != null)
            {
                other.GetComponent<IDamageable>().TakeDamage(1000);
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                other.transform.position = tpSpot.transform.position;
            }
        }
    }
}
