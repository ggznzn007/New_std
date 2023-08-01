using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField]
    private Block blockPrefab;         // ��� ������
    [SerializeField]
    private GridLayoutGroup gridLayout;     // �׸��巹�̾ƿ��׷� ������Ʈ  

    public List<Block> SpawnBlocks(int blockCount)
    {            
        List<Block> blockList = new List<Block>(blockCount * blockCount);

        // �� ũ��
        int cellSize = 300 - 50 * (blockCount - 2);
        gridLayout.cellSize = new Vector2 (cellSize, cellSize);
        // ���ο� ��ġ�� �� ����
        gridLayout.constraintCount = blockCount;

        // ���ī��Ʈ * ���ī��Ʈ ��ŭ ��� ����
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
