using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* <CharacterController의 충돌 체크>
 * 1.OnControllerColliderHit 이벤트 메서드 호출
 * 1) 두 오브젝트 모두 Collider가 있어야 한다
 * 2) 둘 중 하나는 CharacterController가 포함되어야 한다
 * 3) CharacterController를 가진 오브젝트가 컴포넌트 기능으로
 *    Move해야 한다
 *    
 * 2. OnTriggerEnter 이벤트 메서드 호출
 * 1) 두 오브젝트 모두 Collider가 있어야 한다
 * 2) 둘 중 하나는 CharacterController가 포함되어야 한다
 * 3) 둘 중 하나는 IsTrigger가 체크되어야 한다
 * 4) 둘 중 아무나 움직여도 이벤트가 발생한다
 * 5) 만나도 통과를 하게 된다
 */

/* Rigidbody
 *  1) 움직이면 힘이 가해지므로 상대 오브젝트에도 힘이 가해진다
 * CharacterController
 *  1) 움직이다가 상대를 만나면 힘을 가하지 않고 멈추게 된다
 */

public class csCollisionCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Portal"))
        {
            // 현재 씬 이름
            string sceneName = SceneManager.GetActiveScene().name;
            switch (sceneName)
            {
                case "CastleScene":
                    SceneManager.LoadScene("HouseScene");
                    break;
                case "HouseScene":
                    SceneManager.LoadScene("CastleScene");
                    break;
            }
        }
    }
}
