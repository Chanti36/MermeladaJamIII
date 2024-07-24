using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCtrl : MonoBehaviour
{
	[SerializeField] private PlayerCtrl player;

	public int i_gameState = 0;

	[HideInInspector] public int i_cowCount;

	//-------------------------------BG-------------------------------
	[SerializeField] private GameObject star;
	[SerializeField] private Sprite[] startSprites;
	[SerializeField] private GameObject starsContainer;
	private GameObject[] pool;
	int i_size;

	public Color color1;
	public Color color2;
	public Color color3;
	public Color c_bgColor1;
	public Color c_bgColor2;
	public float spd1;
	public float spd2;
	public float spd3;

	public float f_bgDuration;

	private float f_worldHeight, f_worldWidth;

	//-------------------------------OBJS-------------------------------
	[SerializeField] private GameObject cowGO;
	[SerializeField] private GameObject meteoriteGO;
	[SerializeField] private Sprite[] cowSprites;
	private GameObject[] cowPool;
	private GameObject[] meteoritePool;
	//private string[] cowNames = {"", ""};
	private float metSpawnDist = 15;
	private float cowSpawnDist = 50;
	private float metDist = 0;
	private float cowDist = 0;


	public int i_ending;

	   
	void Start()
	{
		f_worldHeight = Camera.main.orthographicSize;
		f_worldWidth = f_worldHeight * ((float)Screen.width / Screen.height);

		InitializeBg();
		InitializeObjs();

		i_cowCount = 0;
		i_ending = 0;
	}

	void InitializeBg()
	{
		i_size = 120;
		pool = new GameObject[i_size];

		for (int i = 0; i < i_size; ++i)
		{
			GameObject prefab = Instantiate(star);
			prefab.GetComponent<SpriteRenderer>().sprite = startSprites[Random.Range(0, startSprites.Length)];
			if (i < i_size / 3) prefab.GetComponent<SpriteRenderer>().color = color1;
			else if (i < (i_size / 3) * 2) prefab.GetComponent<SpriteRenderer>().color = color2;
			else prefab.GetComponent<SpriteRenderer>().color = color3;

			pool[i] = prefab;
			pool[i].transform.position = new Vector3(Random.Range(-f_worldWidth, f_worldWidth),
														Random.Range(-f_worldHeight - 1, f_worldHeight + 1),
														10);
			pool[i].transform.parent = starsContainer.transform;
		}
	}

	void InitializeObjs()
	{
		cowPool			= new GameObject[20];
		meteoritePool	= new GameObject[20];

		for (int i = 0; i < 20; ++i)
		{
			GameObject newCow = Instantiate(cowGO);
			GameObject newMet = Instantiate(meteoriteGO);

			cowPool[i] = newCow;
			cowPool[i].SetActive(false);
			meteoritePool[i] = newMet;
			meteoritePool[i].SetActive(false);
		}
	}

	GameObject GetCow()
	{
		for (int i = 0; i < 20; ++i)
		{
			if (!cowPool[i].activeInHierarchy)
				return (cowPool[i]);
		}
		return (null);
	}

	GameObject GetMet()
	{
		for (int i = 0; i < 20; ++i)
		{
			if (!meteoritePool[i].activeInHierarchy)
				return (meteoritePool[i]);
		}
		return (null);
	}

	// Update is called once per frame
	void Update()
	{
		float t = Mathf.PingPong(Time.time, f_bgDuration) / f_bgDuration;
		Camera.main.backgroundColor = Color.Lerp(c_bgColor1, c_bgColor2, t);

		if (i_gameState == 1)
			Mode1();
		else if (i_gameState == 2)
			Mode2();
	}

	private void Mode1()
	{
		if (!player.b_playing)
			return;

		float f_spd;
		for (int i = 0; i < i_size; ++i)
		{
			if (i < i_size / 3) f_spd = spd1;
			else if (i < (i_size / 3) * 2) f_spd = spd2;
			else f_spd = spd3;

			pool[i].transform.position = pool[i].transform.position - new Vector3(1, 0, 0) * (f_spd / 2) * player.GetXSpeed() * 5;

			if (pool[i].transform.position.x < -f_worldWidth - 2)
				pool[i].transform.position = new Vector3(f_worldWidth + 2, Random.Range(-f_worldHeight - 1, f_worldHeight + 1), 10);
			if (pool[i].transform.position.x > f_worldWidth + 2)
				pool[i].transform.position = new Vector3(-f_worldWidth - 2, Random.Range(-f_worldHeight - 1, f_worldHeight + 1), 10);
		}

		for (int i = 0; i < 20; ++i)
		{
			if (cowPool[i].activeInHierarchy)
			{
				cowPool[i].transform.position = cowPool[i].transform.position - new Vector3(1, 0, 0) * spd1 * player.GetXSpeed() * 5;
				if (cowPool[i].transform.position.x < -f_worldWidth - 2)
					cowPool[i].SetActive(false);
			}
			if (meteoritePool[i].activeInHierarchy)
			{
				meteoritePool[i].transform.position = meteoritePool[i].transform.position - new Vector3(1, 0, 0) * spd1 * player.GetXSpeed() * 5;
				if (meteoritePool[i].transform.position.x < -f_worldWidth - 2)
					meteoritePool[i].SetActive(false);
			}
		}

		if (i_cowCount > 5)
			return ;

		//SPAWN
		if (player.GetXSpeed() > 0)
		{
			metDist += player.GetXSpeed() * 0.4f;
			cowDist += player.GetXSpeed() * 0.4f;

			if (metDist >= metSpawnDist)
			{			
				metDist = 0;

				GameObject prefab = GetMet();
				if (prefab)
				{
					prefab.transform.position = new Vector3(f_worldWidth + 2,
															Random.Range(-f_worldHeight + prefab.GetComponent<SpriteRenderer>().size.y,
															f_worldHeight - prefab.GetComponent<SpriteRenderer>().size.y),
															10);
					prefab.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
					prefab.SetActive(true);
					prefab.GetComponent<CircleCollider2D>().enabled = true;
				}
			}
			if (cowDist >= cowSpawnDist)
			{
				cowDist = 0;

				GameObject prefab = GetCow();
				if (prefab)
				{
					prefab.GetComponent<SpriteRenderer>().sprite = cowSprites[Random.Range(0, cowSprites.Length)];
					prefab.transform.position = new Vector3(f_worldWidth + 2,
															Random.Range(-f_worldHeight + prefab.GetComponent<SpriteRenderer>().size.y,
															f_worldHeight - prefab.GetComponent<SpriteRenderer>().size.y),
															9);
					prefab.SetActive(true);
				}
			}
		}
	}
	
	private void Mode2()
	{
		float f_spd;
		for (int i = 0; i < i_size; ++i)
		{
			if (i < i_size / 3) f_spd = spd1;
			else if (i < (i_size / 3) * 2) f_spd = spd2;
			else f_spd = spd3;

			pool[i].transform.position = pool[i].transform.position + new Vector3(1, 0, 0) * f_spd;

			if (pool[i].transform.position.x > f_worldWidth + 2)
				pool[i].transform.position = new Vector3(-f_worldWidth - 2, Random.Range(-f_worldHeight - 1, f_worldHeight + 1), 10);
		}

		if (player.transform.position.x < -f_worldWidth - 2)
			i_ending = 1;
	}

	public bool ScenaryEmpty()
	{
		for (int i = 0; i < 20; i++)
		{
			if (cowPool[i].activeInHierarchy || meteoritePool[i].activeInHierarchy)
				return (false);
		}
		return (true);
	}
}
