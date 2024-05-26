using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopPanelViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textNickname;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private Slider sliderExperience;
    [SerializeField]
    private TextMeshProUGUI textHeart;
    [SerializeField]
    private TextMeshProUGUI textJewel;
    [SerializeField]
    private TextMeshProUGUI textGold;

    private void Awake()
    {
        BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameData);
    }

    public void UpdateNickname()
    {
        // 닉네임이 없으면 gamer_id를 출력하고, 닉네임이 있으면 닉네임 출력
        textNickname.text = UserInfo.Data.nickname == null ?
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;
    }

    public void UpdateGameData()
    {
        textLevel.text = $"{BackendGameData.Instance.UserGameData.level}";
        // 임시로 최대 경험치를 100으로 설정
        sliderExperience.value = BackendGameData.Instance.UserGameData.experience / 100;
        textHeart.text = $"{BackendGameData.Instance.UserGameData.heart} / 30";
        textJewel.text = $"{BackendGameData.Instance.UserGameData.jewel}";
        textGold.text = $"{BackendGameData.Instance.UserGameData.gold}";
    }
}

