using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CellScript : MonoBehaviour
{
    public int colorIndex;
    public Vector3[] ShapePos;
    GameManager GM;
    Vector3 BlockPos, pos;

    void Start()
    {

        BlockPos = transform.parent.position;
        GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        ShapePos = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            ShapePos[i] = transform.GetChild(i).localPosition;
    }

    IEnumerator FollowMouse()
    {
        while (true)
        {
            yield return null;
            transform.position = Vector3.Lerp(transform.position, pos + new Vector3(0, 1, 0), Time.deltaTime * 100);
        }
    }

    void OnMouseDown()
    {
        SoundManager.instance.Playsound(SoundManager.instance.blockClick);
        transform.DOScale(1, 0.2f);
        StartCoroutine(nameof(FollowMouse));
    }

    void OnMouseDrag()
    {
        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, -0.5f, 10);
    }

    void OnMouseUp()
    {
        SoundManager.instance.Playsound(SoundManager.instance.blockDrop);
        StopCoroutine(nameof(FollowMouse));
        transform.DOScale(0.6f, 0.2f);
        transform.DOMove(BlockPos, 0.2f);
        Vector3 lastPos = pos + new Vector3(0, 1, 0);
        lastPos.x = Mathf.RoundToInt(lastPos.x);
        lastPos.y = Mathf.RoundToInt(lastPos.y);

        GM.BlockInput(this, colorIndex, lastPos, ShapePos);
    }

    public void ForceDestroy()
    {
        DOTween.KillAll();
        Destroy(gameObject);
    }
}
