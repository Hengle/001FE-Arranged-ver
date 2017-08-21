using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMain : MonoBehaviour {

    public GameObject[,] matrixG;
    public int[,] objectAxis;
    public int[,] fieldRec;
    [SerializeField] float randomizer;

    // Use this for initialization
    void Start () {

        matrixG = new GameObject[7, 7];
        objectAxis = new int[7, 7];
        fieldRec = new int[7,7];

        GameObject prefab = (GameObject)Resources.Load("Objects/GrassTile");
        GameObject prefabW = (GameObject)Resources.Load("Objects/Wall");
        //GameObject stageObject = GameObject.FindWithTag("Stage");

        for(int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                
                if (prefab != null)
                {

                    if (Random.value < randomizer)
                    {
                        Vector3 tile_pos = new Vector3(0 + prefab.transform.localScale.x * i, 0.25f , 0 + prefab.transform.localScale.z * j);
                        GameObject instant_objectW = Instantiate(prefabW, tile_pos, Quaternion.identity);
                        //instant_objectW.transform.parent = stageObject.transform;
                        matrixG[i, j] = instant_objectW;
                        objectAxis[i, j] = -10;
                        fieldRec[i, j] = 1;
                    }
                    else
                    {
                        Vector3 tile_pos = new Vector3(0 + prefab.transform.localScale.x * i, 0, 0 + prefab.transform.localScale.z * j);
                        GameObject instant_object = Instantiate(prefab, tile_pos, Quaternion.identity);
                        //instant_object.transform.parent = stageObject.transform;
                        matrixG[i, j] = instant_object;
                        objectAxis[i, j] = -1;
                        fieldRec[i, j] = 0;
                    }
                }
            }
        }
    }

    void Update()
    {
        
    }

 

}

