using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour{
    public bool _attacking;
    private Animator _anim;
    private PlayerMove _playerMove;
    private int _attackCount = 0;
    private bool _attack2 = false;
    private bool _attack3 = false;
    private float _lastClickedTime;
    private bool waitForAnotherIf = true;
    private float attackRate = 0.2f;
    private float nextAttackTime;

    [SerializeField]
    private GameObject _attackPoint;
    [SerializeField]
    private float _attackRadius;
    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private int _damageAmount;
    private bool _canAttack = true;
    public bool _defensing = false;
    private Rigidbody2D _rb;
    public bool isDefensing = false;
    private void Start(){
        _anim = this.GetComponent<Animator>();
        _playerMove = this.GetComponent<PlayerMove>();
        _rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update(){
        Defense();
        Attack();
        if(_attacking == true){
            Vector3 moveDirection = _rb.velocity;
            if(_rb.velocity.x > 0.1){
                moveDirection.x -= 15f*Time.deltaTime;
            }
            else if(_rb.velocity.x < -0.1){
                moveDirection.x += 15f*Time.deltaTime;
            }
            else{
                moveDirection.x = 0f;
            }
            _rb.velocity = moveDirection;
        }
    }
    void Attack(){
        if(_playerMove._isGrounded == true && _canAttack == true){
            if(Input.GetKeyDown(KeyCode.Mouse0) && Time.time > nextAttackTime){
                Hit();
                nextAttackTime = Time.time + attackRate;
                _attacking = true;
                _anim.SetBool("AttackToRun",false);
                _anim.SetBool("AttackToIdle",false);
                _anim.SetBool("JumpToFall",false);
                if(_attackCount == 0){
                    waitForAnotherIf = true;
                }
                _attackCount++;
                _lastClickedTime = Time.time;
                if(_attackCount == 1){
                    _anim.SetTrigger("Attack1");
                    _anim.SetBool("Attack2",false);
                    _anim.SetBool("Attack3",false);
                }
                else if(_attackCount == 2 && _attack2 == true){
                    _anim.SetBool("Attack2",true);
                    _attack2 = false;
                }
                else if(_attackCount == 3 && _attack3 == true){
                    _anim.SetBool("Attack3",true);
                    _attack3 = false;
                    waitForAnotherIf = false;
                }
            }
            if(_attackCount == 1 && Time.time - _lastClickedTime <= 0.3f){
                if(Input.GetKey(KeyCode.Mouse0)){
                    _attack2 = true;
                    _lastClickedTime = Time.time;
                }
            }
            else if(_attackCount == 1 && Time.time - _lastClickedTime > 0.3f){
                _attackCount = 0;
                _attacking = false;
                if(!Input.GetKey(KeyCode.Mouse1)){
                    Input.ResetInputAxes();
                }
            }
            if(_attackCount == 2 && Time.time - _lastClickedTime <= 0.3f){
                if(Input.GetKey(KeyCode.Mouse0)){
                    _attack3 = true;
                    _lastClickedTime = Time.time;
                }
            }
            else if(_attackCount == 2 && Time.time - _lastClickedTime > 0.3f){
                _attackCount = 0;
                _attacking = false;
                if(!Input.GetKey(KeyCode.Mouse1)){
                    Input.ResetInputAxes();
                }
            }
            else if(_attackCount == 3 && Time.time - _lastClickedTime > 0.3f){
                _attackCount = 0;
                _attacking = false;
                if(!Input.GetKey(KeyCode.Mouse1)){
                    Input.ResetInputAxes();
                }
            }
            else if(_attackCount > 3 && waitForAnotherIf == false){
                _attackCount = 0;
                Invoke("setAttackingToFalse",0.2f);
                if(!Input.GetKey(KeyCode.Mouse1)){
                    Input.ResetInputAxes();
                }
            }
        }
    }

    private void Hit(){
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(_attackPoint.transform.position,_attackRadius,_enemyLayer);
        if(enemyColliders != null){
            foreach(Collider2D enemy in enemyColliders){
                if(enemy.tag == "Goblin"){
                    enemy.GetComponent<GoblinAI>().TakeDamage(_damageAmount);
                }
                else if(enemy.tag == "Skeleton"){
                    enemy.GetComponent<SkeletonAI>().TakeDamage(_damageAmount);
                }
            }
        }
    }

    private void Defense(){
        if(_playerMove._isGrounded == true && _attacking == false){
            if(Input.GetKey(KeyCode.Mouse1)){
                isDefensing = true;
                if(_rb.velocity == Vector2.zero){
                    _canAttack = false;
                    _defensing = true;
                    _anim.SetBool("IdleBlock",true);
                }
                else{
                    _rb.velocity = Vector3.zero;
                    _canAttack = false;
                    _defensing = true;
                    _anim.SetBool("RunBlock",true);
                    _anim.SetBool("IdleBlock",true);
                }
            }
            else if(Input.GetKeyUp(KeyCode.Mouse1)){
                isDefensing = false;
                _canAttack = true;
                _defensing = false;
                _anim.SetBool("IdleBlock",false);
                _anim.SetBool("RunBlock",false);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(_attackPoint.transform.position,_attackRadius);
    }

    private void setAttackingToFalse(){
        _attacking = false;
    }
}
