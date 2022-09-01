using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAI : MonoBehaviour{
    [SerializeField]
    private float _patrolEdgeLimit;
    private float _rightPatrolLimit;
    private float _leftPatrolLimit;
    [SerializeField]
    private float _speed;
    private Rigidbody2D _rb;
    [SerializeField]
    private GameObject _player;
    private Animator _anim;
    private bool _patrolling = true;
    private bool _attacking = false;
    private int _health = 70;
    private bool _canGetHurt = true;
    [SerializeField]
    private GameObject _exclamationMark;
    private bool _chasing = true;
    private PlayerMove _playerMove;
    [SerializeField]
    private float _attackCoolDown;

    void Start(){
        _rightPatrolLimit = this.transform.position.x + _patrolEdgeLimit;
        _leftPatrolLimit = this.transform.position.x - _patrolEdgeLimit;
        _rb = this.GetComponent<Rigidbody2D>();
        _anim = this.GetComponent<Animator>();
        _playerMove = _player.GetComponent<PlayerMove>();
    }
    void Update(){
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
            _rb.velocity = Vector3.zero;
            _anim.SetBool("Idle",true);
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
        if(_patrolling == true && _health > 0){
            _attacking = false;
            _rb.velocity = new Vector3(_speed,0f,0f);
            if(this.transform.position.x >= _rightPatrolLimit && _speed > 0){
                StartCoroutine(IdleRoutine());
                _speed = -1*_speed;
            }
            else if(this.transform.position.x <= _leftPatrolLimit && _speed < 0){
                StartCoroutine(IdleRoutine());
                _speed = -1*_speed;
            }
            if(_rb.velocity.x > 0){
                this.transform.localScale = new Vector3(3,this.transform.localScale.y,this.transform.localScale.z);
            }
            else{
                this.transform.localScale = new Vector3(-3,this.transform.localScale.y,this.transform.localScale.z);
            }
        }
    }
    IEnumerator IdleRoutine(){
        _patrolling = false;
        _rb.velocity = Vector3.zero;
        _anim.SetBool("Idle",true);
        yield return new WaitForSeconds(3f);
        _patrolling = true;
        _anim.SetBool("Idle",false);
    }

    private void Chase(){
        if(_chasing == true && _health > 0){
            _anim.SetBool("Idle",false);
            Vector3 direction = _player.transform.position - this.transform.position;
            if(direction.x < 0){
                direction.x = -1;
                _rb.velocity = new Vector3(2*Mathf.Abs(_speed)*direction.x,0f,0f);
                this.transform.localScale = new Vector3(-3,this.transform.localScale.y,this.transform.localScale.z);
            }
            else if(direction.x > 0){
                direction.x = 1;
                _rb.velocity = new Vector3(2*Mathf.Abs(_speed)*direction.x,0f,0f);
                this.transform.localScale = new Vector3(3,this.transform.localScale.y,this.transform.localScale.z);
            }
        }
    }

    private void Attack(){
        if(_attacking == false && _health > 0){
            StartCoroutine(AttackRoutine());
        }
    }
    private IEnumerator AttackRoutine(){
        _chasing = false;
        _attacking = true;
        _rb.velocity = Vector3.zero;
        _anim.SetBool("Idle",true);
        yield return new WaitForSeconds(_attackCoolDown);
        Vector3 direction = _player.transform.position - this.transform.position;
        if(direction.x > 0){
            this.transform.localScale = new Vector3(3,this.transform.localScale.y,this.transform.localScale.z);
        }
        else{
            this.transform.localScale = new Vector3(-3,this.transform.localScale.y,this.transform.localScale.z);
        }
        /*_exclamationMark.SetActive(true);
        _canGetHurt = false;
        yield return new WaitForSeconds(0.6f);
        _exclamationMark.SetActive(false);*/
        if(_health > 0){
            _exclamationMark.SetActive(true);
            _canGetHurt = false;
            yield return new WaitForSeconds(0.6f);
            _exclamationMark.SetActive(false);
            _anim.SetBool("Attack",true);
            yield return new WaitForSeconds(0.1f);
            if(Vector3.Distance(this.transform.position,_player.transform.position) < 1.5f){
                _player.GetComponent<PlayerTakeDamage>().TakeDamage(1);
            }
        }
        /*_anim.SetBool("Attack",true);
        yield return new WaitForSeconds(0.1f);
        _player.GetComponent<PlayerTakeDamage>().TakeDamage(1);*/
        _canGetHurt = true;
        _anim.SetBool("Attack",false);
        _attacking = false;
        _chasing = true;
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
