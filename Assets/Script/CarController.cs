using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;
using System;

public class CarController : MonoBehaviour
{
    [Header("ㅁSettings")]

    [FormerlySerializedAs("BaseSpeed")]
    [SerializeField]
    float BaseSpeed = 0.2f;

    [Header("ㅁDebuggings")]

    [SerializeField]
    private float _currentMovementSpeed;

    private float beforeSpeed;

    public float currentMovementSpeed
    {
        get
        {
            return this._currentMovementSpeed;
        }
        set
        {
            this._currentMovementSpeed = value;
            if(value != beforeSpeed)
            {
                CarMoveEvent.Invoke();
            }

            beforeSpeed = _currentMovementSpeed;
        }
    }

    private Action CarMoveEvent =  () => { };
    private Action CarStopEvent =  () => { };
    private Action CarActionEvent =  () => { };

    Vector2 MouseStartPos;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public bool isCarMoving;

    public void SetCarStopEvent(Action Event)
    {
        this.CarStopEvent = Event;
    }

    public void SetCarActionEvent(Action Event)
    {
        this.CarActionEvent = Event;
    }

    public void SetCarMoveEvent(Action Event)
    {
        this.CarMoveEvent = Event;
    }

    private void GetInput()
    {
        var mouse = Mouse.current;

        if(mouse != null )
        {
            if(mouse.leftButton.wasPressedThisFrame)
            {
                this.MouseStartPos = mouse.position.ReadValue();
            }
            else if(mouse.leftButton.wasReleasedThisFrame)
            {
                Vector2 endPos = mouse.position.ReadValue();

                float swipeLength = endPos.x - this.MouseStartPos.x;
                this.isCarMoving = true;

                this.currentMovementSpeed = swipeLength / 500f * this.BaseSpeed;
                CarActionEvent.Invoke();
            }
        }
    }

    private void DataUpdate()
    {
        this.currentMovementSpeed *= 0.98f;
    }

    private void MovementUpdate()
    {
        transform.Translate(this.currentMovementSpeed, 0f, 0f);
    }

    private void Update()
    {
        GetInput();
        DataUpdate();
        MovementUpdate();
    }

}