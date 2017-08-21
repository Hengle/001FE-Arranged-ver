using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSelect : MonoBehaviour {
    int n;
    int m;
    public int[,] rec;

    //Getting stuff from Grid~
    Grid _gr;
    Grid axisChecker;
    //bool isButtonUp;
    RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス 
    Ray ray;  // 光線クラス
    TileBase tile_base;
    GameObject obj;

    void Start()
    {
        _gr = GetComponent<Grid>();
        rec = new int[_gr.gridSizeX, _gr.gridSizeY];
    }


    void Update()
    {
        axisChecker = GetComponent<Grid>();
        _gr = GetComponent<Grid>();

        if (Input.GetMouseButtonUp(0))
        {
            print("いま左ボタンが離された");
            //isButtonUp = true;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                tile_base = hit.collider.GetComponent<TileBase>();
                obj = hit.collider.gameObject;
            }
        }
        if (tile_base != null)
        {

            tile_base.bColourState = true;

            for (int i = 0; i < _gr.gridSizeX; i++)
            {
                for (int j = 0; j < _gr.gridSizeY; j++)
                {
                    if (axisChecker.fieldRec[i, j] == 2)
                    {
                        axisChecker.objectAxis[i, j] = -3;
                    }
                    if (axisChecker.fieldRec[i, j] == 1)
                    {
                        axisChecker.objectAxis[i, j] = -10;
                    }
                    if (axisChecker.fieldRec[i, j] == 0)
                    {
                        axisChecker.objectAxis[i, j] = -1;
                    }
                    if (axisChecker.matrixG[i, j] == obj)
                    {
                        axisChecker.objectAxis[i, j] = 3;
                        rec[i, j] = 3;
                        n = i;
                        m = j;
                        tile_base.bColourState1 = true;
                    }
                    else
                    {
                        rec[i, j] = 0;
                    }
                }
            }
        }
        Search4(n, m);
    }

    void Search4(int x, int y)
    {
        _gr = GetComponent<Grid>();
        if (x + 1 < _gr.gridSizeX)
        {
            Search(x + 1, y, rec[x, y]);
        }
        if (y + 1 < _gr.gridSizeY)
        {
            Search(x, y + 1, rec[x, y]);
        }
        if (x - 1 >= 0)
        {
            Search(x - 1, y, rec[x, y]);
        }
        if (y - 1 >= 0)
        {
            Search(x, y - 1, rec[x, y]);
        }
    }

    void Search(int x, int y, int tree)
    {
        axisChecker = GetComponent<Grid>();
        if (rec[x, y] < tree)
        {
            if (axisChecker.fieldRec[x, y] == 2)
            {
                axisChecker.objectAxis[x, y] = -3;
            }
            if (axisChecker.fieldRec[x, y] == 1)
            {
                axisChecker.objectAxis[x, y] = -10;
            }
            if (axisChecker.fieldRec[x, y] == 0)
            {
                axisChecker.objectAxis[x, y] = -1;
            }
            axisChecker.objectAxis[x, y] += tree;
            rec[x, y] = axisChecker.objectAxis[x, y];

            if (rec[x, y] >= 0)
            {
                tile_base = axisChecker.matrixG[x, y].GetComponent<TileBase>();
                if (tile_base != null)
                {
                    tile_base.bColourState = true;
                }
                Search4(x, y);
            }
        }



    }
}
