using UnityEngine;
using System.Collections;

public class StarController : CollectableController {
    public float speed;             //vitesse de déplacement de l'objet
    public int score;               //incrementation du score

    /////////////////////////////////////////////////// INIT ///////////////////////////////////////////////////////
    // Use this for initialization
    internal override void Start() {
        Debug.Log("star init");

        base.Start();
    }

    /////////////////////////////////////////////////// EVENTS /////////////////////////////////////////////////////
    internal override void FixedUpdate() {
        if(collected && _player != null) {
            transform.position = Vector3.Lerp(transform.position, _player.transform.position, speed);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(.25f, .25f, 1), speed);
        }
    }


    /////////////////////////////////////////////////// HELPERS ////////////////////////////////////////////////////
    public override void CollectItem() {
        if(collected) {
            _pc.score+= score;
            Destroy(gameObject);
        } else {
            collected = true;
            CircleCollider2D coll = GetComponent<CircleCollider2D>();
            coll.radius *= .25f;
        }

       
    }
}
