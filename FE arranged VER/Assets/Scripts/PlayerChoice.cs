using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChoice : MonoBehaviour {

    [SerializeField] public GameObject hPGause;

    public List<GameObject> moveDoneSoldiers;

    //BattleGUI battleGUIscript;
    int n;
    int m;
    public int[,] rec;

    float fakeX;
    float fakeY;

    //Getting stuff from Grid
    Grid gridScript;
    RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス 
    Ray ray;  // 光線クラス
    public TileBase tile_base;
    public GameObject obj;
    public Infantry infantryScript;
    public Cavalry cavalryScript;
    public bool isInfantrySelect;

    public static PlayerChoice instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        //battleGUIscript = new BattleGUI();

        //PlayerChoiceGetMouseButtonUp = false;
        isInfantrySelect = false;
        infantryScript = null;
        gridScript = GetComponent<Grid>();
        rec = new int[gridScript.gridSizeX, gridScript.gridSizeY];
        for (int i = 0; i < gridScript.gridSizeX; i++)
        {
            for (int j = 0; j < gridScript.gridSizeY; j++)
            {
                if (gridScript.fieldRec[i, j] == 2)
                {
                    gridScript.objectAxis[i, j] = -3;
                }
                if (gridScript.fieldRec[i, j] == 1)
                {
                    gridScript.objectAxis[i, j] = -10;
                }
                if (gridScript.fieldRec[i, j] == 0)
                {
                    gridScript.objectAxis[i, j] = -1;
                }
                rec[i, j] = 0;
            }
        }
    }

    TileBase ColourResetter;
    //bool PlayerChoiceGetMouseButtonUp;

    public void PlayerChoose()
    {
        gridScript = GetComponent<Grid>();

        //initializiation on the colour of the tile base, this must be set at the start of the calling function
        //this used to be included in the TileBase class. 
        //However it made the bug within the function where to determine the colour from the raycasted object.
        for (int i = 0; i < gridScript.gridSizeX; i++)
        {
            for (int j = 0; j < gridScript.gridSizeY; j++)
            {
                if (gridScript.matrixG[i, j].GetComponent<TileBase>() != null)
                {
                    ColourResetter = gridScript.matrixG[i, j].GetComponent<TileBase>();
                    ColourResetter.bNormalColour = true;
                    ColourResetter.bColourState = false;
                    ColourResetter.bColourState1 = false;
                    ColourResetter.bColourState2 = false;
                }
            }
        }

        //Determining and selecting the soldier (Infantry)
        if (Input.GetMouseButtonUp(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (!moveDoneSoldiers.Contains(hit.collider.gameObject))
                {
                    if (hit.collider.GetComponent<TileBase>() != null)
                    {
                        if (hit.collider.GetComponent<Infantry>() != null)
                        {
                            InfantrySelected(hit);
                        }
                    }
                }
            }

        }

        //Map colouring from the datas :D
        if (tile_base != null)
        {
            tile_base.bColourState1 = true;
            //Initializing the objectAxis and rec to prevent from the data from before to be reflected.
            for (int i = 0; i < gridScript.gridSizeX; i++)
            {
                for (int j = 0; j < gridScript.gridSizeY; j++)
                {
                    if (gridScript.fieldRec[i, j] == 2)
                    {
                        gridScript.objectAxis[i, j] = -3;
                    }
                    if (gridScript.fieldRec[i, j] == 1)
                    {
                        gridScript.objectAxis[i, j] = -10;
                    }
                    if (gridScript.fieldRec[i, j] == 0)
                    {
                        gridScript.objectAxis[i, j] = -1;
                    }
                    rec[i, j] = 0;
                }
            }
            TileBase selectedTile = gridScript.matrixG[n, m].GetComponent<TileBase>();
            if (selectedTile != null)
            {
                selectedTile.bColourState1 = true;
            }
            if (infantryScript != null)
            {
                gridScript.objectAxis[n, m] = infantryScript.infantry.moveableArea;
                rec[n, m] = infantryScript.infantry.moveableArea;
            }
            Search4(n, m);
        }

        if (isInfantrySelect)
        {
            if (obj != null)
            {
                infantryScript.InfantryMove(obj);
            }
        }

        if (obj != null)
        {
            if (obj.GetComponent<Infantry>().infantry.isMoveDone)
            {
                n = NodeFromWorldPointX(obj.transform.position);
                m = NodeFromWorldPointY(obj.transform.position);

                obj.GetComponent<TileBase>().bColourState1 = true;
                for (int x = -1; x <= 1; x++)
                {
                     if (0 <= n + x && n + x < gridScript.gridSizeX)
                     {
                        if (gridScript.matrixG[n + x, m].GetComponent<TileBase>() != null)
                        {
                            TileBase selectAttackTile = gridScript.matrixG[n + x, m].GetComponent<TileBase>();
                            selectAttackTile.hColourState = false;
                            selectAttackTile.bColourState = true;
                        }
                     }
                }

                for (int y = -1; y <= 1; y++)
                {
                    if(!(y == 0))
                    {
                        if (0 <= m + y && m + y < gridScript.gridSizeY)
                        {
                            if (gridScript.matrixG[n, m + y].GetComponent<TileBase>() != null)
                            {
                                TileBase selectAttackTile = gridScript.matrixG[n, m + y].GetComponent<TileBase>();
                                selectAttackTile.hColourState = false;
                                selectAttackTile.bColourState = true;
                            }
                        }
                    }
                }
                infantryScript.InfantryAttack(obj);
            }
        }


        if (Input.GetMouseButtonUp(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<TileBase>() != null)
                {
                    if (tile_base != null)
                    {
                        tile_base.bColourState1 = false;
                    }
                    if (hit.collider.GetComponent<TileBase>().bColourState == false
                        && hit.collider.GetComponent<TileBase>().bColourState1 == false
                        && hit.collider.GetComponent<Infantry>() == null
                        && obj != null)
                    {
                        print("Clicking out of the Selected Soldier");
                        tile_base = null;
                        obj.GetComponent<Infantry>().infantry.isMoveDone = false;
                        obj.GetComponent<TileBase>().bColourState1 = false;
                        obj = null;
                        infantryScript.isOnGUIonSelectAttack = false;
                        //isInfantrySelect = false;
                        infantryScript.isOnGUIonSelectMovingArea = false;
                        hPGause.SetActive(false);
                    }
                }
            }
        }

        if (obj != null)
        {
            if (infantryScript.infantry.hasAttacked)//This is not good, the best is to see all the soldiers and see 
                //if they have attacked, maybe list it and .Length == would be good
                //it might not be good if this is gonna have fluid soldier number change
            {
                infantryScript.infantry.hasAttacked = false;
                StateMachine.currentState = StateMachine.BattleStates.PLAYERCHOICE;
            }
        }

    }

    public void DisplayingVisibleAreas(RaycastHit hit)
    {
        obj = hit.collider.gameObject;
        n = NodeFromWorldPointX(obj.transform.position);
        m = NodeFromWorldPointY(obj.transform.position);
        if (hit.collider.GetComponent<Infantry>() != null)
        {
            infantryScript = hit.collider.GetComponent<Infantry>();
            gridScript.objectAxis[n, m] = infantryScript.infantry.moveableArea;
            rec[n, m] = infantryScript.infantry.moveableArea;
        }
        if (hit.collider.GetComponent<Cavalry>() != null)
        {
            cavalryScript = hit.collider.GetComponent<Cavalry>();
            gridScript.objectAxis[n, m] = cavalryScript.cavalry.moveableArea;
            rec[n, m] = cavalryScript.cavalry.moveableArea;
        }
        SearchDiff4(n, m);
    }


    void Search4(int x, int y)
    {
        gridScript = GetComponent<Grid>();
        if (x + 1 < gridScript.gridSizeX)
        {
            Search(x + 1, y, rec[x, y]);
        }
        if (y + 1 < gridScript.gridSizeY)
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
        gridScript = GetComponent<Grid>();
        if (rec[x, y] < tree)
        {
            if (gridScript.fieldRec[x, y] == 2)
            {
                gridScript.objectAxis[x, y] = -2;
            }
            if (gridScript.fieldRec[x, y] == 1)
            {
                gridScript.objectAxis[x, y] = -10;
            }
            if (gridScript.fieldRec[x, y] == 0)
            {
                gridScript.objectAxis[x, y] = -1;
            }
            gridScript.objectAxis[x, y] += tree;
            rec[x, y] = gridScript.objectAxis[x, y];

            if (rec[x, y] >= 0)
            {
                TileBase tile_base1 = gridScript.matrixG[x, y].GetComponent<TileBase>();
                if (tile_base1 != null)
                {
                    tile_base1.bColourState = true;
                    tile_base1.hColourState = false;
                }
                Search4(x, y);
            }
        }
    }


    void SearchDiff4(int x, int y)
    {
        gridScript = GetComponent<Grid>();
        if (x + 1 < gridScript.gridSizeX)
        {
            SearchDiff(x + 1, y, rec[x, y]);
        }
        if (y + 1 < gridScript.gridSizeY)
        {
            SearchDiff(x, y + 1, rec[x, y]);
        }
        if (x - 1 >= 0)
        {
            SearchDiff(x - 1, y, rec[x, y]);
        }
        if (y - 1 >= 0)
        {
            SearchDiff(x, y - 1, rec[x, y]);
        }
    }

    void SearchDiff(int x, int y, int tree)
    {
        gridScript = GetComponent<Grid>();
        if (rec[x, y] < tree)
        {
            if (gridScript.fieldRec[x, y] == 2)
            {
                gridScript.objectAxis[x, y] = -2;
            }
            if (gridScript.fieldRec[x, y] == 1)
            {
                gridScript.objectAxis[x, y] = -10;
            }
            if (gridScript.fieldRec[x, y] == 0)
            {
                gridScript.objectAxis[x, y] = -1;
            }
            gridScript.objectAxis[x, y] += tree;
            rec[x, y] = gridScript.objectAxis[x, y];

            if (rec[x, y] >= 0)
            {
                TileBase tile_base1 = gridScript.matrixG[x, y].GetComponent<TileBase>();
                if (tile_base1 != null)
                {
                    tile_base1.bNormalColour = true;
                    tile_base1.hColourState = false;
                }
                SearchDiff4(x, y);
            }
        }
    }

    public Text hPGauseText;

    public void InfantrySelected(RaycastHit hit)
    {
        if (!hit.collider.GetComponent<Infantry>().infantry.isMoveDone)
        {
            print("Clicking and Selecting a Soldier");
            tile_base = hit.collider.GetComponent<TileBase>();
            obj = hit.collider.gameObject;
            n = NodeFromWorldPointX(obj.transform.position);
            m = NodeFromWorldPointY(obj.transform.position);
            infantryScript = hit.collider.GetComponent<Infantry>();
            hPGause.SetActive(true);
            isInfantrySelect = true;
            hPGauseText = hPGause.GetComponent<Text>();
            hPGauseText.text = infantryScript.infantry.baseHP.ToString();
        }
    }



    public int NodeFromWorldPointX(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + Grid.instance.gridWorldSize.x / 2) / Grid.instance.gridWorldSize.x;
        percentX = Mathf.Clamp01(percentX);

        int x = Mathf.RoundToInt((gridScript.gridSizeX - 1) * percentX);
        return x;
    }
    public int NodeFromWorldPointY(Vector3 worldPosition)
    {
        float percentY = (worldPosition.z + Grid.instance.gridWorldSize.y / 2) / Grid.instance.gridWorldSize.y;
        percentY = Mathf.Clamp01(percentY);

        int y = Mathf.RoundToInt((gridScript.gridSizeY - 1) * percentY);
        return y;
    }

}
