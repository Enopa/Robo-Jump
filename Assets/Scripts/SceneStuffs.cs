using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStuffs : MonoBehaviour
{
    public bool[] unlockedLevels;
    public GameObject[] lockedPanels;
    public GameObject startMenu;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString(0.ToString(), "true");
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            for (int i = 0; i < lockedPanels.Length; i++)
            {
                if (PlayerPrefs.GetString(i.ToString()) != unlockedLevels[i].ToString())
                {
                    unlockedLevels[i] = PlayerPrefs.GetString(i.ToString()) == "true";
                    
                } else
                {
                    PlayerPrefs.SetString(i.ToString(), unlockedLevels[i].ToString());
                }

            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (!startMenu.activeSelf)
            {
                for (int i = 0; i < lockedPanels.Length; i++)
                {
                    lockedPanels[i].SetActive(!unlockedLevels[i]);
                }
            }
        }
    }


    public void SceneLoad(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }


    public void reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
