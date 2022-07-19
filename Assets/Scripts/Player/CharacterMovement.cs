using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    public float Speed;
    private PlayerInput _input;
    private Vector2 _rawMovement;

    [Range(1,10)]
    public float JumpVelocity;
    private bool _isJumping;
    public float FallMultiplier = 2.5f;
    public float lowJumpMultiplier = .2f;
    private Rigidbody2D _rb;
    public bool canJump;

    public BoxCollider2D ground;
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
        IsGrounded();
    }

    private void Move(){
        transform.position += (Vector3)_rawMovement * Speed * Time.deltaTime;
    }

    private void Jump(){
        if(IsGrounded())
            {
                _isJumping = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.up * JumpVelocity;
            }
    }

    private void FallModifier(){
        if (_rb.velocity.y < 0){
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
        }
        else if (_rb.velocity.y > 0  && !_isJumping ){
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public bool IsGrounded(){
        float extraHeightText = .01f;
        RaycastHit2D raycastHit = Physics2D.Raycast(ground.bounds.center, Vector2.down, ground.bounds.extents.y, _groundLayerMask);
        raycastHit = Physics2D.OverlapBox()
        if(raycastHit.collider != null){
            rayColor = Color.green;
        } else {
            rayColor = Color.red;
        }
        Debug.DrawRay(ground.bounds.center, Vector2.down * (ground.bounds.extents.y + extraHeightText), rayColor);
        Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;
    }


}
