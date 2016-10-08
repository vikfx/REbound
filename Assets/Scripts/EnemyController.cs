using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
    [HideInInspector] public GameObject           _player;        //le player
    [HideInInspector] public PlayerController     _pc;
    public bool                 onAttack;       //si l'ennemi attaque
    //public string               action;         //nom de l'action
    //private GameObject          go;             //this gameobject





    /////////////////////////////////////////////////// INIT ///////////////////////////////////////////////////////
    // Use this for initialization
    internal virtual void Start() {
        //Debug.Log("power init : " + action);

        //recuperer le player et son controller
        _player = GameObject.FindGameObjectWithTag("Player");
        if(_player != null) {
            _pc = _player.GetComponent<PlayerController>();
        }
    }


    /////////////////////////////////////////////////// EVENTS /////////////////////////////////////////////////////
    // Update is called once per frame
    internal virtual void Update() {

    }

    //update physics
    internal virtual void FixedUpdate() {
    }


    //entrée de trigger
    internal virtual void OnTriggerEnter2D(Collider2D other) {
        if (_player != null) {
            switch (other.tag) {
                //Player
                case "Player":
                    AddAttack();
                    break;

                default:
                    break;
            }
        }
    }

    //entrée de trigger
    internal virtual void OnTriggerStay2D(Collider2D other) {
        if (_player != null) {
            switch (other.tag) {
                //Player
                case "Player":
                    StayAttack();
                    break;

                default:
                    break;
            }
        }
    }

    //sortie de trigger
    internal virtual void OnTriggerExit2D(Collider2D other) {
        if (_player != null) {
            switch (other.tag) {
                //Player
                case "Player":
                    KillAttack();
                    break;

                default:
                    break;
            }
        }
    }


    /////////////////////////////////////////////////// HELPERS ////////////////////////////////////////////////////
    //lancer une action en collsion
    public virtual void AddAttack() {
        onAttack = true;
        _pc.looseGame();
    }

    //update une action en collsion
    public virtual void StayAttack() {
    }

    //kill d'une action en collsion
    public virtual void KillAttack() {
        onAttack = false;
    }
}
