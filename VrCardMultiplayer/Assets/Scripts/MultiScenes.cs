using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MultiScenes : NetworkBehaviour
{

    private string m_SceneName = "Scenes/SampleScene";
    private Scene m_LoadedScene;

    public bool SceneIsLoaded
    {
        get
        {
            if (m_LoadedScene.IsValid() && m_LoadedScene.isLoaded)
            {
                return true;
            }
            return false;
        }
    }

    public void StartGame()
    {
        if (IsServer && !string.IsNullOrEmpty(m_SceneName))
        {
            NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
            var status = NetworkManager.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);
            CheckStatus(status);
        }

        base.OnNetworkSpawn();
    }

    private void CheckStatus(SceneEventProgressStatus status, bool isLoading = true)
    {
        var sceneEventAction = isLoading ? "load" : "unload";
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to {sceneEventAction} {m_SceneName} with" +
                $" a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }

    /// <summary>
    /// Handles processing notifications when subscribed to OnSceneEvent
    /// </summary>
    /// <param name="sceneEvent">class that contains information about the scene event</param>
    private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        var clientOrServer = sceneEvent.ClientId == NetworkManager.ServerClientId ? "server" : "client";
        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadComplete:
                {
                    // We want to handle this for only the server-side
                    if (sceneEvent.ClientId == NetworkManager.ServerClientId)
                    {
                        // *** IMPORTANT ***
                        // Keep track of the loaded scene, you need this to unload it
                        m_LoadedScene = sceneEvent.Scene;
                    }
                    Debug.Log($"Loaded the {sceneEvent.SceneName} scene on " +
                        $"{clientOrServer}-({sceneEvent.ClientId}).");
                    break;
                }
            case SceneEventType.UnloadComplete:
                {
                    Debug.Log($"Unloaded the {sceneEvent.SceneName} scene on " +
                        $"{clientOrServer}-({sceneEvent.ClientId}).");
                    break;
                }
            case SceneEventType.LoadEventCompleted:
            case SceneEventType.UnloadEventCompleted:
                {
                    var loadUnload = sceneEvent.SceneEventType == SceneEventType.LoadEventCompleted ? "Load" : "Unload";
                    Debug.Log($"{loadUnload} event completed for the following client " +
                        $"identifiers:({sceneEvent.ClientsThatCompleted})");
                    if (sceneEvent.ClientsThatTimedOut.Count > 0)
                    {
                        Debug.LogWarning($"{loadUnload} event timed out for the following client " +
                            $"identifiers:({sceneEvent.ClientsThatTimedOut})");
                    }
                    break;
                }
        }
    }

    public override void OnNetworkDespawn()
    {
        // Assure only the server calls this when the NetworkObject is
        // spawned and the scene is loaded.
        if (!IsServer || !IsSpawned || !m_LoadedScene.IsValid() || !m_LoadedScene.isLoaded)
        {
            return;
        }

        // Unload the scene
        if (IsServer)
        {
            var status = NetworkManager.SceneManager.LoadScene("Scenes/StartUp", LoadSceneMode.Single);
            CheckStatus(status, false);
        }
        base.OnNetworkDespawn();
    }
}
