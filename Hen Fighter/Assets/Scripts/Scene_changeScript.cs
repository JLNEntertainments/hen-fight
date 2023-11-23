using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_changeScript : MonoBehaviour
{
	public void SceneChange(string ScreneName)
	{
		SceneManager.LoadScene(ScreneName);
	}
}
