using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallLeafMove : LeafMove
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Finish"))
        {
            StartCoroutine(DelayPos());
        }
    }

    IEnumerator DelayPos()
    {
        yield return new WaitForSecondsRealtime(10);                   // (10) 10.5�ʸ� ���ϸ� (10.5f) ���� �� ����Ʈ 10�� 
        transform.position = startPos;                                 // ���� ��ġ���� ó����ġ������ �ʱ�ȭ
        transform.rotation = startRot;                                 // ���� ȸ������ ó��ȸ�������� �ʱ�ȭ
        transform.localScale = new Vector3(0, 0, 0);                   // ���� ������(ũ��)�� 0,0,0���� ����
        yield return new WaitForSecondsRealtime(1);                    // 1���� �����̸� ����
        transform.LeanScale(new Vector3(1, 1, 1), 2).setEaseLinear();  // ���� �ʱ�ũ�Ⱑ 1.5f�� 1.5f�� �����ϰ� �ڿ� 2�� Ŀ���� �ð�
                                                                       // ex) transform.LeanScale(new Vector3(1,1,1), 1.5f).setEaseLinear();
                                                                       // ���� ���ô� ũ�Ⱑ 1 Ŀ���� �ð��� 1.5�� 
        transform.position = startPos;                                 // �ٽ� ���� ��ġ�� �ʱ�ȭ
        transform.rotation = startRot;                                 // �ٽ� ���� ȸ���� �ʱ�ȭ
    }
}
