using UnityEngine;
using System.Collections;

public class CollectableController : MonoBehaviour {
    [HideInInspector] public GameObject           _player;            //le player
    [HideInInspector] public PlayerController     _pc;                //player controller
    private GameObject          go;                 //this gameobject
    public bool                 collected;          //si l'objet est capturé





    /////////////////////////////////////////////////// INIT ///////////////////////////////////////////////////////
    // Use this for initialization
    internal virtual void Start() {
        Debug.Log("collectable init");

        //recuperer le player et son controller
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null) {
            _pc = _player.GetComponent<PlayerController>();
        }
        collected = false;

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
                    CollectItem();
                    break;

                default:
                    break;
            }
        }
    }

    //entrée de trigger
    internal virtual void OnTriggerStay2D(Collider2D other) {
    }

    //sortie de trigger
    internal virtual void OnTriggerExit2D(Collider2D other) {
    }


    /////////////////////////////////////////////////// HELPERS ////////////////////////////////////////////////////
    //collecter l'item en collision
    public virtual void CollectItem() {
    }
}
