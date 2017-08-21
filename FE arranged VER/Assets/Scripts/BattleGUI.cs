using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGUI : MonoBehaviour {
    /*
    private void OnGUI()
    {
        if (StateMachine.currentState == StateMachine.BattleStates.SELECTMOVE)
        {
            DisplayMoveChoice();
        }
    }
    */
    private void OnGUI()
    {
        
    }

    public void DisplayMoveChoice()
    {
        if (GUI.Button(new Rect (Screen.width - 250, Screen.height - 50, 100, 30), "Move1"))
        {

            StateMachine.currentState = StateMachine.BattleStates.CALCDAMAGE;
        }
        if (GUI.Button(new Rect(Screen.width - 125, Screen.height - 50, 100, 30), "Move2"))
        {

            StateMachine.currentState = StateMachine.BattleStates.CALCDAMAGE;
        }
    }

}
