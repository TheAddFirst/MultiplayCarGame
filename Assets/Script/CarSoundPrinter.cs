using UnityEngine;
using UnityEngine.InputSystem;

using System.Net;
using System.Net.Sockets;

[RequireComponent(typeof(AudioSource))]

public class CarSoundPrinter : MonoBehaviour
{
    AudioSource Audio;
    CarController myCarController;

    private void Start()
    {
        this.Audio = GetComponent<AudioSource>();

        if(TryGetComponent<CarController>(out myCarController))
        {
            myCarController.SetCarActionEvent(this.PrintCarSound);
        }
        else
        {
            Debug.LogError("Failed To Find Controlling Identity");
            Destroy(this);
        }

    }

    public void PrintCarSound()
    {
        Debug.Log("Printing Audio");
        Audio.Play();
    }
}
