﻿using UnityEngine;
using System.Collections;

public class EndController : PowerController {

    /////////////////////////////////////////////////// INIT ///////////////////////////////////////////////////////
    // Use this for initialization

    /////////////////////////////////////////////////// EVENTS /////////////////////////////////////////////////////

    /////////////////////////////////////////////////// HELPERS ////////////////////////////////////////////////////
    //ajouter le pouvoir
    public override void AddPower() {
        if (!_pc.gameOver) {
            _pc.winGame();
        }
    }
}
