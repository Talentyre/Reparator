using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
	public void LoadGame ()
	{
		SceneManager.LoadScene ("MainScene");
	}
}