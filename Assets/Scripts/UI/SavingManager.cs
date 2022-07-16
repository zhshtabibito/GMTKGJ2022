using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingManager : MonoBehaviour
{
    public static SavingManager Instance;
    public static int Chapter;

    private void Awake()
    {
        Instance = this;
        Chapter = LoadGame();
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
        if(CurrentChapter > Chapter)
        {
            Chapter = CurrentChapter;
            PlayerPrefs.SetInt("Chapter", CurrentChapter);
        }
    }

    private int LoadGame()
    {
        return PlayerPrefs.GetInt("Chapter", 0);
    }

}
