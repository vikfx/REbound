using UnityEngine;
using System.Collections;

public class PowerController : MonoBehaviour {
    public GameObject           _player;        //le player
    public PlayerController    _pc;
    //public string               action;         //nom de l'action
    //private GameObject          go;             //this gameobject
    
    
   
    

    /////////////////////////////////////////////////// INIT ///////////////////////////////////////////////////////
    // Use this for initialization
    internal virtual void Start () {
        //Debug.Log("power init : " + action);

        //recuperer le player et son controller
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null) {
            _pc = _player.GetComponent<PlayerController>();
        }

    }


    /////////////////////////////////////////////////// EVENTS /////////////////////////////////////////////////////
    // Update is called once per frame
    void Update () {
	
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
                    AddPower();
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
                    StayPower();
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
                    Debug.Log("killPower");
                    KillPower();
                    break;

                default:
                    break;
            }
        }
    }


    /////////////////////////////////////////////////// HELPERS ////////////////////////////////////////////////////
    //lancer une action en collsion
    public virtual void AddPower() {
        _pc.powerController = this;
    }

    //update une action en collsion
    public virtual void StayPower() {

    }

    //kill d'une action en collsion
    public virtual void KillPower() {
        _pc.powerController = null;
    }

    //se déplacer
    public virtual void moveH() {
    }
}
