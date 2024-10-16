using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Pool/Manager")]
public class PoolManagerSO : ScriptableObject
{
    public List<PoolingItemSO> poolingItems;
    private Dictionary<PoolTypeSO, Pool> _pools;

    [SerializeField] private Transform _rootTrm;

    public void InitializePool(Transform root)
    {
        _rootTrm = root;
        _pools = new Dictionary<PoolTypeSO, Pool>();

        foreach (PoolingItemSO item in poolingItems)
        {
            IPoolable poolable = item.prefab.GetComponent<IPoolable>();
            Debug.Assert(poolable != null, $"Pool Item does not have IPoolable interface {item.prefab.name}");

            Pool pool = new Pool(poolable, _rootTrm, item.initCount);
            _pools.Add(item.poolType, pool);
        }
    }

    public IPoolable Pop(PoolTypeSO type)
    {
        if (_pools.TryGetValue(type, out Pool pool)) return pool.Pop();
        return null;
    }

    public void Push(IPoolable item)
    {
        if (_pools.TryGetValue(item.PoolType, out Pool pool)) pool.Push(item);
    }
}
