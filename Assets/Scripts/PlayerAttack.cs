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
    private void Start(){
        _anim = this.GetComponent<Animator>();
        _playerMove = this.GetComponent<PlayerMove>();
    }

    private void Update(){
        Attack();
    }
    void Attack(){
        if(_playerMove._isGrounded == true){
            if(Input.GetKeyDown(KeyCode.Mouse0) && Time.time > nextAttackTime){
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
            if(_attackCount == 1 && Time.time - _lastClickedTime <= 0.5f){
                if(Input.GetKey(KeyCode.Mouse0)){
                    _attack2 = true;
                    _lastClickedTime = Time.time;
                }
            }
            else if(_attackCount == 1 && Time.time - _lastClickedTime > 0.5f){
                _attackCount = 0;
                _attacking = false;
            }
            if(_attackCount == 2 && Time.time - _lastClickedTime <= 0.5f){
                if(Input.GetKey(KeyCode.Mouse0)){
                    _attack3 = true;
                    _lastClickedTime = Time.time;
                }
            }
            else if(_attackCount == 2 && Time.time - _lastClickedTime > 0.5f){
                _attackCount = 0;
                _attacking = false;
            }
            else if(_attackCount == 3 && Time.time - _lastClickedTime > 0.5f){
                _attackCount = 0;
                _attacking = false;
            }
            else if(_attackCount > 3 && waitForAnotherIf == false){
                _attackCount = 0;
                _attacking = false;
            }
        }
    }
}
