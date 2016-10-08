using UnityEngine;
using System.Collections;

public class DeathStarController : EnemyController {
    public Vector2          angleLimits;            //limites d'angle pour le rayon
    public float            speed;                  //vitesse de deplacement du rayon
    public float            _angle;                 //angle en degrés en cours pour le rayon
    private float           _sens;                  //sens de rotation du rayon
    public Vector3          BeamOffset;             //decallage de la source du rayon
    private LayerMask       _mask;                  //layers à activer
    private LineRenderer    _line;                  //rendu de la ligne
    public GameObject       emitter;                //position de l'emetteur de particule

    /////////////////////////////////////////////////// INIT ///////////////////////////////////////////////////////
    // Use this for initialization
    internal override void Start() {
        Debug.Log("death star init");

        base.Start();
        _angle = angleLimits.x;
        _sens = 1;
        _line = GetComponent<LineRenderer>();
        _mask = LayerMask.GetMask("Environment", "Player");
    }

    /////////////////////////////////////////////////// EVENTS /////////////////////////////////////////////////////
    //update physics
    internal override void FixedUpdate() {
        //launchBeam();
    }


    /////////////////////////////////////////////////// HELPERS ////////////////////////////////////////////////////
    //lancer une action en collsion
    public override void AddAttack() {
        ParticleSystem  ps;                 //systeme de particule

        //en attaque
        onAttack = true;

        //actionner l'emetteur
        ps = emitter.GetComponent<ParticleSystem>();
        if (!ps.isPlaying) ps.Play();
    }

    //update une action en collsion
    public override void StayAttack() {
        launchBeam();
    }

    //kill d'une action en collsion
    public override void KillAttack() {
        ParticleSystem  ps;                 //systeme de particule

        //stopper l'attaque
        onAttack = false;

        //stopper l'emetteur
        ps = emitter.GetComponent<ParticleSystem>();
        if (ps.isPlaying) {
            ps.Stop();
            ps.Clear();
        }

        //rendre le rayon
        renderLine(transform.position + BeamOffset, transform.position + BeamOffset);
    }

    //lancer de rayon
    private void launchBeam() {
        Vector3         direction;          //direction du rayon
        Vector3         sp;                 //origine du rayon
        RaycastHit2D    hit;                //hitpoint du rayon
        Vector3         hp;                 //point de contact
        Collider2D      coll;               //collider de contact
        
        if (onAttack) {
            //incrementation de l'angle du rayon
            _angle += speed * _sens;
            if (_angle <= angleLimits.x || _angle >= angleLimits.y) {
                _sens *= -1;
            }

            //origine du rayon
            sp = transform.position + BeamOffset;

            //definir le vecteur direction en fonction de l'angle
            direction = new Vector3(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad), 0);

            //lancé du rayon
            hit = Physics2D.Raycast(sp, direction, Mathf.Infinity, _mask.value);

            //definir le contact
            hp = hit.point;
            coll = hit.collider;

            if(coll.tag == "Player") {
                _pc.looseGame();
            }

            //placer l'emetteur de particules
            emitter.transform.position = hp;
            
            //rendu de la ligne
            renderLine(sp, hp);
        }
    }

    

    //rendu de la ligne
    private void renderLine(Vector3 from, Vector3 to) {
        Vector3[] positions;

        positions = new Vector3[2];
        positions[0] = from;
        positions[1] = to;
        _line.SetPositions(positions);
    }
}