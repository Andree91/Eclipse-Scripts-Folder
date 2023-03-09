using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [System.Serializable]
    public class SerializebleDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        // Called right before serialization
        // Save the dictionary TO list
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // Called right after serialization
        // Load the dictionary FROM list
        public void OnAfterDeserialize()
        {
            Clear();

            if (keys.Count != values.Count)
            {
                Debug.LogError($"Error while trying to deserialize the dictionary, the number of keys {keys.Count} doesn't match with the number of values {values.Count}");
            }

            for (int i = 0; i < keys.Count; i++)
            {
                Add(keys[i], values[i]);
            }
        }
    }
}
