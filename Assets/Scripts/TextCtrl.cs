using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCtrl : MonoBehaviour
{
	private int i_charIndex = 0;
	[HideInInspector] public int i_phraseIndex = 0;

	private float f_charTimer = 0.0f;
	private float f_phraseTimer = 0.0f;

	private float f_charTime		= 0.1f;
	private float f_charTimeVal		= 0.1f;
	private float f_phraseTime		= 2.0f;
	private float f_phraseTimeVal	= 2.0f;


	public bool b_onText;
	public bool b_shake;

	private string [] text;

	[SerializeField] private Text displayTxt;
	[SerializeField] private float f_shakeIntensity;
	private Vector3 v3_displayPos;


	private void Start()
	{
		b_onText = false;
		b_shake = false;

		i_charIndex = 0;
		i_phraseIndex = 0;

		f_charTimer = 0.0f;
		f_phraseTimer = 0.0f;
		v3_displayPos = displayTxt.rectTransform.position;
		displayTxt.text = "";
	}

	public void StartText(string [] newText, bool doShake = false)
	{
		i_charIndex		= 0;
		i_phraseIndex	= 0;
		f_charTimer		= 0.0f;
		f_phraseTimer	= 0.0f;

		text = newText;
		b_shake = doShake;
		b_onText = true;
	}


	private void Update()
	{
		if (!b_onText)
			return;

		//inputs
		if(	Input.GetKey(KeyCode.RightArrow)||
			Input.GetKey(KeyCode.DownArrow)	||
			Input.GetKey(KeyCode.LeftArrow) ||
			Input.GetKey(KeyCode.UpArrow) ||
			Input.GetKey(KeyCode.Space))
		{
			f_charTime = f_charTimeVal * .3f;
			f_phraseTime = f_phraseTimeVal * .3f;
		}
		else
		{
			f_charTime = f_charTimeVal;
			f_phraseTime = f_phraseTimeVal;
		}

		//chars
		f_charTimer += Time.deltaTime;
		if (f_charTimer > f_charTime)
		{
			if (i_phraseIndex < text.Length && 
				i_charIndex < text[i_phraseIndex].Length)
			{
				displayTxt.text += text[i_phraseIndex][i_charIndex];
				i_charIndex++;
				if (b_shake)
					displayTxt.rectTransform.position = v3_displayPos += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0) * f_shakeIntensity;
			}
			f_charTimer = 0;
		}

		//phrase
		if (i_phraseIndex < text.Length &&
			i_charIndex < text[i_phraseIndex].Length)
			return;

		displayTxt.rectTransform.position = v3_displayPos;
		f_phraseTimer += Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.RightArrow) ||
			Input.GetKeyDown(KeyCode.DownArrow)  ||
			Input.GetKeyDown(KeyCode.LeftArrow)  ||
			Input.GetKeyDown(KeyCode.UpArrow)	 ||
			Input.GetKeyDown(KeyCode.Space))
			f_phraseTimer = f_phraseTime;

		if (f_phraseTimer < f_phraseTime)
			return;

		//reset
		if (i_phraseIndex < text.Length)
		{
			displayTxt.text = "";
			f_charTimer = 0;
			f_phraseTimer = 0;
			i_charIndex = 0;
			i_phraseIndex++;
		}
		//end
		else
		{
			displayTxt.text = "";
			f_charTimer = 0;
			f_phraseTimer = 0;
			i_charIndex = 0;
			i_phraseIndex = 0;
			b_onText = false;
		}
	}
}
