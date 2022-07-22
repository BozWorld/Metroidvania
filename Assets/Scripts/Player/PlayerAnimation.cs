using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private CharacterMovement _player;
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteRenderer _renderer;

    private float _lockedTill;
    private bool _isJumping;
    private bool _isFalling;
    private bool _isMoving;
    private bool _isGrounded;

    private void Start()
    {
        _player.Input.Player.Move.started += ctx => _isMoving = true;
        _player.Input.Player.Move.started += ctx => _isJumping = true;
    }

    private void Update()
    {
        _isGrounded = _player.IsGrounded();
        var state = GetState();

        _isJumping = false;
        _isFalling = false;

        if (state == _currentState) return;
        _anim.CrossFade(state, 0, 0);
        _currentState = state;

    }
    private int GetState() 
    {
        if (Time.time < _lockedTill) return _currentState;
        // Priorities
        if (_isJumping) return Jump;
        if (_isGrounded) return _player.RawMovement.x == 0 ? Idle : Walk;
        return _player.Rb.velocity.y > 0 ? Jump : Fall;

        int LockState(int s, float t) {
            _lockedTill = Time.time + t;
            return s;
        }
    }

    #region hashedProprieties

    private int _currentState;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Fall = Animator.StringToHash("Fall");

    #endregion
}
