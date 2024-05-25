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
			// Alpha 값을 1에서 0으로 : Fade Out
			yield return StartCoroutine(OnFade(1, 0));

			// Alpha 값을 0에서 1로 : Fade In
			yield return StartCoroutine(OnFade(0, 1));
		}
	}

	private IEnumerator OnFade(float start, float end)
	{
		float current = 0;
		float percent = 0;

		// fadeTime 시간동안 while() 반복문 실행
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

