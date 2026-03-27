using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class DistanceUIPresenter : MonoBehaviour
{
    [FormerlySerializedAs("DistanceUIPresenter")]
    [SerializeField]
    private TextMeshProUGUI DistanceUIText;


    public void SetDistanceTextAsFloat(float value)
    {
        if(value > 0)
        {
            this.DistanceUIText.text = "Distance :" + value.ToString("F2") + "m";
        }
        else
        {
            this.DistanceUIText.text = "Game Over";
        }
    }

    //public void SetDistanceTextAsString(string text)
    //{
    //    this.DistanceUIText.text = "Distance :" + text + "m";
    //}

}
