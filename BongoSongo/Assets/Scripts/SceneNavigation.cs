using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
   public void StartDance()
	{
        SceneManager.LoadScene("WebCamTestScene");
	}

	public void SelectSong()
	{   
		SceneManager.LoadScene ("SongSelect");   
	}

	public void HomeScreen()
	{
		SceneManager.LoadScene ("HomeScreen");
	}

    public void EndScreen()
	{
		SceneManager.LoadScene ("EndScreen");
	}

    public void CloseApp()
    {
        // Function linked to close button on HomeScreen
        Debug.Log ("Closing App");
    }
}
