using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour{
    private State state;
    private Vector3 _hitPosition;
    [SerializeField]
    private GameObject _player;
    private Rigidbody2D _rb;
    float hookSize;
    private float timeCounter;
    float angle;

    private enum State{
        Normal,
        HookThrown,
        HookShotFlyingPlayer,
    }

    void Start(){
        state = State.Normal;
        _rb = _player.GetComponent<Rigidbody2D>();
    }

    void Update(){
        if(state == State.Normal){
            HookNormalState();
        }
        else if(state == State.HookThrown){
            HookThrownState();
        }
        else if(state == State.HookShotFlyingPlayer){
            HookPlayerFlyingState();
        }
    }

    private void HookNormalState(){
        if(Input.GetKeyDown(KeyCode.Space)){
            Vector3 rayCastDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position,rayCastDirection.normalized);
            if(hit){
                Debug.Log(hit.transform.name);
                _hitPosition = hit.point;
                if(_player.transform.localScale.x == 1 && Camera.main.ScreenToWorldPoint(Input.mousePosition).x - _player.transform.position.x > 1f ||
                    _player.transform.localScale.x == -1 && Camera.main.ScreenToWorldPoint(Input.mousePosition).x - _player.transform.position.x < -1f)
                {
                    if(Vector3.Distance(_hitPosition,this.transform.position) > 1f && hit.transform.tag == "Grapple"){
                        state = State.HookThrown;
                    }
                }
            }
        }
    }

    private void HookThrownState(){
        float throwSpeed = 100f;
        Vector3 lookDir = _hitPosition - _player.transform.position;
        float rotateAngle = Mathf.Atan2(lookDir.y,lookDir.x)*Mathf.Rad2Deg;
        if(_player.transform.localScale.x == -1){
            rotateAngle += 180f;
        }
        Quaternion rotationToTarget = Quaternion.AngleAxis(rotateAngle,Vector3.forward);
        this.transform.rotation = rotationToTarget; 
        hookSize += throwSpeed*Time.deltaTime;
        this.transform.localScale = new Vector3(hookSize,0.2f,0.2f);
    }

    private void HookPlayerFlyingState(){
         CircularMovement();
        if(Vector3.Distance(this.transform.position,_hitPosition) < 1f){
            _player.GetComponent<PlayerMove>().enabled = true;
            _player.GetComponent<PlayerMove>()._jumpCount = 2;
            _player.GetComponent<Dash>().enabled = true;
            state = State.Normal;
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            _player.GetComponent<PlayerMove>().enabled = true;
            _player.GetComponent<PlayerMove>()._jumpCount = 2;
            _player.GetComponent<Dash>().enabled = true;
            _rb.gravityScale =1.5f;
            this.transform.localScale = new Vector3(0,0.2f,0.2f);
            hookSize = 0f;
            state = State.Normal;
        }
    }

    private void CircularMovement(){
        //timeCounter += Time.deltaTime;
        //float x = Mathf.Cos(timeCounter)*Vector3.Distance(_hitPosition,_player.transform.position) + _hitPosition.x;
        //float y = Mathf.Sin(timeCounter)*Vector3.Distance(_hitPosition,_player.transform.position) + _hitPosition.y;
        float slope = (_hitPosition -_player.transform.position).y / (_hitPosition -_player.transform.position).x;
        float slopeofTangent = -1 / slope;
        Vector3 slopeDir = new Vector3(1,slopeofTangent,0);
        if(_player.transform.localScale.x == -1){
            slopeDir = -1*slopeDir;
        }
        //_rb.position = new Vector3(x,y,0);
        _rb.velocity = slopeDir.normalized*7f;
        Vector3 lookDir = _hitPosition - _player.transform.position;
        float rotateAngle = Mathf.Atan2(lookDir.y,lookDir.x)*Mathf.Rad2Deg;
        if(_player.transform.localScale.x == -1){
            rotateAngle += 180f;
        }
        Quaternion rotationToTarget = Quaternion.AngleAxis(rotateAngle,Vector3.forward);
        this.transform.rotation = rotationToTarget;
        if(Mathf.Abs(Vector3.Angle(Vector3.right,_player.transform.position -_hitPosition)) < 5f && _player.transform.localScale.x == 1){
            _player.GetComponent<PlayerMove>().enabled = true;
            _player.GetComponent<PlayerMove>()._jumpCount = 2;
            _player.GetComponent<Dash>().enabled = true;
            _rb.gravityScale =1.5f;
            this.transform.localScale = new Vector3(0,0.2f,0.2f);
            hookSize = 0f;
            state = State.Normal;
        }
        else if(Mathf.Abs(Vector3.Angle(Vector3.left,_player.transform.position -_hitPosition)) < 5f && _player.transform.localScale.x == -1){
            _player.GetComponent<PlayerMove>().enabled = true;
            _player.GetComponent<PlayerMove>()._jumpCount = 2;
            _player.GetComponent<Dash>().enabled = true;
            _rb.gravityScale =1.5f;
            this.transform.localScale = new Vector3(0,0.2f,0.2f);
            hookSize = 0f;
            state = State.Normal;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        _rb.velocity = Vector3.zero;
        _rb.gravityScale = 0f;
        _player.GetComponent<PlayerMove>().enabled = false;
        _player.GetComponent<Dash>().enabled = false;
        state = State.HookShotFlyingPlayer;
    }
}
