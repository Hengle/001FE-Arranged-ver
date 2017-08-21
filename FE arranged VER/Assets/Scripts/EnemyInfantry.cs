using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfantry : MonoBehaviour {

    BaseClass infantryEnemy;

    void Start()
    {
        infantryEnemy = new BaseClass();
        infantryEnemy.className = "Infantry";
        infantryEnemy.classDescription = "A normal soldier type";
        infantryEnemy.baseHP = 100;
        infantryEnemy.baseDmg = 20;
        infantryEnemy.baseDef = 10;
        infantryEnemy.baseEnergy = 80;
        infantryEnemy.baseResources = 80;
        infantryEnemy.classSize = 1;
        infantryEnemy.isFight = false;//Just for now
        infantryEnemy.isSeen = false;//
        infantryEnemy.isCaution = false;//
        infantryEnemy.onHighGround = false;//
        infantryEnemy.inBuilding = false;//
        infantryEnemy.isMoveDone = false;
        infantryEnemy.hasAttacked = false;
        infantryEnemy.visibleArea = 3;
        infantryEnemy.moveableArea = 3;
    }

    public void EnemyInfantryMove()
    {

    }


}
