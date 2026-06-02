using UnityEngine;
using System.Collections.Generic;
public class ObjectPooler : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 5;
    private List<GameObject> pool;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreatePool();    
    }

    void CreatePool()
    {
        // reset pool to make it empty list waiting for gameobject to fill it
        pool = new List<GameObject>();

        for(int i = 0; i < poolSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }

    public GameObject GetPooledObject()
    {
        foreach(GameObject obj in pool)
        {
            // looks for first inactive one and returns to us
            if(!obj.activeSelf)
            {
                return obj;
            }
        }
        // if we run out of available objects because they're all in use
        // then we create another one with the function below
        return CreateNewObject();
    }
}

