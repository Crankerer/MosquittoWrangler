using System.Runtime.Remoting.Channels;

public class RowData
{
    public RowData(string time, string sender, string topic, string content)
    {
        this.time = time;
        this.sender = sender;
        this.topic = topic;
        this.content = content;
    }

    public string time;
    public string sender;
    public string topic;
    public string content;
}