using UnityEngine;
using TMPro;

public class GuildPage : MonoBehaviour
{
    [SerializeField]
    private BackendGuildSystem backendGuildSystem;
    [SerializeField]
    private TextMeshProUGUI textGuildName;              // Popup ��ܿ� ��µǴ� ��� �̸� Text UI
    [SerializeField]
    private Notice notice;
    [SerializeField]
    private GameObject executivesOption;
    [SerializeField]
    private TextMeshProUGUI textMemberCount;

    [SerializeField]
    private GameObject memberPrefab;
    [SerializeField]
    private Transform parentContent;

    private string guildName = string.Empty;    // ��� �̸�
    private MemoryPool memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool(memberPrefab, parentContent);
    }

    public void Setup(string guildName, bool isMaster = false, bool isOtherGuild = false)
    {
        notice.Setup(isMaster, isOtherGuild);
        executivesOption.SetActive(isMaster);

        gameObject.SetActive(true);

        textGuildName.text = guildName;
        this.guildName = guildName;

        if (isOtherGuild == true)
        {
            textMemberCount.text = $"��� �ο� {backendGuildSystem.otherGuildData.memberCount}/100";
            backendGuildSystem.GetGuildMemeberList(backendGuildSystem.otherGuildData.guildInDate);
        }
        else
        {
            textMemberCount.text = $"��� �ο� {backendGuildSystem.myGuildData.memberCount}/100";
            backendGuildSystem.GetGuildMemeberList(backendGuildSystem.myGuildData.guildInDate);
        }
    }

    public void Activate(GuildMemberData member)
    {
        GameObject item = memoryPool.ActivatePoolItem();
        item.GetComponent<GuildMember>().Setup(member);
    }

    public void Deactivate(GameObject member)
    {
        memoryPool.DeactivatePoolItem(member);
    }

    public void DeactivateAll()
    {
        memoryPool.DeactivateAllPoolItems();
    }

    public void OnClickApplyGuild()
    {
        backendGuildSystem.ApplyGuild(guildName);
    }
}

