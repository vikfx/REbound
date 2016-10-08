using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject           target;             //cible à suivre
    public Vector3              margins;            //marges du viewport pour limiter le mouvement
    private SpringJoint2D    _ropeJoint;          //joint entre le player et l'objet
    private LineRenderer     _ropeLine;

    /////////////////////////////////////////////////// INIT ///////////////////////////////////////////////////////
    // Use this for initialization
    void Start () {
    }


    /////////////////////////////////////////////////// EVENTS /////////////////////////////////////////////////////
    // Update is called once per frame
    void Update () {
	
	}

    // Update is used before applying physics
    void FixedUpdate() {
        if (target) {
            Vector3     vPos;           //position de la cible dans le viewport
            Vector3     cPos;           //nouvelle posde la cam
            Camera      cam;            //camera

            //recuperer la camera
            cam = Camera.main;

            //recuperer les positions de la cible et de la camera
            vPos = cam.WorldToViewportPoint(target.transform.position);
            cPos = transform.position;

            //override de la position x de la cam
            if (vPos.x > 1 - margins.x) {
                cPos.x = cam.ViewportToWorldPoint(new Vector3(vPos.x - .5f + margins.x, 0, 0)).x;
                //Debug.Log("out right");                
            }
            else if (vPos.x < margins.x) {
                //Debug.Log("out left");
                cPos.x = cam.ViewportToWorldPoint(new Vector3(.5f + vPos.x - margins.x, 0, 0)).x;
            }
            else {
                //Debug.Log("in");
            }

            //override de la position y de la cam
            if (vPos.y > 1 - margins.y) {
                cPos.y = cam.ViewportToWorldPoint(new Vector3(0, vPos.y - .5f + margins.y, 0)).y;
                //Debug.Log("out top");
            }
            else if (vPos.y < margins.y) {
                //Debug.Log("out bottom");
                cPos.y = cam.ViewportToWorldPoint(new Vector3(0, .5f + vPos.y - margins.y, 0)).y;
            }
            else {
                //Debug.Log("in");
            }

            //appliquer la nouvelle position de la cam
            transform.position = cPos;
            //Debug.Log(transform.position.ToString());
        }
    }



    /////////////////////////////////////////////////// HELPERS ////////////////////////////////////////////////////
}
