using System;
using UnityEngine;
using System.Collections.Generic;


public class HashNode<K, V>
{
    public K Key { get; set; }
    public V Value { get; set; }

    public HashNode(K key, V value)
    {
        Key = key;
        Value = value;
    }
}
public class CustomHashMap<K, V>
{
    private List<HashNode<K, V>>[] buckets;
    private int capacity;

    public CustomHashMap(int size = 100)
    {
        capacity = size;
        buckets = new List<HashNode<K, V>>[capacity];

        for (int i = 0; i < capacity; i++)
        {
            buckets[i] = new List<HashNode<K, V>>();
        }
    }

    private int GetBucketIndex (K key)
    {
        int hashCode = key.GetHashCode();
        int index = Math.Abs(hashCode) % capacity;
        return index;
    }

    public void Put(K key, V value)
    { 
        int index = GetBucketIndex(key);
        List<HashNode<K, V>> bucket = buckets[index];

        foreach (var node in bucket)
        {
            if (node.Key.Equals(key))
            {
                node.Value = value;
                return;
            }
        }

        bucket.Add(new HashNode<K, V>(key, value));
    }

    public V Get(K key)
    {
        int index = GetBucketIndex(key);
        List<HashNode<K, V>> bucket = buckets[index];

        foreach (var node in bucket)
        {
            if (node.Key.Equals(key))
            {
                return node.Value;
            }
        }

        return default(V);
    }
}
