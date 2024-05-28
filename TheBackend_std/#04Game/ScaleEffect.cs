using System.Collections;
using UnityEngine;
using TMPro;

public class ScaleEffect : MonoBehaviour
{
	[SerializeField]
	[Range(0.01f, 10f)]
	private	float			effectTime;		// 크기 확대/축소 되는 시간
	private	TextMeshProUGUI	effectText;     // 크기 확대/축소 효과에 사용되는 텍스트

	private void Awake()
	{
		effectText = GetComponent<TextMeshProUGUI>();
	}

	public void Play(float start, float end)
	{
		StartCoroutine(Process(start, end));
	}

	private IEnumerator Process(float start, float end)
	{
		float current = 0;
		float percent = 0;

		while ( percent < 1 )
		{
			current += Time.deltaTime;
			percent = current / effectTime;

			effectText.fontSize = Mathf.Lerp(start, end, percent);

			yield return null;
		}
	}
}

