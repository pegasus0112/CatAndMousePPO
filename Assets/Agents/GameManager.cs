using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Setup")]
    public List<GameObject> mice = new List<GameObject>();
    public List<GameObject> cats = new List<GameObject>();
    public Transform spawnPoints;
    [Space(10)]
    [Header("Settings")]
    [Range(30, 600)] public float gameTime;

    float playedTime;
    float existingMice;
    public float deadMice = 0;
    bool playing;

    // Start is called before the first frame update
    void Start()
    {
        existingMice= mice.Count;
        placeTeams();
    }

    // Update is called once per frame
    void Update()
    {
        playedTime += Time.deltaTime;
        if (playedTime > gameTime || existingMice <= deadMice)
        {
            ResetScene();
        }
    }

    void placeTeams()
    {
        int miceSpawnNumber = Random.Range(0, spawnPoints.childCount-1);
        int catSpawnNumber = Random.Range(0, spawnPoints.childCount - 1);
        while (catSpawnNumber == miceSpawnNumber)
        {
            catSpawnNumber = Random.Range(0, spawnPoints.childCount - 1);
        }
        Vector3 miceLocation = new Vector3(spawnPoints.GetChild(miceSpawnNumber).transform.position.x, 0.25f, spawnPoints.GetChild(miceSpawnNumber).transform.position.z);
        placeMice(miceLocation);

        Vector3 catLocation = new Vector3(spawnPoints.GetChild(catSpawnNumber).transform.position.x, 0.25f, spawnPoints.GetChild(catSpawnNumber).transform.position.z);
        placeCat(catLocation);
    }

    void placeMice(Vector3 miceLocation) {
        mice[0].transform.SetPositionAndRotation(miceLocation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        miceLocation.x += 1;
        mice[1].transform.SetPositionAndRotation(miceLocation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        miceLocation.x -= 2;
        mice[2].transform.SetPositionAndRotation(miceLocation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
    }

    void placeCat(Vector3 catLocation)
    {
        cats[0].transform.SetPositionAndRotation(catLocation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        catLocation.x += 1;
        cats[1].transform.SetPositionAndRotation(catLocation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        catLocation.x -= 2;
        cats[2].transform.SetPositionAndRotation(catLocation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
    }

    void ResetScene()
    {
        foreach (GameObject cat in cats)
        {
            cat.GetComponent<AgentCatForTeam>().EndEpisode();
            cat.SetActive(false);
        }
        foreach (GameObject mouse in mice)
        {
                mouse.GetComponent<AgentMouseForTeam>().EndEpisode();
                mouse.SetActive(false);
        }
        placeTeams();
        deadMice = 0;
        playedTime = 0;
        foreach (GameObject cat in cats)
        {
            cat.SetActive(true);
        }
        foreach (GameObject mouse in mice)
        {
            mouse.SetActive(true);
        }
    }
}
