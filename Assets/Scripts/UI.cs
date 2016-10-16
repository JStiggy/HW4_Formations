using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

	public void LoadScalable ()
    {
        SceneManager.LoadScene(0);
	}
	
	public void LoadEmergent ()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadTwoLevelFormation()
    {
        SceneManager.LoadScene(2);
    }
}
