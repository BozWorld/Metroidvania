using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float Speed;
    private PlayerInput _input;
    private Vector2 _rawMovement;
    private float m_JumpForce;
    [Range(1,10)]
    public float JumpVelocity;
    private bool _isJumping;
    public float FallMultiplier = 2.5f;
    public float lowJumpMultiplier = .2f;
    private Rigidbody2D _rb;

    private void Awake(){
        _rb = GetComponent<Rigidbody2D>();
        _input = new PlayerInput();
        _input.Player.Enable();
        _input.Player.Move.started += ctx => _rawMovement = ctx.ReadValue<Vector2> ();
        _input.Player.Move.canceled += _ => _rawMovement = Vector2.zero;
        _input.Player.Jump.started += ctx => Jump();
        _input.Player.Jump.canceled += ctx => _isJumping = false;        
    }

    private void FixedUpdate () {
        Move();
        FallModifier();
    }

    private void Move(){
        transform.position += (Vector3)_rawMovement * Speed * Time.deltaTime;
    }

    private void Jump(){
        _isJumping = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.up * JumpVelocity;
    }

    private void FallModifier(){
        if (_rb.velocity.y < 0){
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
        }
        else if (_rb.velocity.y > 0  && !_isJumping ){
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }


}
