using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour{
    private Rigidbody2D _rb;
    [SerializeField]
    private float _dashForce;
    private bool _dashing;
    private float _dashStartTime;
    private float _timePassed;
    private PlayerMove _playerMove;
    private float _nextDashTime;
    private void Start(){
        _rb = this.GetComponent<Rigidbody2D>();
        _playerMove = this.GetComponent<PlayerMove>();
    }
    private void Update(){
        if(Input.GetKeyDown(KeyCode.LeftShift) && Time.time > _nextDashTime){
            _dashing = true;
            _dashStartTime = Time.time;
            _nextDashTime = Time.time +1.0f;
            _playerMove._horizontalspeed = 20f;
        }
        if(_dashing){
            _timePassed += Time.deltaTime;
            if(Time.time - _dashStartTime > 0.13f){
                _playerMove._horizontalspeed = 4f;
                _dashing = false;
            }
        }
    }
}
