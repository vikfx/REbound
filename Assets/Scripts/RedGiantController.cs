using UnityEngine;
using System.Collections;

public class RedGiantController : EnemyController {
    public float        timeAttack;                 //temps entre chaque attaque
    private float       _nextAttack;                //temps de la prochaine attaque
    public GameObject   bullet;                     //projectile
    public int          numOfBullet;                //nombre de projectiles par lancé
    public float        radius;


    private Light           _light;                 //lumiere de l'objet
    public float            lightIncMin;               //intensité déclinante par frame
    public float            lightIncMax;               //intensité déclinante par frame
    public float            pulseLight;             //intensité de lumière pour l'attaque
    public float            pulseDecay;             //decallage de temps entre la lumiere et l'envoi des projectiles                                   

    /////////////////////////////////////////////////// INIT ///////////////////////////////////////////////////////
    // Use this for initialization
    internal override void Start() {
        Debug.Log("red giant init");

        _nextAttack = timeAttack;
        _light = GetComponent<Light>();

        base.Start();
    }

    /////////////////////////////////////////////////// EVENTS /////////////////////////////////////////////////////
    // Update is called once per frame
    internal override void Update() {
        
    }

    //update physics
    internal override void FixedUpdate() {
        lightEnemy();
        launchBullets();
    }

    /////////////////////////////////////////////////// HELPERS ////////////////////////////////////////////////////
    //lancer une action en collsion
    public override void AddAttack() {
        //_pc.looseGame();
        onAttack = true;
    }

    //update d'une attaque en collision
    public override void StayAttack() {
        //launchBullets();
    }

    //lancer les projectiles
    public void launchBullets() {
        Quaternion angle;   //angle du projectile
        Vector3 position;
        float incA;         //angle d'incrementation pour chaque projectile en degrés           
        float pa;           //angle pour la position

        if (onAttack && Time.time >= _nextAttack) {
            //incrementer le temps pour la prochaine attaque
            _nextAttack = Time.time + timeAttack;

            //definir l'incrementation de l'angle
            incA = 360 / numOfBullet;

            //creer les projectiles
            for(int i = 0; i < numOfBullet; i++) {
                angle = Quaternion.AngleAxis(incA * i, Vector3.forward);
                pa = (incA * i + 90) * Mathf.Deg2Rad;

                position = new Vector3(transform.position.x + radius * Mathf.Cos(pa), transform.position.y + radius * Mathf.Sin(pa), transform.position.z);

                GameObject obj = Instantiate(bullet, position, angle) as GameObject;
                obj.transform.parent = transform;
            }
        }

        
    }

    //eclairer l'enemie
    public void lightEnemy() {
        //if (onAttack && Time.time >= _nextAttack - pulseDecay) {
        //    _light.range = pulseLight;
        //}

        //if (_light.range >= lightInc) {
        //    _light.range -= lightInc;
        //}


        if(onAttack) {
            if(Time.time >= _nextAttack - pulseDecay) {
                if (_light.range < pulseLight) {
                    _light.range += lightIncMax;
                }
            } else if(_light.range >= lightIncMin) {
                _light.range -= lightIncMin;
            }
        }



    }
}
