using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterMovement : MonoBehaviour, IStartCooldown<float>, IUpdateCooldown<float>
{
    public Vector2 bottomOffset;
    public float RadiusCollider;
    public bool test;
    public bool collide;
    public bool CanMove = true;
    public bool ModePlayer;
    [SerializeField] private LayerMask _groundLayerMask;
    public float Speed;
    public PlayerInput Input;
    public Vector2 RawMovement;
    private float _checkDirection;
    public SpriteRenderer PlayerSprite;

    [Range(1, 10)]
    public float JumpVelocity;
    private bool _isJumping;
    public float FallMultiplier = 2.5f;
    public float LowJumpMultiplier = .2f;
    [HideInInspector] public Rigidbody2D Rb;
    private bool _canJump;
    private BoxCollider2D _PlayerCollider;
    private float _yVelocity;
    public Animator PlayerAnimator;

    public float DashDistance;
    public float DashSpeed;
    public float DashinCoolDown;
    public bool _canDash = true;
    public bool IsDashing;
    public TrailRenderer DashTrail;


    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Input = new PlayerInput();
        Input.Player.Enable();
        Input.Player.Move.started += ctx => RawMovement = ctx.ReadValue<Vector2>();
        Input.Player.Move.canceled += _ => RawMovement = Vector2.zero;
        Input.Player.Jump.started += ctx => Jump();
        Input.Player.Jump.canceled += ctx => _isJumping = false;
        Input.Player.Dash.started += ctx => StartCoroutine(Dash());
        Input.Player.Switch.started += ctx => SwitchMode();
        _PlayerCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        Move();
        FallModifier();
        IsGrounded();
        flip();
        CheckColl();
    }
    private void CheckColl()
    {
        
    }
    private void Move()
    {
        if (RawMovement != Vector2.zero && CanMove)
        {
            PlayerSprite.flipX = RawMovement.x < 0;
            transform.position += (Vector3)RawMovement * Speed * Time.deltaTime;
        }
    }

    public void Jump()
    {
        if (IsGrounded() && !_isJumping )
        {
            _isJumping = true;
            Rb.velocity = Vector2.up * JumpVelocity;
        }
    }

    private void FallModifier()
    {
        if (Rb.velocity.y < 0)
        {
            Rb.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
        }
        else if (Rb.velocity.y > 0 && !_isJumping)
        {
            Rb.velocity += Vector2.up * Physics2D.gravity.y * (LowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public bool IsGrounded()
    {
        test = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, RadiusCollider, _groundLayerMask);
        _yVelocity = Mathf.Clamp(Rb.velocity.y, -1, 1);
        PlayerAnimator.SetFloat("yVelocity", _yVelocity);
        if (test)
        {
            return true;
        }
        else return false;
    }

    private void SwitchMode()
    {
        ModePlayer = !ModePlayer;
    }
#region spell

    private IEnumerator Dash()
    {
        if( _canDash)
        {
            _canDash = false;
            IsDashing = true;
            CanMove = false;
            float originaGravity = Rb.gravityScale;
            Rb.gravityScale = 0;
            Rb.velocity = new Vector2((transform.localScale.x * DashDistance) * _checkDirection , 0f);
            DashTrail.emitting = true;
            _canJump = false;
            yield return new WaitForSeconds(DashSpeed);
            DashTrail.emitting = false;
            Rb.gravityScale = originaGravity;
            IsDashing = false;
            Rb.velocity = new Vector2(0, 0);
            yield return new WaitForSeconds(DashinCoolDown);
            _canJump = true;
            CanMove = true;
            _canDash = true;
        }
    }

    private void flip()
    {
        if(RawMovement.x < 0)
        {
            _checkDirection = -1;
        }
        if(RawMovement.x > 0)
        {
            _checkDirection = 1;
        }
    }

    #endregion
    #region coolDown

    public void StartCooldown(float cooldown, float cooldownT)
    {
        cooldownT = cooldown;
        //valeur to set false;
    }

    public void UpdateCooldown(float cooldownT)
    {
        cooldownT -= Time.deltaTime;
        //if (cooldownT) <= 0) valeur to set to true;
    }
    #endregion

    private void OnDrawGizmos() 
    {
        if(test)
        {
            Gizmos.color = Color.green;
        }
        if (!test)
        {
            Gizmos.color = Color.red;
        } 
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, RadiusCollider);
        Gizmos.DrawRay(transform.position, Vector3.right *_checkDirection);

    }
}