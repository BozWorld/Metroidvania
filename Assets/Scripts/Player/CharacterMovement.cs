using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    public float Speed;
    private PlayerInput _input;
    private Vector2 _rawMovement;
    private SpriteRenderer playerSprite;

    [Range(1,10)]
    public float JumpVelocity;
    private bool _isJumping;
    public float FallMultiplier = 2.5f;
    public float LowJumpMultiplier = .2f;
    private Rigidbody2D _rb;
    public bool CanJump;
    public BoxCollider2D Ground;
    private float _yVelocity;
    public Animator PlayerAnimator;

    public float DashDistance;
    public float DashSpeed;
    public float DashinCoolDown;
    private bool _canDash;
    private bool _isDashing;
    public TrailRenderer DashTrail;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = new PlayerInput();
        _input.Player.Enable();
        _input.Player.Move.started += ctx => _rawMovement = ctx.ReadValue<Vector2> ();
        _input.Player.Move.canceled += _ => _rawMovement = Vector2.zero;
        _input.Player.Jump.started += ctx => Jump();
        _input.Player.Jump.canceled += ctx => _isJumping = false;
        _input.Player.Dash.started += ctx => Dash();
    }

    private void FixedUpdate() 
    {
        Move();
        FallModifier();
        IsGrounded();
    }

    private void Move()
    {
        if(_rawMovement != Vector2.zero){
            PlayerAnimator.SetBool("isMoving", true);
            transform.position += (Vector3)_rawMovement * Speed * Time.deltaTime;
        }
        else
        {
            PlayerAnimator.SetBool("isMoving", false);
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            _isJumping = true;
            _rb.velocity = Vector2.up * JumpVelocity;
            
        }
    }

    private void FallModifier()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
        }
        else if (_rb.velocity.y > 0  && !_isJumping )
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (LowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public bool IsGrounded()
    {
        _yVelocity = Mathf.Clamp(_rb.velocity.y, -1, 1);
        PlayerAnimator.SetFloat("yVelocity", _yVelocity);
        
        float extraHeightText = .01f;
        RaycastHit2D raycastHit = Physics2D.Raycast(Ground.bounds.center, Vector2.down, Ground.bounds.extents.y, _groundLayerMask);
        Color rayColor;
        
        if(raycastHit.collider != null)
        {
            rayColor = Color.green;
            PlayerAnimator.SetBool("isJumping", false);
        } 
        else 
        {
            PlayerAnimator.SetBool("isJumping", true);
            rayColor = Color.red;
        }
        
        Debug.DrawRay(Ground.bounds.center, Vector2.down * (Ground.bounds.extents.y + extraHeightText), rayColor);
        Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;
    }

    #region spell
    
    private IEnumerator Dash()
    {
        Debug.Log("Dashing");
        float originaGravity = _rb.gravityScale;
        _rb.gravityScale = 0;
        _rb.velocity = new Vector2(transform.localScale.x * DashDistance, 0f);
        DashTrail.emitting = true;
        yield return new WaitForSeconds(DashSpeed);
        DashTrail.emitting = false;
        _rb.gravityScale = originaGravity;
        _isDashing = false;
        yield return new WaitForSeconds(DashinCoolDown);
        _canDash = true;
        // Vector2 dashPos = new Vector2(transform.position.x + DashDistance, transform.position.y);
        // transform.position = Vector2.Lerp(transform.position,dashPos, DashSpeed);
        //_rb.velocity = new Vector2(transform.position.x * DashDistance *, transform.position.y);
    }

    #endregion
}
