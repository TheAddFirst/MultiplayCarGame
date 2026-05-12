using TMPro;
using UnityEngine;

public class ServerUIPresentManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI HighScoreText;
    [SerializeField] private TextMeshProUGUI TotalPlayerCount;
    [SerializeField] private TextMeshProUGUI TotalPlayerTravledDistance;
    
    void Update()
    {
        // 매 프레임 NetClient에 저장된 최신 점수를 가져와 UI에 뿌려줍니다.
        // Update는 메인 스레드라 절대 에러가 나지 않습니다.
        if (NetClient.instance != null)
        {
            float currentBest = NetClient.instance.GlobalBestScore;

            if (currentBest > 900)
            {
                HighScoreText.text = "Become New World Record!";
            }
            else
            {
                HighScoreText.text = "World Record: " + currentBest.ToString("F3")  + "M";
            }
            TotalPlayerCount.text = "Online Player: " + NetClient.instance.ServerPlayerCount.ToString();
            TotalPlayerTravledDistance.text = "Total Server Travled Distance:" + NetClient.instance.TotalDistance.ToString("F3");
        }
    }
}