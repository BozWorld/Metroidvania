using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnHealth : MonoBehaviour, IDeath, IDamageable<float>
{
    public enum PawnType { player, enemy,boss};
    public PawnType Type;
    public SpriteRenderer sprite;

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
        if(coll.CompareTag("Player"))
        {
            Damage(10);
        }
    }
}
