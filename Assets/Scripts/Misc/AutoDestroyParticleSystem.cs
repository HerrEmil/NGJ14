using UnityEngine;
using System.Collections;

public class AutoDestroyParticleSystem : MonoBehaviour {

	private ParticleSystem ps;
	
	////////////////////////////////////////////////////////////////
	
	void Start () {
		ps = GetComponent<ParticleSystem>();
	}
	
	void Update () {
		if(ps)
		{
			if(!ps.IsAlive())
			{
				Destroy(gameObject);
			}
		}
	}
}
