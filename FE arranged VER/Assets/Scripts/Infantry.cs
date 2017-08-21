using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Infantry : MonoBehaviour {

    public BaseClass infantry;
    PlayerChoice playerChoiceScript;
    RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス 
    Ray ray;  // 光線クラス
    GameObject selectedTile;
    TileBase selectedTileColour;
    Vector3 selectedTileVector3;

    void Start()
    {
        infantry = new BaseClass();
        infantry.className = "Infantry";
        infantry.classDescription = "A normal soldier type";
        infantry.baseHP = 100;
        infantry.baseDmg = 20;
        infantry.baseDef = 10;
        infantry.baseEnergy = 80;
        infantry.baseResources = 80;
        infantry.classSize = 1;
        infantry.isFight = false;//Just for now
        infantry.isSeen = false;//
        infantry.isCaution = false;//
        infantry.onHighGround = false;//
        infantry.inBuilding = false;//
        infantry.isMoveDone = false;
        infantry.hasAttacked = false;
        infantry.visibleArea = 3;
        infantry.moveableArea = 3;
        ButtonPressed = false;
        ButtonPressedSelectAttack = false;
        isOnGUIonSelectMovingArea = false;
        isOnGUIonSelectAttack = false;
        InfantryGetMouseButtonUp = false;
    }

    bool InfantryGetMouseButtonUp;
    bool AHOKUSA;

    public void InfantryMove(GameObject objSelectedSoldier)
    {
        //bool AHOKUSA is to prevent the Input.GetMouseButtonUp(0) to be entering
        //this function in the same time as the function 
        //from PlayerChoice, which this is called from.
        if (AHOKUSA) {
            if (Input.GetMouseButtonUp(0))
            {
                InfantryGetMouseButtonUp = true;
            }
        }


        if (InfantryGetMouseButtonUp)
        {
            print("Is this coming up to GetMouseButtonUp?");
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                print("Is this coming up to Raycast?");
                if (hit.collider.GetComponent<TileBase>() != null)
                {
                    if (hit.collider.GetComponent<Infantry>() == null)
                    {
                        if (hit.collider.GetComponent<TileBase>().bColourState == true
                        || hit.collider.GetComponent<TileBase>().bColourState1 == true)
                        {
                            selectedTileColour = hit.collider.GetComponent<TileBase>();
                            selectedTile = hit.collider.gameObject;
                            selectedTileVector3 = selectedTile.transform.position;
                        }
                    }
                    if (selectedTile != null
                    && hit.collider.GetComponent<TileBase>().bColourState == false
                        && hit.collider.GetComponent<TileBase>().bColourState1 == false)
                    {
                        selectedTileColour = null;
                        selectedTile = null;
                    }
                }
            }
            InfantryGetMouseButtonUp = false;
        }
        AHOKUSA = true;
        if (selectedTile != null)
        {
            selectedTileColour.bColourState2 = true;
            isOnGUIonSelectMovingArea = true;
        }
        DisplayMoveChoice(objSelectedSoldier, selectedTileVector3);
    }
    
    bool ButtonPressed;
    bool ButtonPressedSelectAttack;
    public bool isOnGUIonSelectMovingArea;
    public bool isOnGUIonSelectAttack;

    private void OnGUI()
    {
        if (isOnGUIonSelectMovingArea)
        {
            if (GUI.Button(new Rect(Screen.width - 125, Screen.height - 50, 100, 30), "MoveOk"))
            {
                ButtonPressed = true;
            }
        }
        if (isOnGUIonSelectAttack)
        {
            if (GUI.Button(new Rect(Screen.width - 125, Screen.height - 50, 100, 30), "AttackOk"))
            {
                ButtonPressedSelectAttack = true;
            }
        }
    }

    public void DisplayMoveChoice(GameObject objSelectedSoldier, Vector3 selectedTileVector3)
    {
        if (ButtonPressed)
        {
            objSelectedSoldier.transform.position = selectedTileVector3;
            AHOKUSA = false;
            ButtonPressed = false;
            isOnGUIonSelectMovingArea = false;
            PlayerChoice.instance.isInfantrySelect = false;
            PlayerChoice.instance.tile_base.bColourState1 = false;
            PlayerChoice.instance.tile_base = null;
            selectedTileColour = null;
            selectedTile = null;
            InfantryGetMouseButtonUp = false;

            infantry.isMoveDone = true;

        }
    }

    public void InfantryAttack(GameObject objSelectedSoldier)
    {
        if (AHOKUSA)
        {
            if (Input.GetMouseButtonUp(0))
            {
                InfantryGetMouseButtonUp = true;
            }
        }


        if (InfantryGetMouseButtonUp)
        {
            //print("MouseButtonUp Infantry verAtt");
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                print("Raycast Infantry verAtt");
                if (hit.collider.GetComponent<TileBase>() != null)
                {
                    if (hit.collider.GetComponent<Infantry>() == null)
                    {
                        if (hit.collider.GetComponent<TileBase>().bColourState == true
                        || hit.collider.GetComponent<TileBase>().bColourState1 == true)
                        {
                            selectedTileColour = hit.collider.GetComponent<TileBase>();
                            selectedTile = hit.collider.gameObject;
                        }
                    }
                    if (selectedTile != null
                    && hit.collider.GetComponent<TileBase>().bColourState == false
                        && hit.collider.GetComponent<TileBase>().bColourState1 == false)
                    {
                        selectedTileColour = null;
                        selectedTile = null;
                    }
                }
            }
            InfantryGetMouseButtonUp = false;
        }
        AHOKUSA = true;
        if (selectedTile != null)
        {
            selectedTileColour.bColourState2 = true;
            isOnGUIonSelectAttack = true;
        }

        DisplayAttackChoice(objSelectedSoldier);

    }

    public void DisplayAttackChoice(GameObject objSelectedSoldier)
    {
        if (ButtonPressedSelectAttack)
        {

            //Write the Attack code in here
            PlayerChoice.instance.moveDoneSoldiers.Add(objSelectedSoldier);
            PlayerChoice.instance.hPGause.SetActive(false);
            AHOKUSA = false;
            ButtonPressedSelectAttack = false;
            isOnGUIonSelectAttack = false;
            PlayerChoice.instance.isInfantrySelect = false;
            PlayerChoice.instance.obj.GetComponent<TileBase>().bColourState1  = false;
            //PlayerChoice.instance.obj = null;
            selectedTileColour = null;
            selectedTile = null;
            InfantryGetMouseButtonUp = false;
            infantry.isMoveDone = false;//Make a function that can make the hasAttacked true even when you do not have the selectable enemy
            infantry.hasAttacked = true;
        }
    }

}
