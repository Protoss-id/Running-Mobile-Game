using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundControl : MonoBehaviour
{
    public CharacterControl myChar;
    public float speed;     // bisa buat gameobject arm1, gameobject arm2, untuk control masing2 object dalam satu skrip
    public GameObject[] obstaclePrefab;         // gameObject dan transform inherit each other
    public GameObject[] coinPrefab;
    public Transform obstaclePool;
    public Transform coinPool;
    public int obstacleSpawnCount;      // transform ada di dalam gameobject tp bisa bulak balik
    public int coinSpawnCount;

    public Transform backgroundPool;
    public int bgSpawnCount;
    // untuk referensi gameObject dan transform parent object dari script bisa langsung panggil gameObject atau transform for reference
    float lastZPos;

    // Start is called before the first frame update
    void Start()
    {
        lastZPos = transform.GetChild(transform.childCount - 1).position.z;
        //myChar = GetComponent<CharacterControl>();

        for (int i =0; i < obstaclePrefab.Length; i++)
        {
            for (int j = 0; j < obstacleSpawnCount; j++)
            {
                GameObject obstacle = Instantiate(obstaclePrefab[i], obstaclePool); // obstacle.transform.setparent =
                obstacle.SetActive(false);
            }
        }

        for (int i = 0; i < coinPrefab.Length; i++)
        {
            for (int j = 0; j < coinSpawnCount; j++)
            {
                GameObject coin = Instantiate(coinPrefab[i], coinPool);
                coin.SetActive(false);
            }
        }

        // generate background pool pada saat start game awal
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < bgSpawnCount; j++)
            {
                GameObject bg = Instantiate(transform.GetChild(i).gameObject, backgroundPool);
                bg.SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        //transform.position += Vector3.back;
        if (myChar.isRunning)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform tile = transform.GetChild(i);
                tile.position += Vector3.back * speed * Time.deltaTime; //* 

                if (tile.position.z < -4)
                {
                    ClearObject(tile);
                }

                if (tile.position.z <= -10)
                {
                    ClearObject(tile);
                    int randomBgIdx = Random.Range(0, backgroundPool.childCount);
                    Transform newTile = backgroundPool.GetChild(randomBgIdx);               //game object background di object pools

                    newTile.position = transform.GetChild(transform.childCount - 1).position + Vector3.forward * 10; // posisi global
                    newTile.parent = transform;                                             // set parent ke transform variable bawaan monobehaviour
                    newTile.SetAsLastSibling();

                    newTile.gameObject.SetActive(true);
                    tile.parent = backgroundPool;
                    tile.gameObject.SetActive(false);  
                    SpawnObject(newTile);
                }
            }
        }
        if (myChar.isRunningBackwards)
        {
            for(int i = transform.childCount-1; i >= 0; i--)
            {
                Transform tile = transform.GetChild(i);
                tile.position += Vector3.forward * speed * Time.deltaTime; //* 

                if (tile.position.z >= lastZPos)
                {
 
                    tile.position = transform.GetChild(0).position - new Vector3(0, 0, 10);
                    tile.SetAsFirstSibling();
                    ClearObject(tile);  
                }
            }
        }


    }

    void SpawnObject(Transform tile)
    {
        int obstacleChance = Random.Range(0, 100); //random.range result int
        int obstaclePos = Random.Range(-1, 2); // -1 sampai 1

        // chance random 50%

        if (obstacleChance < 50)
        {
            int randomObstacle = Random.Range(0, obstaclePool.childCount);
            Transform obstacle = obstaclePool.GetChild(randomObstacle);
            obstacle.SetParent(tile.GetChild(0));

            obstacle.localPosition = new Vector3(obstaclePos * myChar.pathSpacing, 0.27f, 0);
            obstacle.gameObject.SetActive(true);

        }

        // loop sejumlah jalur, jika jalur lebih dari 3 jalur maka bisa disesuaikan
        for (int i = -1; i<=1; i++)
        {
            int coinChance = Random.Range(0, 100);
            if (i == obstaclePos && obstacleChance < 50)
            {
                continue;
            }

            if(coinChance < 75)
            {
                int randomCoin = Random.Range(0, coinPool.childCount);
                Transform coin = coinPool.GetChild(randomCoin);
                coin.SetParent(tile.GetChild(1));   // tergantung urutan di bg
                coin.localPosition = new Vector3(i * myChar.pathSpacing, 0, 0);
                coin.gameObject.SetActive(true);
            }
        }
    }

    void ClearObject(Transform tile)
    {
        Transform obstacleTileParent = tile.GetChild(0);

        while (obstacleTileParent.childCount > 0)
        {
            Transform obstacle = obstacleTileParent.GetChild(0);
            obstacle.parent = obstaclePool;
            obstacle.gameObject.SetActive(false);
        }
        Transform coinTileParent = tile.GetChild(1);

        while (coinTileParent.childCount > 0)
        {
            Transform coin = coinTileParent.GetChild(0);
            coin.parent = coinPool;
            coin.gameObject.SetActive(false);
            for (int i = 0; i < coin.childCount; i++)
            {
                coin.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
