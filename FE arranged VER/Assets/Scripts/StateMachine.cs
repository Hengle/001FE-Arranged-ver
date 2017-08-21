using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour {

    PlayerChoice playerChoicescript;
    DisplayVisibleAreas displayVisibleAreasScript;

	public enum BattleStates
    {
        START,
        PLAYERCHOICE,
        DISPLAYVISIBLEAREA,
        CALCDAMAGE,
        ENEMYCHOICE,
        LOSE,
        WIN
    }

    public static BattleStates currentState;

    void State()
    {
        //playerChoicescript = new PlayerChoice();
        currentState = BattleStates.START;
    }
    private void Update()
    {
        playerChoicescript = GetComponent<PlayerChoice>();
        displayVisibleAreasScript = GetComponent<DisplayVisibleAreas>();
        Debug.Log(currentState);
        switch (currentState)
        {
            case (BattleStates.START):
                currentState = BattleStates.DISPLAYVISIBLEAREA;//For now
                break;
            case (BattleStates.PLAYERCHOICE):
                playerChoicescript.PlayerChoose();
                break;
            case (BattleStates.DISPLAYVISIBLEAREA):
                displayVisibleAreasScript.DisplayVisible();
                break;
            case (BattleStates.CALCDAMAGE):

                break;
            case (BattleStates.ENEMYCHOICE):

                break;
            case (BattleStates.LOSE):

                break;
            case (BattleStates.WIN):

                break;
        }
    }


    private void OnGUI()
    {
        if(GUILayout.Button("NEXT STATE"))
        {
            if (currentState == BattleStates.START)
            {
                currentState = BattleStates.DISPLAYVISIBLEAREA;
            }
            else if (currentState == BattleStates.DISPLAYVISIBLEAREA)
            {
                currentState = BattleStates.PLAYERCHOICE;
            }
            else if (currentState == BattleStates.PLAYERCHOICE)
            {
                currentState = BattleStates.CALCDAMAGE;
            }
            else if (currentState == BattleStates.CALCDAMAGE)
            {
                currentState = BattleStates.ENEMYCHOICE;
            }
            else if (currentState == BattleStates.ENEMYCHOICE)
            {
                currentState = BattleStates.LOSE;
            }
            else if (currentState == BattleStates.LOSE)
            {
                currentState = BattleStates.WIN;
            }
            else if (currentState == BattleStates.WIN)
            {
                currentState = BattleStates.START;
            }
        }
    }



}
