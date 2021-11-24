using Socket.Quobject.SocketIoClientDotNet.Client;
using UnityEngine;
using Newtonsoft.Json;
public class RMotionControl : MonoBehaviour
{
  private QSocket socket;
  public string serverURL = "http://192.168.0.50:3001";
  // Gyroscope Variables
  private dynamic gyro;
  private float x, y, z;
  private string rotation = "270";
  private Vector3 rotateValue;
  void Start()
  {
    DoOpen();
  }
  void Update()
  {
    if (rotation == "0")
    {
      rotateValue = new Vector3(x, y, z * -1);
    }
    if (rotation == "90")
    {
      rotateValue = new Vector3(y, x * -1, z);
    }
    if (rotation == "180")
    {
      rotateValue = new Vector3(x* -1, y* -1, z);
    }
    if (rotation == "270")
    {
      rotateValue = new Vector3(y * -1, x, z * -1);
    }
    
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
        gyro = JsonConvert.DeserializeObject(recvData);
        x = gyro.x;
        y = gyro.y;
        z = gyro.z;
        rotation = gyro.phoneRotation;
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
