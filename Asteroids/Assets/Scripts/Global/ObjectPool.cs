using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private int _poolSize = 40;
    [SerializeField] private T _prefab;

    private Queue<T> _objectPool = new Queue<T>();

    private void Start()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            T obj = Instantiate(_prefab);
            obj.gameObject.SetActive(false);
            _objectPool.Enqueue(obj);
        }
    }

    public T GetObject()
    {
        if (_objectPool.Count > 0)
        {
            T obj = _objectPool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            T obj = Instantiate(_prefab);
            return obj;
        }
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        _objectPool.Enqueue(obj);
    }
}
