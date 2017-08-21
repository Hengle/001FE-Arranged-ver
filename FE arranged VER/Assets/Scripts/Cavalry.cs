using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cavalry : MonoBehaviour {

    public BaseClass cavalry;

    private void Start()
    {

        cavalry = new BaseClass();
        cavalry.className = "Cavalry";
        cavalry.classDescription = "A soldier for franking the enemy";
        cavalry.baseHP = 80;
        cavalry.baseDmg = 15;
        cavalry.baseDef = 8;
        cavalry.baseEnergy = 80;
        cavalry.baseResources = 70;
        cavalry.classSize = 1;
        cavalry.isFight = false;//Just for now
        cavalry.isSeen = false;//
        cavalry.isCaution = false;//
        cavalry.onHighGround = false;//
        cavalry.inBuilding = false;//
        cavalry.isMoveDone = false;
        cavalry.hasAttacked = false;
        cavalry.visibleArea = 5;
        cavalry.moveableArea = 5;

    }

    private void OnGUI()
    {
        
    }

    public void CavalryMove()
    {

    }

    public void CavalryMoveChoice()
    {

    }


}
