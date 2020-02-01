using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Pool params")]
    public IPoolable Prefab;
    public int PoolAmount;
    
    private List<IPoolable> _pool = new List<IPoolable>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PoolAmount; i++)
        {
            var poolable = Instantiate(Prefab, transform);
            poolable.gameObject.SetActive(false);
            _pool.Add(poolable);
        }
    }

    public GameObject GetObject()
    {
        GameObject go = null;
        foreach (var poolable in _pool)
        {
            if (poolable.gameObject.activeInHierarchy)
                continue;
            poolable.Reset();
            go = poolable.gameObject;
            go.SetActive(true);
            break;
        }

        if (go == null)
            Debug.LogError("Pool out of object (max = "+PoolAmount+")");
        
        return go;
    }
}
