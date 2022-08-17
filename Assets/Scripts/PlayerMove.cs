using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour{
    private Rigidbody2D _rb;
    public float _horizontalspeed;
    [SerializeField]
    private float _verticalSpeed;
    Vector3 moveDirection = new Vector3(0f,0f,0f);
    public int _jumpCount = 2;
    [SerializeField]
    private LayerMask _groundLayer;
    private float _checkRadius = 0.04f;
    public bool _isGrounded = true;
    [SerializeField]
    private GameObject _groundChecker;
    private bool _isJumping = false;
    private bool _isFalling = false;
    private Animator _anim;
    private WallJump _wallJump;
    private PlayerAttack _playerAttack;

    private void Start(){
        _rb = this.GetComponent<Rigidbody2D>();
        _anim = this.GetComponent<Animator>();
        _wallJump = this.GetComponent<WallJump>();
        _playerAttack = this.GetComponent<PlayerAttack>();
    }

    private void Update(){
        GroundCheck();
        if(_playerAttack._attacking == false){
            Move();
            Jump();
        }
        JumpingOrFalling();
    }

    public void Jump(){
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
            if(_isGrounded){
                _jumpCount = 2;
            }
            if(_jumpCount > 0){
                if(_jumpCount == 2){
                    _anim.SetTrigger("Jump");
                }
                else if(_jumpCount ==1){
                    _anim.SetBool("JumpToFall",false);
                    _anim.SetBool("Jump2",true);
                }
                _jumpCount--;
                moveDirection = _rb.velocity;
                moveDirection.y = _verticalSpeed;
                _rb.velocity = moveDirection;
            }
        }
    }

    private void Move(){
        float horizontalInput = Input.GetAxis("Horizontal");
        if(Input.GetButton("Horizontal") == false && horizontalInput == 0f && _isGrounded == true){
            _anim.SetBool("Run",false);
            _anim.SetBool("FallToIdle",true);
            _anim.SetBool("JumpToFall",false);
            _anim.SetBool("Jump2",false);
            _anim.SetBool("AttackToIdle",true);
            _anim.SetBool("Attack2",false);
            _anim.SetBool("Attack3",false);
        }
        if(Input.GetButton("Horizontal")){
            moveDirection = _rb.velocity;
            moveDirection.x = horizontalInput*_horizontalspeed;
            this.transform.localScale = new Vector3(Mathf.Sign(horizontalInput),this.transform.localScale.y,this.transform.localScale.z);
            if(_isGrounded == true){
                _anim.SetBool("Run",true);
                _anim.SetBool("FallToRun",true);
                _anim.SetBool("Jump2",false);
                _anim.SetBool("AttackToRun",true);
                _anim.SetBool("Attack2",false);
                _anim.SetBool("Attack3",false);
            }
            _rb.velocity = moveDirection;
        }
        else if(_rb.velocity.x > 0.5 || _rb.velocity.x < -0.5){
            moveDirection = _rb.velocity;
            if(_rb.velocity.x > 0){
                moveDirection.x -= 5f*Time.deltaTime;
            }
            else if(_rb.velocity.x < 0){
                moveDirection.x += 5f*Time.deltaTime;
            }
            else{
                moveDirection.x = 0f;
            }
            _rb.velocity = moveDirection;
        }
    }

    private void GroundCheck(){
            var ground =  Physics2D.OverlapCircle(_groundChecker.transform.position,_checkRadius,_groundLayer);
            if(ground != null){
                _isGrounded = true;
            }
            else{
                _isGrounded = false;
            }
    }

    private void JumpingOrFalling(){
        if(_isGrounded == false){
            if(_rb.velocity.y > 0f && _wallJump.wall_1 == null && _wallJump.wall_2 == null){
                _isJumping = true;
                _anim.SetBool("Run",false);
                _anim.SetBool("FallToIdle",false);
                _anim.SetBool("FallToRun",false);
            }
            else if(_rb.velocity.y < 0f && _wallJump.wall_1 == null && _wallJump.wall_2 == null){
                _anim.SetBool("Jump2",false);
                _anim.SetBool("JumpToFall",true);
            }
        }
        if(_isGrounded == true){
            _isFalling = false;
            _isJumping = false;
        }
    }
}
