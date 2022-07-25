using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour, IDeath, IDamageable<float>
{
    public enum PawnType { Player, enemy,boss};
    public PawnType TypeToHit;

    [SerializeField] private float _hp = 50f;
        
    public void Damage(float damageReceived)
    {
        _hp -= damageReceived;
    }

    public void Death()
    {
        if(_hp <= 0)
        {
            Debug.Log("mort");
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag(TypeToHit.ToString()))
        {
            Damage(10);
        }
    }
}
