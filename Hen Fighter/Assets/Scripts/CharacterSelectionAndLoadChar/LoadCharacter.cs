using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadCharacter : SingletonGeneric<LoadCharacter>
{
	public GameObject[] characterPrefabs;
    public GameObject[] enemyPrefabs;

    [HideInInspector]
    public GameObject enemyClone, playerClone;
    public Transform spawnPoint,SpawnRotaion;
    public Transform enemySpawnPoint, enemySpawnRotaion;
    public TMP_Text PlayerName,EnemyName;
	public GameObject originalObject; // Assign the original GameObject prefab in the Inspector
	public Vector3 rotationEulerAngles = new Vector3(0f, 90f, 0f);
    public Vector3 enemyrotationEulerAngles = new Vector3(0f, -90f, 0f);

    void Start()
	{
		SpawnEnemy();
		SpawnPlayer();
    }

	void SpawnPlayer()
	{
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        GameObject prefab = characterPrefabs[selectedCharacter];
        GameObject playerClone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        playerClone.transform.rotation = Quaternion.Euler(rotationEulerAngles);
        PlayerName.text = prefab.name;
        playerClone.SetActive(true);
    }

	void SpawnEnemy()
	{
        int randomCharacter = Random.Range(0, enemyPrefabs.Length);
        GameObject prefab = enemyPrefabs[randomCharacter];
        enemyClone = Instantiate(prefab, enemySpawnPoint.position, Quaternion.identity);
        enemyClone.transform.rotation = Quaternion.Euler(enemyrotationEulerAngles);
        EnemyName.text = prefab.name;
        enemyClone.SetActive(true);
    }
}
