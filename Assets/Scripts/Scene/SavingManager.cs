using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingManager : MonoBehaviour
{
    public static SavingManager Instance;
    public static int Level;

    private void Awake()
    {
        Instance = this;
        Level = LoadGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveGame(int CurrentChapter)
    {
        if(CurrentChapter > Level)
        {
            Level = CurrentChapter;
            PlayerPrefs.SetInt("Chapter", CurrentChapter);
        }
    }

    private int LoadGame()
    {
        // return 8;
        return PlayerPrefs.GetInt("Chapter", 0);
    }

}
