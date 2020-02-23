using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject GameOverImage;
    public GameObject WinImage;
    public GameObject RetryButton;

    private void Start()
    {
        FindObjectOfType<EventSystem>().SetSelectedGameObject(RetryButton);
    }

    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    public void Quit()
    {
		SceneManager.LoadScene ("StartScene");
	}
}
