﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
    public void loadScene1()
    {
        SceneManager.LoadScene(1);
    }
    public void loadScene2()
    {
        SceneManager.LoadScene(2);
    }
    public void loadScene3()
    {
        SceneManager.LoadScene(3);
    }
    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
