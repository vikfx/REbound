using UnityEngine;
using System.Collections;

public class BulletController : EnemyController {
    public float speed;


    /////////////////////////////////////////////////// INIT ///////////////////////////////////////////////////////
    // Use this for initialization
    internal override void Start() {
        Debug.Log("enemy collider init");

        base.Start();
    }

    /////////////////////////////////////////////////// EVENTS /////////////////////////////////////////////////////
    internal override void FixedUpdate() {
        Rigidbody2D rb2d;
        Vector2 lf;

        rb2d = GetComponent<Rigidbody2D>();
        lf = new Vector2(0, speed);

        rb2d.velocity = transform.TransformVector(lf);
    }



    //entrée de trigger
    internal override void OnTriggerEnter2D(Collider2D other) {
        switch (other.tag) {
            //Player
            case "Player":
                AddAttack();
                Destroy(gameObject);
                break;

            //Environnement
            case "Environment":
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
    /////////////////////////////////////////////////// HELPERS ////////////////////////////////////////////////////
}
