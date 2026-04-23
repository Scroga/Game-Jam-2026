using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class GameTest : MonoBehaviour
{
    public void GoToMenu() {
        LevelManager.Instance.LoadScene("Menu", "CrossFade");
    }
}
