using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    public RectTransform pad;
    public RectTransform stick;
    Vector3 playerRotate;

    Car player;
    Animator playerAni;
    bool onMove;
    float playerSpeed;

    [Header("MiniMap")]
    public GameObject miniMap;
    public Transform miniMapCam;

    public void StartController()
    {
        player = GameManager.instance.player;
        playerAni = player.GetComponent<Animator>();
        StartCoroutine("PlayerMove");
    }

    public void OnDrag(PointerEventData eventData)
    {
        stick.position = eventData.position;
        stick.localPosition = Vector3.ClampMagnitude(eventData.position - (Vector2)pad.position, pad.rect.width * 0.5f);

        playerRotate = new Vector3(0, stick.localPosition.x, 0).normalized;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        stick.localPosition = Vector3.zero;
        playerRotate = Vector3.zero;
    }

    public void OnMove()
    {
        StartCoroutine("Acceleration");
        onMove = true;
    }

    public void OffMove()
    {
        StartCoroutine("Braking");
    }

    IEnumerator PlayerMove()
    {
        miniMap.SetActive(true);
        while (true)
        {
            GameManager.instance.curSpeedText.text = string.Format("{0:000.00}", playerSpeed * 10);
            if (onMove)
            {
                player.transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);

                if(Mathf.Abs(stick.localPosition.x)>pad.rect.width*0.2f)
                {
                    player.transform.Rotate(playerRotate*30*Time.deltaTime);
                }
                if(Mathf.Abs(stick.localPosition.x)<=pad.rect.width*0.2f)
                {
                    playerAni.Play("Ani_Forward");
                }
                if(stick.localPosition.x>pad.rect.width*0.2f)
                {
                    playerAni.Play("Ani_Right");
                }
                if (stick.localPosition.x < pad.rect.width * -0.2f)
                {
                    playerAni.Play("Ani_Left");
                }
                player.transform.GetChild(3).gameObject.SetActive(true);
                player.transform.GetChild(4).gameObject.SetActive(false);
            }

            if(!onMove)
            {
                playerAni.Play("Ani_Idle");
                player.transform.GetChild(3).gameObject.SetActive(false);
                player.transform.GetChild(4).gameObject.SetActive(true);
            }

            miniMapCam.position = player.transform.position + new Vector3(-1, 80, 0);

            yield return null;
        }
    }

    IEnumerator Acceleration()
    {
        StopCoroutine("Braking");

        while (true)
        {
            playerSpeed += 7f * Time.deltaTime; // 초당 가속

            if (playerSpeed > player.carSpeed)
            {
                playerSpeed -= 7.5f * Time.deltaTime;
            }

            yield return null;
        }
    }

    IEnumerator Braking()
    {
        StopCoroutine("Acceleration");

        while (true)
        {
            playerSpeed -= 7f * Time.deltaTime; // 초당 감속

            if (playerSpeed <= 0)
            {
                playerSpeed = 0f;
                onMove = false;
                StopCoroutine("Braking");
            }

            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        stick.position = eventData.position;
        stick.localPosition = Vector3.ClampMagnitude(eventData.position - (Vector2)pad.position, pad.rect.width * 0.5f);

        playerRotate = new Vector3(0, stick.localPosition.x, 0).normalized;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        { OnMove(); }
        if (Input.GetKeyUp(KeyCode.A))
        { OffMove(); }
    }
}
