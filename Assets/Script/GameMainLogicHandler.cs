using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
public partial class GameMainLogicHandler
{
    [Header("Ealry Requirement")]

    [FormerlySerializedAs("DistanceUIPresenter")]
    [SerializeField]
    DistanceUIPresenter distanceUIPresenter;

    [FormerlySerializedAs("CarController")]
    [SerializeField]
    CarController carController;
    [FormerlySerializedAs("FlagObject")]
    [SerializeField]
    Transform flagObject;
}

public partial class GameMainLogicHandler : MonoBehaviour
{
    [Header("Debuggins")]

    [SerializeField]
    private float Distance;

    [SerializeField]
    private bool networkFlag;

    private void Start()
    {
        this.networkFlag = true;
        carController.SetCarMoveEvent(this.UpdateDistance);
    }

    private void Update()
    {
        UpdateNetWork();
    }

    private void UpdateNetWork()
    {
        if (!networkFlag)
            return;

        if(this.carController.isCarMoving && this.carController.currentMovementSpeed <= 0.0001)
        {
            UpdateDistance();
            float Distance = this.Distance;

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            byte[] buf = Encoding.UTF8.GetBytes(Distance.ToString());
            EndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 12345);

            clientSocket.SendTo(buf, serverEP);

            byte[] recvBytes = new byte[1024];
            int nRecv = clientSocket.ReceiveFrom(recvBytes, ref serverEP);
            string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);
            Debug.Log(txt);
            networkFlag = false;
        }
    }


    public void UpdateDistance()
    {
        Distance = flagObject.transform.position.x - carController.transform.position.x;
        distanceUIPresenter.SetDistanceTextAsFloat(Distance);
    }

}
