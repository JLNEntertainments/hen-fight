using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadCharacter : MonoBehaviour
{
	public GameObject[] characterPrefabs;
	public Transform spawnPoint,SpawnRotaion;
	public TMP_Text label;
	public GameObject originalObject; // Assign the original GameObject prefab in the Inspector
	public Vector3 rotationEulerAngles = new Vector3(0f, 90f, 0f);
	void Start()
	{
		int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
		GameObject prefab = characterPrefabs[selectedCharacter];
		GameObject clone = Instantiate(prefab, spawnPoint.position,  Quaternion.identity);
		clone.transform.rotation = Quaternion.Euler(rotationEulerAngles);
		label.text = prefab.name;
	}
}
