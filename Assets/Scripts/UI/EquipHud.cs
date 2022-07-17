using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquipHud : MonoBehaviour
{
    private GameObject LImage;
    public List<GameObject> LImageList;

    private GameObject LIcon;
    public List<GameObject> LIconList;

    private GameObject RImage;
    public List<GameObject> RImageList;

    public GameObject RText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh(char e, int f)
    {
        LImage?.SetActive(false);
        LIcon?.SetActive(false);
        RImage?.SetActive(false);

        Debug.Log($"Equip {e}, friend {f}");

        switch (e)
        {
            case ' ':
                LImage = LImageList[0];
                break;
            case '+':
                LImage = LImageList[1];
                LIcon = LIconList[0];
                LIcon.SetActive(true);
                break;
            case '-':
                LImage = LImageList[2];
                LIcon = LIconList[1];
                LIcon.SetActive(true);
                break;
            case '*':
                LImage = LImageList[3];
                LIcon = LIconList[2];
                LIcon.SetActive(true);
                break;
            case '/':
                LImage = LImageList[4];
                LIcon = LIconList[3];
                LIcon.SetActive(true);
                break;
        }

        if (e == ' ') 
        {
            RImage = RImageList[10]; // ban
            RText.SetActive(false);
        }
        else
        {
            RImage = RImageList[f];
            RText.GetComponent<TMP_Text>().text = f == 0 ? "?" : f.ToString();
            RText.SetActive(true);
        }

        LImage.SetActive(true);
        RImage.SetActive(true);


    }
}
