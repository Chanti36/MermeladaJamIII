using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
	private Animator anim;

	private bool b_playing;
	private int i_hits;

	private float f_worldHeight, f_worldWidth;


	private float f_moveTimer;
	private float f_moveTime;
	private Vector2 endPos;
	private Vector2 startPos;

	public Transform child;

	void Start()
    {
		f_worldHeight = Camera.main.orthographicSize;
		f_worldWidth = f_worldHeight * ((float)Screen.width / Screen.height);

		i_hits = 0;
		b_playing = false;
		anim = GetComponent<Animator>();        
    }

    void Update()
    {
		if (!b_playing)
			return ;
		
		f_moveTimer += Time.deltaTime;
		transform.position = new Vector2(transform.position.x, Mathf.Lerp(startPos.y, endPos.y, f_moveTimer / f_moveTime));
		print("workds");
		if (f_moveTimer > f_moveTime)
		{
			print("reset");
			f_moveTimer = 0;

			f_moveTime = Random.Range(1.0f, 2.0f);
			endPos = new Vector2(transform.position.x, Random.Range(-f_worldWidth + child.GetComponent<SpriteRenderer>().size.y,
														f_worldWidth - child.GetComponent<SpriteRenderer>().size.y));
			startPos = transform.position;
		}
    }

	public void Show()
	{
		anim.SetTrigger("show");
	}

	public void StartFight()
	{
		b_playing = true;
		anim.enabled = false;
		child.transform.localPosition = Vector3.zero;
	}

	public int Hits()
	{
		return (i_hits);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (b_playing && collision.gameObject.tag == "cowshot")
		{
			collision.gameObject.SetActive(false);
			i_hits++;
		}
	}
}
