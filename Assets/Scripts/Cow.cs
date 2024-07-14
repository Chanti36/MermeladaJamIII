using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : MonoBehaviour
{
	public string nickname;
	public bool b_shoot;

	public float f_spd;


	private float f_worldHeight, f_worldWidth;

	private void Awake()
	{
		b_shoot = false;

		f_worldHeight = Camera.main.orthographicSize;
		f_worldWidth = f_worldHeight * ((float)Screen.width / Screen.height);
	}

	private void Update()
	{
		if (b_shoot)
		{
			transform.position += new Vector3(f_spd, 0, 0);
			if (transform.position.x > f_worldWidth + 2 ||
				transform.position.x < -f_worldWidth - 2)
				gameObject.SetActive(false);
		}
	}
}
