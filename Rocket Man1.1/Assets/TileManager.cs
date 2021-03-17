using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileManager : MonoBehaviour
{
    public bool IsScrolling { set; get; }
    public GameObject[] tilePrefabs; 

    private Transform PlayerTransform;
    private float SafeZone = 65.0f;//25
    private float spawnZ= -60.0f;//-20
    private float tileLength = 23.0f;
    private int amntTileOnScreen = 15;
   //private int lastPrefabIndex = 0;


    private List<GameObject> activeTiles;

    // Start is called before the first frame update
    private void Start()
    {
        activeTiles = new List<GameObject>();
        PlayerTransform = GameObject.FindGameObjectWithTag ("Player").transform;

        for(int i=0; i<amntTileOnScreen; i++) 
        {
           // if(i < 2)
               // SpawnTile(0); //first two time spawn normal tile
           // else
            SpawnTile();
            
        }
        
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(PlayerTransform.position.z -SafeZone > (spawnZ - amntTileOnScreen * tileLength))
        {
            SpawnTile();
            DeleteTile();
        }
        
    }

    private void SpawnTile(int prefabIndex = -1)
    {
        GameObject go;
        //if(prefabIndex == -1)
            go = Instantiate (tilePrefabs[0]) as GameObject;
        
        //else
          //  go = Instantiate (tilePrefabs[prefabIndex]) as GameObject;

        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(go);
    }

    private void DeleteTile()
    {
        Destroy (activeTiles[0]);
        activeTiles.RemoveAt(0);

    }

    //private int RandomPrefabIndex()
   // {
    //    if(tilePrefabs.Length == 0)
      //      return 0;
      //  
     //   int randomIndex = lastPrefabIndex;
    //    while(randomIndex == lastPrefabIndex)
     //   { 
      //      randomIndex = Random.Range (0, tilePrefabs.Length);
     //   }
      ///  lastPrefabIndex = randomIndex;
        //return randomIndex;
    //}
}
