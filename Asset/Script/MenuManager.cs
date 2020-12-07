using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject[] menu;
    
    public void OpenMenu(int index)
    {
        FindObjectOfType<AudioManager>().PlaySound("Click");
        Time.timeScale = 0;
        menu[index].SetActive(true);
    }
    public void CloseMenu(int index)
    {
        FindObjectOfType<AudioManager>().PlaySound("Click");
        Time.timeScale = 1;
        menu[index].SetActive(false);
    }
    public void LoadPlayScene(int level=0)
    {
        FindObjectOfType<AudioManager>().PlaySound("Click");
        Time.timeScale = 1;
        BotLogic.difficulty = level;
        SceneManager.LoadScene(1);
    }
    public void LoadMenuScene()
    {
        FindObjectOfType<AudioManager>().PlaySound("Click");
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        FindObjectOfType<AudioManager>().PlaySound("Click");
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
