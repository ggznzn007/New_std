using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
	float power = 5000.0f;
	Vector3 velocity;	

	void Start()
	{
		velocity = transform.right * power;

		GetComponent<Rigidbody>().AddForce(velocity);
		
		StartCoroutine("DeleteCannon");
	}

	IEnumerator DeleteCannon()
	{
		yield return new WaitForSeconds(2.5f);
		Destroy(this.gameObject);
	}
}
