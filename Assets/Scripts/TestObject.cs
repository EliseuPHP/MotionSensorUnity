using Socket.Quobject.SocketIoClientDotNet.Client;
using UnityEngine;
using Newtonsoft.Json;
public class TestObject : MonoBehaviour
{
  private QSocket socket;
  public string serverURL = "http://192.168.0.50:3001";
  // Gyroscope Variables
  private dynamic gyro;
  private Vector3 rotateValue;
  private char[] delimiterChars = { ' ', ',', ':', '\t', '{', '}' };
  void Start()
  {
    DoOpen();
  }
  void Update()
  {
    rotateValue = new Vector3(gyro.x, gyro.y, gyro.z * -1);
    transform.eulerAngles = transform.eulerAngles - rotateValue;
  }
  void DoOpen()
  {
    if (socket == null)
    {
      Debug.Log("Client Iniciando");
      socket = IO.Socket(serverURL);

      socket.On(QSocket.EVENT_CONNECT, () =>
      {
        Debug.Log("Client Conectado");
        socket.Emit("message", "Unity3d Connected");
      });
      string recvData = "";
      socket.On(QSocket.EVENT_MESSAGE, (data) =>
      {
        recvData = data.ToString();
        dynamic gyro = JsonConvert.DeserializeObject(recvData);
        Debug.Log("Message Received: <x: " + gyro.x + " y: " + gyro.y + " z: " + gyro.z + ">");
      });
      socket.On(QSocket.EVENT_DISCONNECT, (data) =>
      {
        Debug.Log("Disconnect Info: " + data);
      });
    }
  }
  void DoClose()
  {
    if (socket != null)
    {
      socket.Disconnect();
      socket = null;
    }
  }
  void OnDestroy()
  {
    DoClose();
  }
}
