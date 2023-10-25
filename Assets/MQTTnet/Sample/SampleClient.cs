using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MQTTnet;
using MQTTnet.Client;

public class SampleClient : MonoBehaviour
{    
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
        client = new MqttFactory().CreateMqttClient();
        client.Connected += OnConnected;
        client.Disconnected += OnDisconnected;
        client.ApplicationMessageReceived += OnApplicationMessageReceived;

        await ConnectAsync(ipAddress);

        StartCoroutine(SendMessage(0.1f));
    }

    IEnumerator SendMessage(float wait)
    {
        while (true)
        {
            PublishMessage();
            yield return new WaitForSeconds(wait);
        }
        yield return new WaitForSeconds(wait);
    }
    
    async void OnDestroy()
    {
        client.Connected -= OnConnected;
        client.Disconnected -= OnDisconnected;
        client.ApplicationMessageReceived -= OnApplicationMessageReceived;

        Debug.Log("start disconnect");
        await client.DisconnectAsync();
        Debug.Log("disconnected");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PublishMessage();
        }
    }

    public async void Connect(string address)
    {
        await ConnectAsync(address);
    }
    
    public async Task ConnectAsync(string address)
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(address, port)
            .Build();

        var result = await client.ConnectAsync(options);
        Debug.Log($"Connected to the broker: {result.IsSessionPresent}");

        var topic = new TopicFilterBuilder()
            .WithTopic("#")
            .Build();
        await client.SubscribeAsync("#");

        Debug.Log("Subscribed");
    }

    public async void PublishMessage()
    {
        await PublishMessageAsync();
    }

    public async Task PublishMessageAsync()
    {
        var msg = new MqttApplicationMessageBuilder()
                .WithTopic("/my/test/" + Random.Range(0,1000).ToString())
                .WithPayload(Random.Range(0,100).ToString())
                .WithExactlyOnceQoS()
                .Build();
        await client.PublishAsync(msg);
    }


    private void OnConnected(object sender, MqttClientConnectedEventArgs e)
    {
        Debug.Log($"On Connected: {e}");
    }

    private void OnDisconnected(object sender, MqttClientDisconnectedEventArgs e)
    {
        Debug.Log($"On Disconnected: {e}");
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
                messagesService.DistributeNewMessage(e.ApplicationMessage.Topic, e.ClientId, Encoding.UTF8.GetString(e.ApplicationMessage.Payload))
        );
    }
}

