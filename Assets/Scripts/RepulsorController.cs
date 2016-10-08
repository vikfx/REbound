using UnityEngine;
using System.Collections;

public class RepulsorController : PowerController {

    public float            force;
    public float            maxSpeed;
    public float            gravity;
    private float           _initialGravity;
    private Rigidbody2D     _prb2d;

    /////////////////////////////////////////////////// INIT ///////////////////////////////////////////////////////
    // Use this for initialization
    internal override void Start() {
        Debug.Log("repulsor init");

        base.Start();

        //init le rigidbody et la gravite du player
        if (_player != null) {
            _prb2d = _player.GetComponent<Rigidbody2D>();
            _initialGravity = _prb2d.gravityScale;
        }
    }

    /////////////////////////////////////////////////// EVENTS /////////////////////////////////////////////////////

    /////////////////////////////////////////////////// HELPERS ////////////////////////////////////////////////////
    //ajouter le pouvoir
    public override void AddPower() {
        _prb2d.gravityScale = gravity;
        base.AddPower();
    }



    //update le pouvoir
    public override void StayPower() {
        Vector2 moveForce;
        
        Debug.Log("moveforce");
        moveForce = transform.rotation * Vector2.up * force;

        if (_prb2d.velocity.magnitude < maxSpeed) {
            _prb2d.AddForce(moveForce);
        }
    }

    //kill d'une action
    public override void KillPower() {
        _prb2d.gravityScale = _initialGravity;
        base.KillPower();
    }

}
