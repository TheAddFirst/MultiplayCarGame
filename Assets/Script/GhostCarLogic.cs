using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GhostCarLogic : MonoBehaviour
{
    private float HighestScore;
    [FormerlySerializedAs("ㅁ HighestScoreFlag")]
    [SerializeField]
    private Transform HighestScoreFlag;
    
    [SerializeField]
    public float LerpSpeed = 0.1f;
    
    private void Update()
    {
        if (NetClient.instance != null)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, HighestScoreFlag.position - Vector3.right * NetClient.instance.GlobalBestScore, Time.deltaTime * LerpSpeed);
        }
    }
}
