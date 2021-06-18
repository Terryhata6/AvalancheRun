using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner current;
    private static GameObject _temporalGameObject;
    private static int _temporalInt;/// temporalInt
    
    
    [SerializeField]private GameObject _coinObject;    
    
    private void Awake()
    {
        //DontDestroyOnLoad(this);
        current = this;
    }


    public GameObject CreateCoin(Vector3 position) => Instantiate(_coinObject, position, Quaternion.identity);
    public GameObject CreateObject(Vector3 position, GameObject example)
    {
        return Instantiate(example, position, Quaternion.identity);
    }

    public void CreateObjectsInTime(GameObject[] objects, ObjectSpawnerView spawnerView, float deltaTime)
    {
        GameObject[] newObjects = objects;
        if (GameObject.FindObjectOfType<MainController>().IsDermische)
        {
            newObjects = Resources.Load<FallingObjectsCSO>("AllObjectChallenge").Objects;
            for (int i = 0; i < newObjects.Length - 1; i++)
            {
                _temporalGameObject = newObjects[i];
                _temporalInt = Random.Range(i + 1, newObjects.Length);
                newObjects[i] = newObjects[_temporalInt];
                newObjects[_temporalInt] = _temporalGameObject;
            }
        }
        StartCoroutine(Utilits.CreatingObjects(newObjects, spawnerView.transform, deltaTime)); 
        
    }

    private void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
