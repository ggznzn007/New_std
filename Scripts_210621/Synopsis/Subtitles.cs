using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subtitles : MonoBehaviour
{
    public Text tx;
    private string m_text = "식물에게 행복을 전하자! 나의 감정을 보듬어 주면서 식물이 성장하는 힐링 게임! 평화롭던 어느 날 요정의 마을에 까칠이와 버럭이가 나쁜언어로 식물들을 시들게 하여 더 이상 성장하지 못하게 봉인 했다. 이들의 나쁜 마법들이 식물들의 성장을 방해하고 있다.우리의 따뜻한 관심과 마음으로 봉인을 해제하고 행복이와 식물들의 성장을 돕자!";
    
    void Start()
    {
        StartCoroutine(_typing());
    }

    IEnumerator _typing()
    {
        for(int i = 0; i <= m_text.Length; ++i)
        {
            tx.text = m_text.Substring(0, i);

            yield return new WaitForSeconds(0.15f);
        }
    }
}
