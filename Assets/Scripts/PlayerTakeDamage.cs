using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour{
    private int _health = 4;
    private Animator _anim;
    [SerializeField]
    private GameObject _bloodPrefab;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _goblin;
    [SerializeField]
    private GameObject _skeleton;
    private PlayerAttack _playerTakeDamage;
    void Start(){
        _anim = this.GetComponent<Animator>();
        _playerTakeDamage = this.GetComponent<PlayerAttack>();
    }
    public void TakeDamage(int damageAmount){
        if(_health > 0 && _playerTakeDamage.isDefensing == false ){
            _health--;
            Instantiate(_bloodPrefab,this.transform.position + new Vector3(0f,0.5f,0f),Quaternion.identity);
        }
        if(_health <= 0){
            _anim.SetTrigger("Die");
            _player.GetComponent<PlayerMove>().enabled = false;
            _player.GetComponent<Dash>().enabled = false;
            _player.GetComponent<WallJump>().enabled = false;
            _player.GetComponent<PlayerAttack>().enabled = false;
            _goblin.GetComponent<GoblinAI>().enabled = false;
            _skeleton.GetComponent<SkeletonAI>().enabled = false;
        }
    }
}
