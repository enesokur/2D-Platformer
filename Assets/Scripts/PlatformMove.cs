using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour{

    [SerializeField]
    private int _platformType;
    
    private Vector3 upPosition;
    private Vector3 downPosition;
    [SerializeField]
    private int direction;// 0 down 1 up
    [SerializeField]
    private GameObject _player;
    
    void Start(){
        upPosition = new Vector3(this.transform.position.x,this.transform.position.y+2,this.transform.position.z);
        downPosition = new Vector3(this.transform.position.x,this.transform.position.y-6,this.transform.position.z);
    }


    void Update(){
        if(_platformType == 1){
            MoveUpDown();
        }    
    }

    private void MoveUpDown(){
        if(direction == 1){
            this.transform.position = Vector3.MoveTowards(this.transform.position,upPosition,2f*Time.deltaTime);
        }
        else if(direction == 0){
            this.transform.position = Vector3.MoveTowards(this.transform.position,downPosition,2f*Time.deltaTime);
        }

        if(Vector3.Distance(this.transform.position,upPosition) < 0.1f){
            direction = 0;
        }
        else if(Vector3.Distance(this.transform.position,downPosition) < 0.1f){
            direction = 1;
        }
    }

    private IEnumerator PlatformShake(){
        float shakeTime = 0.3f;
        Vector3 originalPos = this.transform.position;
        while(shakeTime > 0){
            shakeTime -= Time.deltaTime;
            this.transform.position += (Vector3)Random.insideUnitCircle*0.1f;
            yield return null;
            this.transform.position = originalPos;
        }
        this.GetComponent<Rigidbody2D>().velocity = Vector3.down*8f;
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(other.transform.CompareTag("Player")){
            _player.transform.SetParent(this.transform);
            _player.transform.localScale = new Vector3(1f,1f,1f);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.transform.CompareTag("Player")){
            _player.transform.SetParent(this.transform);
            _player.transform.localScale = new Vector3(1f,1f,1f);
            if(_platformType == 2){
                StartCoroutine(PlatformShake());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.transform.CompareTag("Player")){
            _player.transform.parent = null;
        }
    }
}
