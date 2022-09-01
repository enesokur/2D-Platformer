using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackground : MonoBehaviour{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject[] backGrounds;
    private float distance;
    void Start(){
        distance = backGrounds[1].transform.position.x - backGrounds[0].transform.position.x;
    }

    void Update(){
        
        if(_player.transform.position.x > backGrounds[1].transform.position.x){
            backGrounds[0].transform.position += new Vector3(2*distance,0f,0f);
            GameObject temp = backGrounds[1];
            backGrounds[1] = backGrounds[0];
            backGrounds[0] = temp; 
        }
        else if(_player.transform.position.x < backGrounds[0].transform.position.x){
            backGrounds[1].transform.position -= new Vector3(2*distance,0f,0f);
            GameObject temp = backGrounds[0];
            backGrounds[0] = backGrounds[1];
            backGrounds[1] = temp;
        }
    }
}
