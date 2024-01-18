using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection_New : MonoBehaviour
{
	public GameObject[] characters;
	public int selectedCharacter = 0;

	public GameObject[] Images;
	public int selectImage = 0;

	public void NextCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter = (selectedCharacter + 1) % characters.Length;
		characters[selectedCharacter].SetActive(true);


		Images[selectImage].SetActive(false);
		selectImage = (selectImage + 1) % Images.Length;
		Images[selectImage].SetActive(true);
	}

	public void PreviousCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter--;
		if (selectedCharacter < 0)
		{
			selectedCharacter += characters.Length;
		}
		characters[selectedCharacter].SetActive(true);

		Images[selectImage].SetActive(false);
		selectImage--;
		if (selectImage < 0)
		{
			selectImage += Images.Length;
		}
		Images[selectImage].SetActive(true);
	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
		SceneManager.LoadScene(2, LoadSceneMode.Single);
	}
}
