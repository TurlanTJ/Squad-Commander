using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitBTN : MonoBehaviour
{
    public void QuitGame()
    {
        LevelManager.instance.QuitGame();
    }
}
