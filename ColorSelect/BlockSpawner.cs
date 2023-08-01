using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField]
    private Block blockPrefab;         // 블록 프리팹
    [SerializeField]
    private GridLayoutGroup gridLayout;     // 그리드레이아웃그룹 컴포넌트  

    public List<Block> SpawnBlocks(int blockCount)
    {            
        List<Block> blockList = new List<Block>(blockCount * blockCount);

        // 셀 크기
        int cellSize = 300 - 50 * (blockCount - 2);
        gridLayout.cellSize = new Vector2 (cellSize, cellSize);
        // 가로에 배치된 셀 개수
        gridLayout.constraintCount = blockCount;

        // 블록카운트 * 블록카운트 만큼 블록 생성
        for (int i = 0; i < blockCount; i++)
        {
            for (int j = 0; j < blockCount; j++)
            {
               Block block = Instantiate(blockPrefab, gridLayout.transform);
               blockList.Add(block);
            }
        }

        return blockList;
    }
}
