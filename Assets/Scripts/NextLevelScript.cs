using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelScript : MonoBehaviour
{
    public void NextLevel() {
        LevelManager.Instance.LoadNextLevel();
    }

    public void Menu() {
        LevelManager.Instance.LoadScene("Menu", "CrossFade");
    }
}
