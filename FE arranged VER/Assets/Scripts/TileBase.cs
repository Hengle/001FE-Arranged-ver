using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour {

    private Color default_color;  // 初期化カラー
    private Color select_colour;    // 選択時カラー
    private Color select_colour1;
    private Color select_colour2;
    private Color hidden_colour;

    protected Material _material;

    public bool bColourState;
    public bool bColourState1;
    public bool bColourState2;
    public bool hColourState;

    public bool bNormalColour;

    // Use this for initialization
    void Start()
    {
        // このクラスが付属しているマテリアルを取得 
        _material = GetComponent<Renderer>().material;
        // 選択時と非選択時のカラーを保持 
        default_color = _material.color;
        select_colour = Color.yellow;
        select_colour1 = Color.red;
        select_colour2 = Color.white;
        hidden_colour = Color.black;

        bColourState = false;
        bColourState1 = false;
        bColourState2 = false;
        hColourState = false;

        bNormalColour = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (bNormalColour)
        {
        _material.color = default_color;
        }

        // StageBaseからbColorStateの値がtrueにされていれば色をかえる 
        if (bColourState)
        {
            //bColorState = false;
            _material.color = select_colour;
        }
        if (bColourState1)
        {
            //bColorState1 = false;
            _material.color = select_colour1;
        }
        if (bColourState2)
        {
            //bColorState2 = false;
            _material.color = select_colour2;
        }
        if (hColourState)
        {
            _material.color = hidden_colour;
        }

    }

}
