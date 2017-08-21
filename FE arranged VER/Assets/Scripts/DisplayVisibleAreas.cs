using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayVisibleAreas : MonoBehaviour {

    public LayerMask playerSoldier;
    public LayerMask unwalkableMask;
    public TileBase tile_base;
    PlayerChoice playerChoiceScript;
    public List<GameObject> playerSoldiers;
    public int numberPlayerSoldierOnBoard;

    void Start () {
        playerChoiceScript = GetComponent<PlayerChoice>();
        numberPlayerSoldierOnBoard = 0;
	}
	
    
    public void DisplayVisible()
    {
        foreach (Node node in Grid.instance.grid)
        {
            bool walkable = !(Physics.CheckSphere(node.worldPosition, Grid.instance.nodeRadius - 0.01f, unwalkableMask));

            if (walkable)
            {
             Ray ray = new Ray(node.worldPosition + Vector3.up * 50, Vector3.down);
             RaycastHit hit;
                 if (Physics.Raycast(ray, out hit))
                 {
                     tile_base = hit.collider.GetComponent<TileBase>();
                     tile_base.hColourState = true;
                 }

            }
        }
        foreach (Node node in Grid.instance.grid)
        {
            bool playerSoldierExists = Physics.CheckSphere(node.worldPosition, Grid.instance.nodeRadius - 0.01f, playerSoldier);
            if (playerSoldierExists)
            {
                Ray ray = new Ray(node.worldPosition + Vector3.up * 50, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    tile_base = hit.collider.GetComponent<TileBase>();
                    tile_base.hColourState = false;
                    tile_base.bNormalColour = true;
                    playerSoldiers.Add(hit.collider.gameObject);
                    playerChoiceScript.DisplayingVisibleAreas(hit);
                }
            }
        }
        numberPlayerSoldierOnBoard = playerSoldiers.Count;
        StateMachine.currentState = StateMachine.BattleStates.PLAYERCHOICE;
    }

}
