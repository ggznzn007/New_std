using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;                        // ���� Ÿ�� ������
    [SerializeField]
    private Transform tileParent;                         // Ÿ���� �����Ǵ� ���� ������Ʈ�� Ʈ������    

    private List<Tile> tileList;                          // ������ Ÿ�� ���� ����

    private Vector2Int puzzleSize = new Vector2Int(4, 4); // 4*4 ����
    private float neighborTileDistance = 102;             // ������ Ÿ�ϻ����� �Ÿ�, ���� ��� ����

    public Vector3 EmptyTilePosition { set; get; }        // �� Ÿ���� ��ġ
    public int Playtime { private set; get; } = 0;        // ���� �÷��� �ð�
    public int MoveCount { private set; get; } = 0;       // Ÿ�� �̵� Ƚ��

    private IEnumerator Start()
    {      
        tileList = new List<Tile>();

        SpawnTiles();

        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(tileParent.GetComponent<RectTransform>());

        // ���� �������� ����� ������ ���
        yield return new WaitForEndOfFrame();

        // tileList ���� ��� ����� SetCorrectPosition() �޼ҵ� ȣ�� 
        tileList.ForEach(x => x.SetCorrectPosition());

        StartCoroutine("OnSuffle");

        // ���� ���۰� ���ÿ� �÷��̽ð� ����
        StartCoroutine("CalculatePlaytime");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            // �ȵ���̵�
#else
Application.Quit();

#endif
        }
    }

    private void SpawnTiles()
    {
        for (int y = 0; y < puzzleSize.y; ++y)
        {
            for (int x = 0; x < puzzleSize.x; ++x)
            {
                GameObject clone = Instantiate(tilePrefab, tileParent);
                
                Tile tile = clone.GetComponent<Tile>();              

                tile.Setup(this, puzzleSize.x * puzzleSize.y, y * puzzleSize.x + x + 1);

                tileList.Add(tile);
            }
        }
    }

    private IEnumerator OnSuffle()
    {
        SE_Manager.instance.Playsound(SE_Manager.instance.shuffle);
        float current = 0;
        float percent = 0;
        float time = 1.5f;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            int index = Random.Range(0, puzzleSize.x * puzzleSize.y);
            tileList[index].transform.SetAsLastSibling();

            yield return null;
        }

        // �׸��巹�̾ƿ��� ����Ͽ� �ڽ��� ��ġ�� �ٲٴ� ������ �����ؼ�
        // Ÿ�ϸ���Ʈ�� �������� �ִ� ��Ұ� ������ �� Ÿ��
        EmptyTilePosition = tileList[tileList.Count - 1].GetComponent<RectTransform>().localPosition;
    }

    public void IsMoveTile(Tile tile)
    {
        SE_Manager.instance.Playsound(SE_Manager.instance.btn);
        // �� Ÿ���� �����¿� �̿��� ��ġ�� Ÿ�ϸ� �Ÿ��� 102�̱� ������ �� Ÿ�Ͽ� ������ Ÿ���̸� �� Ÿ�ϰ� ��ġ ��ȯ
        if (Vector3.Distance(EmptyTilePosition, tile.GetComponent<RectTransform>().localPosition) == neighborTileDistance)
        {
            Vector3 goalPosition = EmptyTilePosition;

            EmptyTilePosition = tile.GetComponent<RectTransform>().localPosition;

            tile.OnMoveTo(goalPosition);

            // Ÿ���� �̵��� �� ���� �̵� Ƚ�� ����
            MoveCount++;
        }
    }

    public void IsGameOver()
    {
        List<Tile> tiles = tileList.FindAll(x => x.IsCorrected == true);

        Debug.Log("Correct Count : " + tiles.Count);
        if(tiles.Count == puzzleSize.x*puzzleSize.y-1)
        {
            Debug.Log("GameClear");
            SE_Manager.instance.Playsound(SE_Manager.instance.gameClear);
            // ���� Ŭ���� �� �ð����� ����
            StopCoroutine("CalculatePlaytime");

            // ���� ������Ʈ�� ������Ʈ�� �����ϱ� ������ 
            // �ѹ��� ȣ���ϱ� ������ ������ ������ �ʰ� �ٷ� ȣ��
            GetComponent<UIController>().OnResultPanel();
        }
    }

    private IEnumerator CalculatePlaytime()
    {
        while(true)
        {
            Playtime++;

            yield return new WaitForSeconds(1f);
        }
    }
}
