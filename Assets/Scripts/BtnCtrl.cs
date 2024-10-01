using UnityEngine;

public class BtnCtrl : MonoBehaviour
{
	public bool esp;
	public GameCtrl gameCtrl;

	public void SelectLanguaje()
	{
		if (esp)	gameCtrl.LanguajeESP();
		else		gameCtrl.LanguajeENG();
	}
}
