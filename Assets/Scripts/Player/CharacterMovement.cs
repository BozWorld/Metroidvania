using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterMovement : MonoBehaviour, IStartCooldown<float>, IUpdateCooldown<float>
{
    public bool ModePlayer;
    public TMP_Text textMode;
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
    public BoxCollider2D Ground;
    private float _yVelocity;
    public Animator PlayerAnimator;

    public float DashDistance;
    public float DashSpeed;
    public float DashinCoolDown;
    private bool _canDash = true;
    private bool _isDashing;
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
    }

    private void FixedUpdate()
    {
        Move();
        FallModifier();
        IsGrounded();
        flip();
    }

    private void Move()
    {
        if (RawMovement != Vector2.zero)
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
        _yVelocity = Mathf.Clamp(Rb.velocity.y, -1, 1);
        PlayerAnimator.SetFloat("yVelocity", _yVelocity);

        float extraHeightText = 0.5f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(Ground.bounds.center, Ground.bounds.size, 0f, Vector2.down, extraHeightText, _groundLayerMask);
        Color rayColor;

        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            ;
            rayColor = Color.red;
        }

        Debug.DrawRay(Ground.bounds.center + new Vector3(Ground.bounds.extents.x, 0), Vector2.down * (Ground.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(Ground.bounds.center - new Vector3(Ground.bounds.extents.x, 0), Vector2.down * (Ground.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(Ground.bounds.center - new Vector3(Ground.bounds.extents.x, Ground.bounds.extents.y + extraHeightText), Vector2.right * (Ground.bounds.extents.x), rayColor);
        return raycastHit.collider != null;
    }

    private void SwitchMode()
    {
        ModePlayer = !ModePlayer;
        textMode.text = (ModePlayer) ? "Hand to Hand"  : "Distance";
    }
#region spell

    private IEnumerator Dash()
    {
        if( _canDash)
        {
            _canDash = false;
            _isDashing = true;
            float originaGravity = Rb.gravityScale;
            Rb.gravityScale = 0;
            Rb.velocity = new Vector2((transform.localScale.x * DashDistance) * _checkDirection , 0f);
            DashTrail.emitting = true;
            yield return new WaitForSeconds(DashSpeed);
            DashTrail.emitting = false;
            Rb.gravityScale = originaGravity;
            _isDashing = false;
            yield return new WaitForSeconds(DashinCoolDown);
            _canDash = true;
        }
    }

    private void flip()
    {
        if(RawMovement.x < 0)
        {
            _checkDirection = -1;
        }
        else
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
}