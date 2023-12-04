using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
//Задание 1.Добавьте поле ввода, куда игрок сможет ввести своё имя. Сделайте так,
//чтобы сразу после подключения к серверу клиент отправлял имя на сервер.
//Задание 2. Создайте на сервере таблицу соответствия идентификаторов клиентов
//и их имён. Первое сообщение от клиента пусть записывается в качестве его имени.
//Внесите изменения, чтобы последующие сообщения пересылались с именем
//игрока, а не его номером.
//Задание 3*. Создайте в клиенте систему из разных типов сообщений для передачи
//текста и имени игрока. Создайте на сервере метод для парсинга сообщений
//(подробно разберём это на следующем уроке).

public class Server : MonoBehaviour
{
    private const int MAX_CONNECTION = 10;
    private int port = 5805;
    private int hostID;
    private int reliableChannel;
    private bool isStarted = false;
    private byte error;
    List<int> connectionIDs = new List<int>();
    Dictionary<int, string> connectionIDNames = new Dictionary<int, string>();

    public void StartServer()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);
        HostTopology topology = new HostTopology(cc, MAX_CONNECTION);
        hostID = NetworkTransport.AddHost(topology, port);
        
        isStarted = true;
    }

    public void ShutDownServer()
    {
        if (!isStarted) return;
        NetworkTransport.RemoveHost(hostID);
        NetworkTransport.Shutdown();
        isStarted = false;
    }

    public void SendMessage(string message, int connectionID)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostID, connectionID, reliableChannel, buffer, message.Length *
        sizeof(char), out error);
        if ((NetworkError)error != NetworkError.Ok) Debug.Log((NetworkError)error);
    }

    public void SendMessageToAll(string message)
    {
        for (int i = 0; i < connectionIDs.Count; i++)
        {
            SendMessage(message, connectionIDs[i]);
        }
    }

    void Update()
    {
        if (!isStarted) return;
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out
        channelId, recBuffer, bufferSize, out dataSize, out error);
        while (recData != NetworkEventType.Nothing)
        {
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    break;
                case NetworkEventType.ConnectEvent:
                    connectionIDs.Add(connectionId);
                    string login = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    SendMessageToAll($"Player {connectionId} has connected.");
                    if (!connectionIDNames.ContainsKey(connectionId))
                    {
                        connectionIDNames.Add(connectionId, login);
                    } 
                    Debug.Log($"Player {connectionId} has connected.");
                    break;
                case NetworkEventType.DataEvent:
                    string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    Debug.Log($"Player {connectionId} : {message}");
                    SendMessageToAll($"Player { connectionId}: { message}");
                    break;
                case NetworkEventType.DisconnectEvent:
                    connectionIDs.Remove(connectionId);
                    SendMessageToAll($"Player {connectionId} {connectionIDNames.Values} has disconnected.");
                    Debug.Log($"Player {connectionId} {connectionIDNames.Values} has disconnected.");
                    break;
                case NetworkEventType.BroadcastEvent:
                    break;
            }
            recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
            bufferSize, out dataSize, out error);
        }
    }

}