using System;
using System.Collections.Generic;
using System.Linq;

public class MultiKeyDictionary<Key1, Key2, Value> : Dictionary<Key1, Dictionary<Key2, Value>>
{
    public Value this[Key1 key1, Key2 key2]
    {
        get
        {
            if (!ContainsKey(key1) || !this[key1].ContainsKey(key2))
                throw new ArgumentOutOfRangeException();
            return base[key1][key2];
        }
        set
        {
            if (!ContainsKey(key1))
                this[key1] = new Dictionary<Key2, Value>();
            this[key1][key2] = value;
        }
    }

    public bool TryGetValue(Key1 key1, Key2 key2, out Value value)
    {
        if (ContainsKey(key1) && this[key1].ContainsKey(key2))
        {
            value = this[key1][key2];
            return true;
        }

        value = default(Value);
        return false;
    }


    public void Add(Key1 key1, Key2 key2, Value value)
    {
        if (!ContainsKey(key1))
            this[key1] = new Dictionary<Key2, Value>();
        this[key1][key2] = value;
    }

    public bool ContainsKey(Key1 key1, Key2 key2)
    {
        return base.ContainsKey(key1) && this[key1].ContainsKey(key2);
    }

    public new IEnumerable<Value> Values
    {
        get
        {
            return from baseDict in base.Values
                   from baseKey in baseDict.Keys
                   select baseDict[baseKey];
        }
    }


}