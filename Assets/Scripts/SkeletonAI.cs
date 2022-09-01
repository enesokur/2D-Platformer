using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : MonoBehaviour{
    [SerializeField]
    private float _patrolEdgeLimit;
    private float _rightPatrolLimit;
    private float _leftPatrolLimit;
    private Animator _anim;
    private Rigidbody2D _rb;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private float _speed;
    private bool _chasing = true;
    private bool _attacking = false;
    private int _health = 70;
    private bool _canGetHurt = true;
    [SerializeField]
    private GameObject _exclamationMark;
    private PlayerMove _playerMove;
    [SerializeField]
    private float _attackCoolDown;

    private void Start(){
        _rightPatrolLimit = this.transform.position.x + _patrolEdgeLimit;
        _leftPatrolLimit = this.transform.position.x - _patrolEdgeLimit;
        _rb = this.GetComponent<Rigidbody2D>();
        _anim = this.GetComponent<Animator>();
        _playerMove = _player.GetComponent<PlayerMove>();
    }
    private void Update(){
        if(Vector3.Distance(_player.transform.position,this.transform.position) > 6f){
            Patrol();
        }
        else if(Vector3.Distance(_player.transform.position,this.transform.position) < 1.5f && _playerMove._isGrounded == true){
            Attack();
        }
        else if(Vector3.Distance(_player.transform.position,this.transform.position) < 6f && _playerMove._isGrounded == true){
            Chase();
        }
        else{
            Debug.Log(".");
            _rb.velocity = Vector3.zero;
            _anim.SetBool("IsIdle",true);
        }
        /*if(Vector3.Distance(_player.transform.position,this.transform.position) < 1.5f){
            Attack();
        }
        else if(Vector3.Distance(_player.transform.position,this.transform.position) < 6f){
            Chase();
        }
        else{
            Patrol();
        }*/ 
    }

    private void Patrol(){
        if(_health > 0){
            _rb.velocity = new Vector3(_speed,0f,0f);
            _anim.SetBool("IsIdle",false);
            _anim.SetFloat("animSpeed",1f);
            if(this.transform.position.x >= _rightPatrolLimit && _speed > 0){
                _speed = -1*_speed;
            }
            else if(this.transform.position.x <= _leftPatrolLimit && _speed < 0){
                _speed = -1*_speed;
            }
            if(_rb.velocity.x > 0){
                this.transform.localScale = new Vector3(2.7f,this.transform.localScale.y,this.transform.localScale.z);
            }
            else{
                this.transform.localScale = new Vector3(-2.7f,this.transform.localScale.y,this.transform.localScale.z);
            }
        }
        
    }

    private void Chase(){
        if(_chasing == true && _health > 0){
            _anim.SetBool("IsIdle",false);
            Vector3 direction = (_player.transform.position - this.transform.position);
            if(direction.x < 0){
                direction.x = -1f;
                this.transform.localScale = new Vector3(-2.7f,this.transform.localScale.y,this.transform.localScale.z);
            }
            else if(direction.x > 0){
                direction.x = 1f;
                this.transform.localScale = new Vector3(2.7f,this.transform.localScale.y,this.transform.localScale.z);
            } 
            _rb.velocity = new Vector3(2.5f*direction.x,0f,0f);
            _anim.SetFloat("animSpeed",2f);
        }
    }

    private void Attack(){
        if(_attacking == false && _health > 0){
            Vector3 direction = (_player.transform.position - this.transform.position);
            if(direction.x < 0){
                direction.x = -1f;
                this.transform.localScale = new Vector3(-2.7f,this.transform.localScale.y,this.transform.localScale.z);
            }
            else if(direction.x > 0){
                direction.x = 1f;
                this.transform.localScale = new Vector3(2.7f,this.transform.localScale.y,this.transform.localScale.z);
            } 
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine(){
            _chasing = false;
            _attacking = true;
            _rb.velocity = Vector3.zero;
            _anim.SetBool("IsIdle",true);
            yield return new WaitForSeconds(_attackCoolDown);
            _canGetHurt = false;
            int randomDecision = Random.Range(1,4);
            if(randomDecision == 1 && _health > 0){
                _exclamationMark.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                _exclamationMark.SetActive(false);
                _anim.SetTrigger("Attack1");
                yield return new WaitForSeconds(0.1f);
                _player.GetComponent<PlayerTakeDamage>().TakeDamage(1);
            }
            else if(randomDecision == 2 && _health > 0){
                _exclamationMark.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                _exclamationMark.SetActive(false);
                _anim.SetTrigger("Attack2");
                yield return new WaitForSeconds(0.1f);
                if(Vector3.Distance(this.transform.position,_player.transform.position) < 1.5f){
                    _player.GetComponent<PlayerTakeDamage>().TakeDamage(1);
                }
            }
            else{
                _anim.SetTrigger("Shield");
                yield return new WaitForSeconds(1f);
            }
            _canGetHurt = true;
            _chasing = true;
            _attacking = false;
    }

    public void TakeDamage(int damage){
        if(_canGetHurt == true){
            if(_health > 0){
                _health -= damage;
                _anim.SetTrigger("Hurt");
            }
            if(_health <= 0){
                _anim.SetBool("IsDead",true);
            }
        }
    }
}
