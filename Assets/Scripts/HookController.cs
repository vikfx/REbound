using UnityEngine;
using System.Collections;

public class HookController : PowerController {
    private Material        _material;
    private int              _clockwise;
    public SpringJoint2D    _ropeJoint;          //joint entre le player et l'objet
    public LineRenderer     _ropeLine;

    public float            damping;
    public float            distanceMin;
    public float            distanceMax;
    public float            frequency;
    public Color            colorStart;
    public Color            colorEnd;
    public float            widthStart;
    public float            widthEnd;
    public float            force;
    public float            maxSpeed;
    public float            repulsion;

    /////////////////////////////////////////////////// INIT ///////////////////////////////////////////////////////
    // Use this for initialization
    internal override void Start() {
        Debug.Log("hook init");

        //material pour le rendu de la ligne
        _material = new Material(Shader.Find("Particles/Additive"));

        //sens de rotation
        _clockwise = 1;

        base.Start();
    }


    /////////////////////////////////////////////////// EVENTS /////////////////////////////////////////////////////
    //update physics
    internal override void FixedUpdate() {
        if (_player != null) {
            if (((_ropeJoint || _pc.onAction) && !Input.GetButton("Fire3"))) {
                destroyHook();
            }
            updateRope();

            //moveHOn();
        }
    }

    //sortie de trigger
    internal override void OnTriggerExit2D(Collider2D other) {
    }


    /////////////////////////////////////////////////// HELPERS ////////////////////////////////////////////////////
    //ajouter le pouvoir
    public override void AddPower() {

    }


    //update le pouvoir
    public override void StayPower() {
        Vector2 rf;             //vecteur angulaire normé 
        float dx;               //difference X entre le hook et le player
        float dy;               //difference Y entre le hook et le player
        float dist;             //distance entre le hook et le player
        Vector2 cv;             //velocite courante du player
        Rigidbody2D rb2d;       //rigidbody du player
        float horizontal;       //axe horizontal du user 

        if (!_pc.onAction && Input.GetButton("Fire3")) {
            //definir le caractere comme en action
            _pc.onAction = true;
            _pc.powerController = this;

            Debug.Log("add hook");

            horizontal = Input.GetAxis("Horizontal");

            rb2d = _player.GetComponent<Rigidbody2D>();

            dx = transform.position.x - _player.transform.position.x;
            dy = transform.position.y - _player.transform.position.y;
            dist = Vector3.Distance(_player.transform.position, transform.position);
            rf = new Vector2(dx / dist, dy / dist);

            //creer le joint
            _ropeJoint = gameObject.AddComponent<SpringJoint2D>();
            _ropeJoint.connectedBody = _player.GetComponent<Rigidbody2D>();
            _ropeJoint.dampingRatio = damping;
            _ropeJoint.autoConfigureDistance = false;
            if (_ropeJoint.distance < distanceMin) {
                _ropeJoint.distance = distanceMin;
            }
            else if (_ropeJoint.distance > distanceMax) {
                _ropeJoint.distance = distanceMax;
            }
            else {
                _ropeJoint.distance = dist;
            }
            _ropeJoint.frequency = frequency;
            cv = rb2d.velocity;

            //definir le sens de rotation
            if(rb2d.velocity.x > 0) {
                _clockwise = (dx < 0 && dy < 0) ? -1 : 1;
            } else {
                _clockwise = (dx > 0 && dy < 0) ? -1 : 1;
            }


            //creer la ligne
            _ropeLine = gameObject.AddComponent<LineRenderer>();
            _ropeLine.SetWidth(widthStart, widthEnd);
            _ropeLine.SetVertexCount(2);
            _ropeLine.material = _material;
            _ropeLine.sortingLayerName = "EnvironmentBack";
            _ropeLine.SetColors(colorStart, colorEnd);
        }
    }

    //supprimer le pouvoir
    public override void KillPower() {
        Debug.Log("killPower Hook");
        //destroyHook();
    }

    //detruire le hook
    private void destroyHook() {
        Rigidbody2D rb2d;

        Debug.Log("destroyHook");
        Destroy(_ropeJoint);
        Destroy(_ropeLine);

        rb2d = _player.GetComponent<Rigidbody2D>();
        Vector2 v = rb2d.velocity * repulsion;
        //v.Normalize();
        //v *= force * 10;
        rb2d.AddForce(v);

        _pc.onAction = false;
        _pc.powerController = null;
    }

    //se déplacer autour du crochet
    public override void moveH() {
        float       horizontal;             //commande horizontale du user
        float       vertical;               //commande verticale du user
        Vector2     moveForce;              //force de mouvement
        Vector2 rf;
        float dx;
        float dy;
        Rigidbody2D rb2d;

        //recuperer l'input du user
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        rb2d = _player.GetComponent<Rigidbody2D>();


        //rotation
        rf = GetNormalTan(transform.position, _player.transform.position);
        dx = transform.position.x - _player.transform.position.x;
        dy = transform.position.y - _player.transform.position.y;

        //sens de rotation
        if (horizontal == 0) {
            _clockwise = 0;
        }
        else {
            if (_clockwise == 0) {
                _clockwise = (dy > 0) ? 1 : -1;
            }
        }

        //force à appliquer en horizontal
        moveForce = new Vector2(rf.y * _clockwise, -rf.x * _clockwise);
        moveForce = moveForce * force * horizontal;
        if (rb2d.velocity.magnitude < maxSpeed) {
            rb2d.AddForce(moveForce);
        }

        //definir l'orientation du sprite
        Vector3 scale = _player.transform.localScale;
        if (horizontal == 0) {
            if (rb2d.velocity.x != 0) {
                scale.x = rb2d.velocity.x / Mathf.Abs(rb2d.velocity.x);
            }
        }
        else {
            scale.x = horizontal / Mathf.Abs(horizontal);
        }
        _player.transform.localScale = scale;

        //descendre/grimper le long de la corde
        //_ropeJoint.distance = Mathf.Clamp(_ropeJoint.distance + vertical / 10, distanceMin, distanceMax);
    }

    //update de la corde
    void updateRope() {
        Vector3[] positions;

        if (_ropeLine) {
            //Debug.Log("draw _ropeLine");

            if(_ropeJoint.distance < distanceMax) {
                _ropeJoint.distance += .1f;
            }
                
            positions = new Vector3[2];
            positions[0] = _player.transform.position;
            positions[1] = transform.position;
            _ropeLine.SetPositions(positions);
        }
        
    }

    //normale tangentielle
    private Vector2 GetNormalTan(Vector2 center, Vector2 point) {
        Vector2 rf;
        float dx;
        float dy;
        float dist;

        dx = center.x - point.x;  //transform.position.x - _player.transform.position.x;
        dy = center.y - point.y;  //transform.position.y - _player.transform.position.y;
        dist = Vector3.Distance(center, point);
        rf = new Vector2(dx / dist, dy / dist);

        return rf;
    }  
}
