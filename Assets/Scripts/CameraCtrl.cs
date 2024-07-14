using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
	private Vector3 mainPosition;
	private float f_time;
	private float f_intensity;

    void Start()
    {
		mainPosition = transform.position;        
    }

    void Update()
    {
		if (f_time < 0)
		{
			transform.position = mainPosition;
			return;
		}

		f_time -= Time.deltaTime;
		transform.position = mainPosition + new Vector3(Random.Range(-.1f, .1f) * f_intensity, Random.Range(-.1f, .1f) * f_intensity, -10);		
    }

	public void DoShake(float f_shaketime, float f_shakeIntensity = 1)
	{
		f_intensity = f_shakeIntensity;
		f_time = f_shaketime;
	}
}
