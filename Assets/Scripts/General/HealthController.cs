using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour, IDeath, IDamageable<float>
{
    public enum PawnType { Player, enemy,boss};
    public PawnType Type;
    public CharacterMovement Controller;
    public float _hp = 50f;
    private bool _IsColliding;

    private void Update() 
    {
        _IsColliding = false;
    }
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
        if(coll.CompareTag(Type.ToString()) && Controller.IsDashing)
        {
            if(_IsColliding) return;
            _IsColliding = true;
            Damage(10);
        }
    }
}