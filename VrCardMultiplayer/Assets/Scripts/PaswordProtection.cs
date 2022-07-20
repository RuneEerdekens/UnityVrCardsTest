using UnityEngine;
using Unity.Netcode;
using TMPro;
using System.Text;
using Unity.Netcode.Transports.UTP;

public class PaswordProtection : NetworkBehaviour
{

    [SerializeField] private GameObject PaswordUi;
    [SerializeField] private GameObject LeaveUi;
    [SerializeField] private GameObject HostButton;
    [SerializeField] private GameObject ClientButton;
    [SerializeField] private GameObject AdressUi;
    [SerializeField] private GameObject PortUi;
    [SerializeField] private GameObject StartUi;

    public UnityTransport transport;
    public MultiScenes SManager;
    [SerializeField] private TMP_InputField PaswordInputField;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    public override void OnDestroy()
    {
        if (NetworkManager.Singleton == null) { return; }

        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    }

    public void Host()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalChek;
        NetworkManager.Singleton.StartHost();
        StartUi.SetActive(true);
    }

    public void Client()
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(PaswordInputField.text);
        NetworkManager.Singleton.StartClient();
    }

    public void Leave()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalChek;
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
        }
        PaswordUi.SetActive(true);
        LeaveUi.SetActive(false);
        ClientButton.SetActive(true);
        HostButton.SetActive(true);
        AdressUi.SetActive(true);
        PortUi.SetActive(true);
        StartUi.SetActive(false);
    }


    private void HandleClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            PaswordUi.SetActive(false);
            LeaveUi.SetActive(true);
            ClientButton.SetActive(false);
            HostButton.SetActive(false);
            AdressUi.SetActive(false);
            PortUi.SetActive(false);
        }
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            PaswordUi.SetActive(true);
            LeaveUi.SetActive(false);
            ClientButton.SetActive(true);
            HostButton.SetActive(true);
            AdressUi.SetActive(true);
            PortUi.SetActive(false);
        }
    }

    private void ApprovalChek(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        var conectionData = request.Payload;
        var clientID = request.ClientNetworkId;

        string pasword = Encoding.ASCII.GetString(conectionData);

        bool Aproved = pasword == PaswordInputField.text;

        response.Approved = Aproved;

        response.CreatePlayerObject = true;
        response.PlayerPrefabHash = null;
        response.Position = new Vector3(2, 0, 0);
        response.Rotation = Quaternion.identity;
        response.Pending = false;
    }


    public void OnIPChanged(string Ip)
    {
        transport.ConnectionData.Address = Ip;
    }

    public void OnPortChangerd(string Port)
    {
        transport.ConnectionData.Port = ushort.Parse(Port);
    }

}

