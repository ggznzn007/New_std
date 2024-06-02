using UnityEngine;

public class GuildApplicantsPage : MonoBehaviour
{
	[SerializeField]
	private	BackendGuildSystem	backendGuildSystem;
	[SerializeField]
	private	GameObject			applicantPrefab;
	[SerializeField]
	private	Transform			parentContent;
	[SerializeField]
	private	GameObject			textSystem;			// �ش� �������� ������� �� ����ϴ� Text UI

	private	MemoryPool			memoryPool;

	private void Awake()
	{
		memoryPool = new MemoryPool(applicantPrefab, parentContent);
	}

	private void OnEnable()
	{
		backendGuildSystem.GetApplicants();
	}

	private void OnDisable()
	{
		DeactivateAll();
	}

	public void Activate(GuildMemberData applicant)
	{
		if ( textSystem.activeSelf ) textSystem.SetActive(false);

		GameObject item = memoryPool.ActivatePoolItem();
		item.GetComponent<GuildApplicant>().Setup(backendGuildSystem, this, applicant);
	}

	public void Deactivate(GameObject applicant)
	{
		memoryPool.DeactivatePoolItem(applicant);

		if ( memoryPool.ActiveCount == 0 )
		{
			textSystem.SetActive(true);
		}
	}

	public void DeactivateAll()
	{
		textSystem.SetActive(true);

		memoryPool.DeactivateAllPoolItems();
	}
}

