using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : SmartSingleton<LevelManager>
{
    public Slider progressBar;
    public GameObject transitionsContainer;
    public SceneTransition[] transitions;

    private List<string> levels = new(){ 
        "Level0",
        "Level1",
        "Level2",
        "Level3",
    };
    private int currentLevel = 0;

    private void Start()
    {
        transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>();
    }
    private bool IsValidLevelIndex(int index)
    {
        return index >= 0 && index < levels.Count;
    }

    public void LoadNextLevel()
    {
        int nextLevel = currentLevel + 1;

        if (!IsValidLevelIndex(nextLevel))
        {
            Debug.Log("No next level");
            return;
        }

        currentLevel = nextLevel;
        LoadScene(levels[currentLevel], "CrossFade");
    }

    public void OnDeath()
    {
        if (!IsValidLevelIndex(currentLevel))
        { 
            Debug.LogError($"Invalid currentLevel index: {currentLevel}");
            return;
        }
        MusicManager.Instance.PlayMusic("Game", 0.5f);
        LoadScene(levels[currentLevel], "YouDied");
    }

    public void LoadScene(string sceneName, string transitionName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, transitionName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
    {
        SceneTransition transition = transitions.First(t => t.name == transitionName);

        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        yield return transition.AnimateTransitionIn();

        //progressBar.gameObject.SetActive(true);

        do
        {
            progressBar.value = scene.progress;
            yield return null;
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        //progressBar.gameObject.SetActive(false);

        yield return transition.AnimateTransitionOut();

    }
}
