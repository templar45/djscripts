using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author:Christopher Cruz
public class projectile : MonoBehaviour
{
    [Range(5, 100)]
	public float destroyAfter;
	public int damage;
	public bool destroyOnImpact = false;
	public float minDestroyTime;
	public float maxDestroyTime;
	public GameObject bulletImpact;
    void Start()
    {
        StartCoroutine (DestroyTimer ());
    }
    private void OnCollisionEnter (Collision collision) 
	{
		print("Collided with " + collision.gameObject.name);
		if(collision.gameObject.GetComponent<CreatureStats>() != null)
        {
			collision.gameObject.GetComponent<CreatureStats>().TakeDamage(damage);
			Destroy(gameObject);
        }
		else if(!destroyOnImpact) 
		{
            StartCoroutine (DestroyTimer ());
		}
		//Otherwise, destroy bullet on impact
		else 
		{
			bulletImpact = Instantiate(bulletImpact, transform.position, Quaternion.identity);
			Destroy (gameObject);
			
		}
    }

    private IEnumerator DestroyTimer () 
	{
		//Wait random time based on min and max values
		yield return new WaitForSeconds
		(Random.Range(minDestroyTime, maxDestroyTime));
		//Destroy bullet object
		Destroy(gameObject);
	}
}
