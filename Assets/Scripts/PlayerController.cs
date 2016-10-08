using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D     _rb2d;                                      //rigidbody du character
    //private Collider2D      _c2d;                                     //collider du player
    public Vector2          minForce;                                   //force min à appliquer
    public Vector2          maxForce;                                   //force max à appliquer
    public Vector2          idleForce;                                  //force au repos
    public Vector2          maxSpeed;                                   //vitesse de déplacement max
    public float            velocityYMin;                               //velocité minimum pour le petit saut
    public float            velocityYMax;                               //velocité minimum pour le grand saut
    public float            rotationMultiplier;                         //multiplicateur pour la rotation du personnage en mouvement
    
    public bool             onAction;                                   //si le player est en action
    [HideInInspector] public PowerController  powerController;                            //l'acteur de l'action

    public float            tailLength;                                 //longueur de la queue pour chaque point du score
    private TrailRenderer   _tail;                                      //la queue
    private int             _score;                                     //score du player
    private int             _numOfStars;                                //nombre d'étoiles dans le niveau
    private int             _maxScore;                                  //score total de toutes les étoiles
    private float           _scale;

    private Light           _light;                                     //lumiere de l'objet
    public float            lightInc;                                   //intensité déclinante par frame
    public float            reboundLight;                               //intensité de lumière pour le rebond


    public  Text            scoreText;                                  //texte pour afficher le temps et le score
    public bool             gameOver;                                   //si le niveau est terminé
    [HideInInspector] public Vector3          checkpoint;                                 //point de retour en cas de gameover

    /////////////////////////////////////////////////// INIT ///////////////////////////////////////////////////////
    // Use this for initialization
    void Start() {
        _rb2d = GetComponent<Rigidbody2D>();

        //init l'action
        onAction = false;
        powerController = null;

        //init la lumiere et la queue
        _tail = GetComponent<TrailRenderer>();
        _light = GetComponent<Light>();
        //Component[] l = GetComponentsInChildren(typeof(Light));
        //_light = l[0] as Light;

        //init le score et l'etat de jeu
        score = 0;
        gameOver = false;
        GameObject[] stars;
        stars = GameObject.FindGameObjectsWithTag("Collectable");
        _numOfStars = stars.Length;
        _maxScore = 0;

        _scale = transform.localScale.x;

        foreach(GameObject star in stars) {
            _maxScore += star.GetComponent<StarController>().score;
        }

        //_c2d = GetComponent<Collider2D>();
        Debug.Log("player ready!");
    }


    /////////////////////////////////////////////////// EVENTS /////////////////////////////////////////////////////
    // Update is called once per frame
    void Update() {
        if(!gameOver) {
            drawScore();

            if (lightIntensity > 0) {
                lightIntensity -= lightInc;
            }
        }
    }

    // Update is used before applying physics
    void FixedUpdate() {
        //deplacement du player
        if (!onAction) {
            moveH();
        }
        else {
            powerController.moveH();
        }

        if (checkpoint != null && Input.GetButton("Fire2")) {
            MoveToCheckpoint();
        }

        //
        //triggerKillPower();
    }

    //Sorties de collision
    void OnCollisionExit2D(Collision2D coll) {
        switch (coll.gameObject.tag) {
            //environnement : rebondir
            case "Environment":
                rebound(coll);
                break;

            //defaut
            default:
                break;
        }
    }



    /////////////////////////////////////////////////// HELPERS ////////////////////////////////////////////////////
    //rebondir
    public void rebound(Collision2D coll) {
        Vector2     normal;             //normale de la collision
        Vector2     refForce;           //force de reference selon les commandes users
        Vector2     jumpForce;          //force de saut
        float       vertical;           //commande verticale du user

        //recuperer l'input du user
        vertical = Input.GetAxis("Vertical");

        //definition de la force de reference
        refForce = new Vector2();

        if (vertical == 0) {
            refForce.y = idleForce.y;
        }
        else {
            refForce.y = (vertical > 0) ? maxForce.y : minForce.y;
            refForce.y *= Mathf.Abs(vertical);
        }

        //normal de contact
        normal = coll.contacts[0].normal;

        //forces à appliquer
        jumpForce = new Vector2(0, refForce.y * normal.y);

        //limiter la velocité Y
        if(vertical < 0 && Mathf.Abs(_rb2d.velocity.y) > velocityYMin && Mathf.Abs(normal.y) > 0.5f) {
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, velocityYMin * normal.y);
        }

        if(vertical > 0 && Mathf.Abs(_rb2d.velocity.y) < velocityYMax && Mathf.Abs(normal.y) > 0.5f) {
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, velocityYMax * normal.y);
        }

        if (Mathf.Abs(_rb2d.velocity.y) < velocityYMin && Mathf.Abs(normal.y) > 0.5f) {
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, velocityYMin * normal.y);
        }

        //if (Mathf.Abs(_rb2d.velocity.y) > velocityYMax && Mathf.Abs(normal.y) > 0.5f) {
        //    _rb2d.velocity = new Vector2(_rb2d.velocity.x, velocityYMax * normal.y);
        //}

        //application de la force dans les limites
        if (_rb2d.velocity.y > -maxSpeed.y) {
            _rb2d.AddForce(jumpForce);
        }

        //illuminer le character
        lightIntensity = reboundLight * /*(vertical + 2) * */ Mathf.Abs(_rb2d.velocity.y);
        //Debug.Log("y : " + Mathf.Abs(_rb2d.velocity.y) + ", magn : " + _rb2d.velocity.magnitude);
    }

    //se déplacer sur l'axe Horizontal
    public void moveH() {
        float       horizontal;             //commande horizontale du user
        Vector2     moveForce;              //force de mouvement
        Vector3     scale;                  //orientation G / D du character
        float     rotation;               //angle d'inclinaison selon la velocité

        //recuperer l'input du user
        horizontal = Input.GetAxis("Horizontal");

        //definir la force
        moveForce = new Vector2();
        moveForce.x = maxForce.x * horizontal;
        if (Mathf.Abs(_rb2d.velocity.x) < maxSpeed.x || (_rb2d.velocity.x / moveForce.x < 0)) {
            _rb2d.AddForce(moveForce);
        }

        //definir l'orientation du sprite
        scale = transform.localScale;
        rotation = _rb2d.velocity.x * rotationMultiplier;
        if (horizontal == 0) {
            if (_rb2d.velocity.x != 0) {
                scale.x = _rb2d.velocity.x / Mathf.Abs(_rb2d.velocity.x);
            }
        }
        else {
            scale.x = horizontal / Mathf.Abs(horizontal);
        }

        scale.x *= _scale;
        transform.localScale = scale;
        transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
    }

    //afficher la fin du niveau
    public void winGame() {
        gameOver = true;
        drawScore();
        scoreText.text += "\nYOU WIN!!!";
    }

    //afficher la partie comme perdu
    public void looseGame() {
        //gameOver = true;
        //drawScore();
        //scoreText.text += "\nYOU LOOOOOSE!!!";
        MoveToCheckpoint();

        //Destroy(gameObject);
    }

    //afficher le score
    public void drawScore() {
        scoreText.text = "Score : " + _score.ToString() + " / " + _maxScore.ToString() + "\nTime : " + timeFormat(Time.time);
    }


    //formatage du temps (t en s)
    private string timeFormat(float t) {
        string tf;
        float m;
        float s;
        float ms;

        m = Mathf.Floor(t / 60);
        s = Mathf.Floor(t % 60);
        ms = Mathf.Floor((t * 1000) % 1000);

        tf = "";
        //tf += t.ToString();
        tf += (m < 10) ? "0" + m.ToString() : m.ToString();
        tf += ":";
        tf += (s < 10) ? "0" + s.ToString() : s.ToString();
        tf += ".";
        tf += (ms < 10) ? "00" + ms.ToString() : (ms < 100) ? "0" + ms.ToString() : ms.ToString();

        return tf;
    }


    //revenir au checkpoint
    public void MoveToCheckpoint() {
        transform.position = checkpoint;
        _rb2d.velocity = Vector3.zero;

        _tail.Clear();

        if (powerController != null) {
            powerController.KillPower();
        }
        onAction = false;
        powerController = null;
    }

    /////////////////////////////////////////////////// GETTERS/SETTERS ////////////////////////////////////////////
    //le score
    public int score {
        get { return _score;  }
        set {
            _score = value;
            _tail.time = _score * tailLength;
        }
    }

    public float lightIntensity {
        get { return _light.range; } //_light.intensity;  }
        set {
            //_light.intensity = value/20;
            _light.range = value;
        }
    }
}
