using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIController : MonoBehaviour
{
    [SerializeField]
    private Button buttonStartServer;
    [SerializeField]
    private Button buttonShutDownServer;
    [SerializeField]
    private Button buttonConnectClient;
    [SerializeField]
    private Button buttonDisconnectClient;
    [SerializeField]
    private Button buttonSendMessage;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private TMP_InputField nameInputField;
    [SerializeField]
    private Button buttonNameSendMessage;
    [SerializeField]
    private TextField textField;
    [SerializeField]
    private Server server;
    [SerializeField]
    private Client client;
    private void Start()
    {
        buttonStartServer.onClick.AddListener(() => StartServer());
        buttonShutDownServer.onClick.AddListener(() => ShutDownServer());
        buttonConnectClient.onClick.AddListener(() => Connect());
        buttonDisconnectClient.onClick.AddListener(() => Disconnect());
        buttonSendMessage.onClick.AddListener(() => SendMessage());
        buttonNameSendMessage.onClick.AddListener(() => SendName());
        client.onMessageReceive += ReceiveMessage;
    }
    private void StartServer()
    {
        server.StartServer();
    }
    private void ShutDownServer()
    {
        server.ShutDownServer();
    }
    private void Connect()
    {
        client.Connect();
    }
    private void Disconnect()
    {
        client.Disconnect();
    }
    private void SendMessage()
    {
        Debug.Log("SendMessage ");

        client.SendMessage(inputField.text);
        inputField.text = "";
    }

    private void SendName()
    {
        client.SendName(inputField.text);
        inputField.text = "";
    }

    public void ReceiveMessage(object message)
    {
        textField.ReceiveMessage(message);
    }
}