using UnityEngine;

public class Medikit : MonoBehaviour
{
    [Header("Medikit settings")]
    public int healAmount = 50;
    
    private void OnTriggerEnter(Collider other)
    {
        IHeal healable = other.GetComponent<IHeal>();
        
        if (healable != null)
        {
            healable.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
