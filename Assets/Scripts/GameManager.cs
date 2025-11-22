using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
   
    void Awake()
    {
        Instance = this;
    }

    public int LevelNo { get; set; }
    public List<GameObject> LevelObjs;
    public GameObject Player;
    public CameraFollow Cam;
    public LevelManager activeLevelManager { get; set; }

    public Action LevelComplete, LevelFail;
    public void LoadLevel()
    {
        LevelNo = PlayerPrefs.GetInt("Level", 0);
        GameObject level = Instantiate(LevelObjs[LevelNo], Vector3.zero, Quaternion.identity);
        activeLevelManager = level.GetComponent<LevelManager>();
        activeLevelManager.SetUpLevel(Player, Cam);
    }

    public bool LoadNextLevelIfAvailable()
    {
        LevelNo++;
        if (LevelNo < LevelObjs.Count)
        {
            LoadLevel();
        }
        else
        {
            LevelNo = 0;
            return false;
        }

        return true;
    }

    public void DeleteLevel()
    {
        Destroy(activeLevelManager.gameObject, 0.4f);
    }

}
