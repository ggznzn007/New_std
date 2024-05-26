using UnityEngine;
using BackEnd;		// �ڳ� SDK

public class BackendManager : MonoBehaviour
{
    private void Awake()
    {
        // Update() �޼ҵ��� Backend.AsyncPoll(); ȣ���� ���� ������Ʈ�� �ı����� �ʴ´�
        DontDestroyOnLoad(gameObject);

        // �ڳ� ���� �ʱ�ȭ
        BackendSetup();
    }

    private void Update()
    {
        // ������ �񵿱� �޼ҵ� ȣ��(�ݹ� �Լ� Ǯ��)�� ���� �ۼ�
        // ���� : https://developer.thebackend.io/unity3d/guide/Async/AsyncFuncPoll/
        if (Backend.IsInitialized)
        {
            Backend.AsyncPoll();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif

        }
    }

    private void BackendSetup()
    {
        // �ڳ� �ʱ�ȭ (�ݹ� �Լ� Ǯ���� ����Ϸ��� �Ű������� true�� ����)
        var bro = Backend.Initialize(true);

        // �ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            // �ʱ�ȭ ���� �� statusCode 204 Success
            Debug.Log($"�ʱ�ȭ ���� : {bro}");
        }
        else
        {
            // �ʱ�ȭ ���� �� statusCode 400�� ���� �߻�
            Debug.LogError($"�ʱ�ȭ ���� : {bro}");
        }
    }
}

