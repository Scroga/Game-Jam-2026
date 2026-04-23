using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalBootstrap
{
    private static void LoadPrefab<T>(string prefabName) where T : SmartSingleton<T>{
        if (Object.FindFirstObjectByType<T>() != null) return;

        var prefab = Resources.Load<GameObject>(prefabName);
        if (prefab == null)
        {
            Debug.LogError("GlobalManagers prefab not found at Resources/" + prefabName);
            return;
        }

        Object.Instantiate(prefab);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureGlobals()
    {
        LoadPrefab<SoundManager>("AudioManager");
        LoadPrefab<LevelManager>("LevelManager");
    }
}
