using System.Collections;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
	private	SpriteRenderer	spriteRenderer;
	private	float			fadeTime = 0.1f;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		StartCoroutine(nameof(TwinkleLoop));
	}

	private IEnumerator TwinkleLoop()
	{
		while ( true )
		{
			// Alpha ���� 1���� 0���� : Fade Out
			yield return StartCoroutine(OnFade(1, 0));

			// Alpha ���� 0���� 1�� : Fade In
			yield return StartCoroutine(OnFade(0, 1));
		}
	}

	private IEnumerator OnFade(float start, float end)
	{
		float current = 0;
		float percent = 0;

		// fadeTime �ð����� while() �ݺ��� ����
		while ( percent < 1 )
		{
			current += Time.deltaTime;
			percent = current / fadeTime;

			Color color = spriteRenderer.color;
			color.a = Mathf.Lerp(start, end, percent);
			spriteRenderer.color = color;

			yield return null;
		}
	}
}

