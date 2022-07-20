using Unity.Netcode;
using UnityEngine;

public class PreLoader : MonoBehaviour
{
    public NetworkManager network;

    private void Awake()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            Debug.Log("ARG " + i + ": " + args[i]);
            if (args[i] == "-Start-Server")
            {
                StartServer();
            }
        }
    }
    public void StartHost()
    {
        network.StartHost();
    }

    public void StartServer()
    {
        network.StartServer();
    }

    public void StartClient()
    {
        network.StartClient();
    }
}
