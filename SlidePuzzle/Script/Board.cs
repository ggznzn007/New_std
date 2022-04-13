using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab; // 숫자 타일 프리팹
    [SerializeField]
    private Transform tileParent; // 타일이 생성되는 보드 오브젝트의 트랜스폼    

    private List<Tile> tileList; // 생성된 타일 정보 저장

    private Vector2Int puzzleSize = new Vector2Int(4, 4); // 4*4 퍼즐
    private float neighborTileDistance = 102; // 인접한 타일사이의 거리, 별도 계산 가능

    public Vector3 EmptyTilePosition { set; get; }  // 빈 타일의 위치

    private IEnumerator Start()
    {
        tileList = new List<Tile>();

        SpawnTiles();

        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(tileParent.GetComponent<RectTransform>());

        // 현재 프레임이 종료될 때까지 대기
        yield return new WaitForEndOfFrame();

        // tileList 내에 모든 요소의 SetCorrectPosition() 메소드 호출 
        tileList.ForEach(x => x.SetCorrectPosition());

        StartCoroutine("OnSuffle");
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

        // 그리드레이아웃을 사용하여 자식의 위치를 바꾸는 것으로 설정해서
        // 타일리스트의 마지막에 있는 요소가 무조건 빈 타일
        EmptyTilePosition = tileList[tileList.Count - 1].GetComponent<RectTransform>().localPosition;
    }

    public void IsMoveTile(Tile tile)
    {// 빈 타일의 상하좌우 이웃에 위치한 타일만 거리가 102이기 때문에 빈 타일에 인접한 타일이면 빈 타일과 위치 전환
        if (Vector3.Distance(EmptyTilePosition, tile.GetComponent<RectTransform>().localPosition) == neighborTileDistance)
        {
            Vector3 goalPosition = EmptyTilePosition;

            EmptyTilePosition = tile.GetComponent<RectTransform>().localPosition;

            tile.OnMoveTo(goalPosition);
        }
    }
}
