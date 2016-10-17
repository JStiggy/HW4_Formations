using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

	public void LoadScalable ()
    {
		print("loadingScalable");
        SceneManager.LoadScene(0);
	}
	
	public void LoadEmergent ()
	{
		print("loadingScalable");
		SceneManager.LoadScene(1);
    }

    public void LoadTwoLevelFormation()
	{
		print("loadingScalable");
		SceneManager.LoadScene(2);
    }
}
