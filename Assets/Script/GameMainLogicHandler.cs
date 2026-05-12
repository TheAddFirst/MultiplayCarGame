using System;
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

    [FormerlySerializedAs("isScoreFlaged")] [FormerlySerializedAs("networkFlag")] [SerializeField]
    private bool isScoreFlagable;

    private float DefaultFlagDistance = 15f;
    
    private void Start()
    {
        this.isScoreFlagable = true;
        //NetClient.instance.RequestBestScore();
        carController.SetCarMoveEvent(this.UpdateDistance);
    }

    private void Update()
    {
        UpdateNetWork();
    }

    private void UpdateNetWork()
    {
        if (!isScoreFlagable)
            return;

        if(this.carController.isCarMoving && this.carController.currentMovementSpeed <= 0.0001)
        {
            Debug.Log("자동차가 멈췄다!");
            UpdateDistance();
            if (Distance > 0)
            {
                NetClient.instance.AddDistanceToServer(DefaultFlagDistance - Distance);
            }
            
            NetClient.instance.SendHighestScoreToServer(Distance);
            isScoreFlagable = false;
        }
    }


    public void UpdateDistance()
    {
        Distance = flagObject.transform.position.x - carController.transform.position.x;
        distanceUIPresenter.SetDistanceTextAsFloat(Distance);
        
    }

}
