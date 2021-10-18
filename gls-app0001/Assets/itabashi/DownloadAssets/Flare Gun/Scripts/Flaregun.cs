using UnityEngine;
using System.Collections;

public class Flaregun : MonoBehaviour
{

	public Rigidbody flareBullet;
	public Transform barrelEnd;
	public GameObject muzzleParticles;
	public AudioClip flareShotSound;

	public int bulletSpeed = 2000;

	public void Shoot()
	{
		GetComponent<Animation>().CrossFade("Shoot");
		GetComponent<AudioSource>().PlayOneShot(flareShotSound);


		Rigidbody bulletInstance;
		bulletInstance = Instantiate(flareBullet, barrelEnd.position, barrelEnd.rotation) as Rigidbody; //INSTANTIATING THE FLARE PROJECTILE


		bulletInstance.AddForce(barrelEnd.forward * bulletSpeed); //ADDING FORWARD FORCE TO THE FLARE PROJECTILE

		Instantiate(muzzleParticles, barrelEnd.position, barrelEnd.rotation);   //INSTANTIATING THE GUN'S MUZZLE SPARKS	
	}
}
