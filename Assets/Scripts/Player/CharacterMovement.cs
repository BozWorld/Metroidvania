using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float Speed;
    private PlayerInput _input;
    private Vector2 _rawMovement;
    private float m_JumpForce;
    private bool _isJumping;

    private void Awake(){
        _input = new PlayerInput();
        _input.Player.Enable();
        _input.Player.Move.started += ctx => _rawMovement = ctx.ReadValue<Vector2> ();
        _input.Player.Move.canceled += _ => _rawMovement = Vector2.zero;
        _input.Player.Jump.started += ctx => Jump();
        _input.Player.Jump.canceled += ctx => _isJumping = false;        
    }

    private void FixedUpdate () {
        Move();
    }

    private void Move(){
        transform.position += (Vector3)_rawMovement * Speed * Time.deltaTime;
    }

    private void Jump(){

    }

    
}
