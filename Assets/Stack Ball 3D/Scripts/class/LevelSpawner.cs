using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSpawner : MonoBehaviour
{
    [Header("Element")]
    public GameObject[] model;
    [HideInInspector]
    public GameObject[] modelPrefab = new GameObject[4];
    public GameObject WinPrefab;

    private GameObject temp1, temp2;

    [Header("Setting")]
    public int level = 1, addOn = 7;
    private float i = 0;

    SecurePlayerPrefs securePlayerPrefs;

    #region UnityLifecycle
    void Awake()
    {
        // PlayerPrefs.DeleteAll();
        securePlayerPrefs = new SecurePlayerPrefs(PlayerPrefsTags.KEY, PlayerPrefsTags.LEVEL);
        level = PlayerPrefs.GetInt(PlayerPrefsTags.LEVEL);
        SpawnPrefab();
    }
    #endregion

    #region FunctionPrivate
    private void SpawnPrefab()
    {
        if (level > 9)
            addOn = 0;

        ModelSelection();
        float random = Random.value;

        for (i = 0; i > -level - addOn; i -= 0.5f)
        {
            if (level <= 20)
                temp1 = Instantiate(modelPrefab[Random.Range(0, 2)]);

            if (level > 20 && level <= 50)
                temp1 = Instantiate(modelPrefab[Random.Range(1, 3)]);

            if (level > 50 && level <= 100)
                temp1 = Instantiate(modelPrefab[Random.Range(2, 4)]);

            if (level > 100)
                temp1 = Instantiate(modelPrefab[Random.Range(3, 4)]);

            temp1.transform.position = new Vector3(0, i - 0.01f, 0);
            temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);

            // Mathf.Abs: returns the absolute value of a given number. It can handle both integer and floating-point values.
            if (Mathf.Abs(i) >= level * .3f && Mathf.Abs(i) <= level * .6f)
            {
                temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);
                temp1.transform.eulerAngles += Vector3.up * 180;
            }
            else if (Mathf.Abs(i) >= level * .8f)
            {
                temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);

                if (random > .75f)
                    temp1.transform.eulerAngles += Vector3.up * 180;
            }

            temp1.transform.parent = FindObjectOfType<Rotator>().transform;
        }

        temp2 = Instantiate(WinPrefab);
        temp2.transform.position = new Vector3(0, i - 0.01f, 0);
    }

    private void ModelSelection()
    {
        int randomModel = Random.Range(0, 5);

        switch (randomModel)
        {
            case 0:
                for (int i = 0; i < 4; i++)
                    modelPrefab[i] = model[i];
                break;
            case 1:
                for (int i = 0; i < 4; i++)
                    modelPrefab[i] = model[i + 4];
                break;
            case 2:
                for (int i = 0; i < 4; i++)
                    modelPrefab[i] = model[i + 8];
                break;
            case 3:
                for (int i = 0; i < 4; i++)
                    modelPrefab[i] = model[i + 12];
                break;
            case 4:
                for (int i = 0; i < 4; i++)
                    modelPrefab[i] = model[i + 16];
                break;
        }
    }

    private void CheckForCheating()
    {
        if (securePlayerPrefs.HasBeenEdited())
        {
            Debug.Log("Stop cheating!");
            PlayerPrefs.SetInt(PlayerPrefsTags.LEVEL, level);
        }
        else
        {
            Debug.Log("Game clear!");
            SceneManager.LoadScene(0);
        }
    }
    #endregion

    #region FunctionPublic
    public void NextLevel()
    {
        PlayerPrefs.SetInt(PlayerPrefsTags.LEVEL, PlayerPrefs.GetInt(PlayerPrefsTags.LEVEL) + 1);
        securePlayerPrefs.Save();
        CheckForCheating();
    }
    #endregion

}
