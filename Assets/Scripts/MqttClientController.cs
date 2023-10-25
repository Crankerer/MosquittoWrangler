using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MQTTnet;
using MQTTnet.Client;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MqttClientController : MonoBehaviour
{
    [SerializeField]
    private ConnectionService connectionService;
    
    [SerializeField]
    int NumberOfReceivedMessages = 0;
    
    [SerializeField]
    private MessagesService messagesService;
    
    [SerializeField]
    string ipAddress = "";
    
    [SerializeField]
    int port = 1883;

    IMqttClient client;
    StringBuilder sb = new StringBuilder();

    async void Start()
    {
        connectionService.Disconnected();
        
        client = new MqttFactory().CreateMqttClient();
        client.Connected += OnConnected;
        client.Disconnected += OnDisconnected;
        client.ApplicationMessageReceived += OnApplicationMessageReceived;
        messagesService.SendMqttMsg += OnSendMqttMsg;
    }

    private void OnSendMqttMsg(RowData data)
    {
        SendMqttMessage(data.topic, data.content);
    }

    async void OnDestroy()
    {
        client.Connected -= OnConnected;
        client.Disconnected -= OnDisconnected;
        client.ApplicationMessageReceived -= OnApplicationMessageReceived;

        Disconnect();
    }

    public void ConnectDisconnect()
    {
        if (connectionService.IsConnected())
            Disconnect();
        else
            Connect();
    }

    public async void Disconnect()
    {
        Debug.Log("start disconnect");
        await client.DisconnectAsync();
        Debug.Log("disconnected");
    }

    public async void Connect()
    {
        Debug.Log(connectionService.GetCurrentIp());
        if (connectionService.GetCurrentIp() == "")
        {
            Debug.Log("can not connect, error in address");
            return;
        }

        Debug.Log("try connect to " + connectionService.GetCurrentIp());
        connectionService.Connecting();
        await ConnectAsync(connectionService.GetCurrentIp());
    }
    
    public async Task ConnectAsync(string address)
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(address, port)
            .WithClientId("Mosquitto Wrangler")
            .Build();

        var result = await client.ConnectAsync(options);
        Debug.Log($"Connected to the broker: {result.IsSessionPresent}");

        var topic = new TopicFilterBuilder()
            .WithTopic("#")
            .Build();
        await client.SubscribeAsync("#");

        Debug.Log("Subscribed to all messages");
    }

    private void OnConnected(object sender, MqttClientConnectedEventArgs e)
    {
        Debug.Log($"On Connected: {e}");
        connectionService.Connected();
    }

    private void OnDisconnected(object sender, MqttClientDisconnectedEventArgs e)
    {
        Debug.Log($"On Disconnected: {e}");
        connectionService.Disconnected();
    }

    private void OnApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
    {
        NumberOfReceivedMessages++;
        
        sb.Clear();
        sb.AppendLine("Message:");
        sb.AppendFormat("ClientID: {0}\n", e.ClientId);
        sb.AppendFormat("Topic: {0}\n", e.ApplicationMessage.Topic);
        sb.AppendFormat("Payload: {0}\n", Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
        sb.AppendFormat("QoS: {0}\n", e.ApplicationMessage.QualityOfServiceLevel);
        sb.AppendFormat("Retain: {0}\n", e.ApplicationMessage.Retain);

        //Debug.Log(sb);

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
                messagesService.DistributeNewMessage(e.ClientId, e.ApplicationMessage.Topic,Encoding.UTF8.GetString(e.ApplicationMessage.Payload))
        );
    }
    
    public async void SendMqttMessage(string topic, string payload)
    {
        await PublishMessageAsync(topic, payload);
    }
    
    private async Task PublishMessageAsync(string topic, string payload)
    {
        var msg = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithExactlyOnceQoS()
            .Build();
        await client.PublishAsync(msg);
    }
}