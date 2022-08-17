using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour{
    private float _checkRadius = 0.10f;
    [SerializeField]
    private GameObject wallChecker_1;
    [SerializeField]
    private GameObject wallChecker_2;
    [SerializeField]
    private LayerMask _wallLayer;
    [SerializeField]
    private float _slideSpeed;
    private Rigidbody2D _rb;
    private Vector3 _moveDirection;
    public Collider2D wall_1;
    public Collider2D wall_2;
    private PlayerMove _playerMove;
    [SerializeField]
    private float _wallJumpForce;
    [SerializeField]
    private float _wallHorizontalForce;
    private Animator _anim;
    private int _countJump = 2;

    private void Start(){
        _rb = this.GetComponent<Rigidbody2D>();
        _playerMove = this.GetComponent<PlayerMove>();
        _anim = this.GetComponent<Animator>();
    }


    private void Update(){
        wall_1 = Physics2D.OverlapCircle(wallChecker_1.transform.position,_checkRadius,_wallLayer);
        wall_2 = Physics2D.OverlapCircle(wallChecker_2.transform.position,_checkRadius,_wallLayer);// zıplarken yönümüzü değiştirdiğimizde arkamızdaki wallı algılıyor

        PlayerWallJump();

    }

    private void PlayerWallJump(){
         if(wall_1 != null || wall_2 != null){
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
                if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && _countJump > 0){
                    _moveDirection = _rb.velocity;
                    _moveDirection = new Vector3(_wallHorizontalForce,_wallJumpForce,0f);
                    _rb.velocity = _moveDirection;
                    _countJump--;
                }
            }
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
                if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
                    _moveDirection = _rb.velocity;
                    _moveDirection = new Vector3(_wallHorizontalForce,_wallJumpForce,0f);
                    _rb.velocity = _moveDirection;
                }
            }
            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
                if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && _countJump > 0){
                    _moveDirection = _rb.velocity;
                    _moveDirection = new Vector3(-_wallHorizontalForce,_wallJumpForce,0f);
                    _rb.velocity = _moveDirection;
                    _countJump--;
                }
            }
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
                if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
                    _moveDirection = _rb.velocity;
                    _moveDirection = new Vector3(-_wallHorizontalForce,_wallJumpForce,0f);
                    _rb.velocity = _moveDirection;
                }
            }
            else if(_rb.velocity.y < 0 && !Input.GetButton("Horizontal")){
                _moveDirection = _rb.velocity;
                _moveDirection.y = _slideSpeed;
                _rb.velocity = _moveDirection;
            }
            if(wall_2 != null){
                _countJump = 1;
                _playerMove._jumpCount = 1;
            }
        }
    }
}
