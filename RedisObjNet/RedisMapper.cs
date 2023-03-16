using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RedisObjNet;

public class RedisMapper
{
    private readonly IDatabase _db;

    public RedisMapper(IDatabase db)
    {
        _db = db;
    }

    protected Dictionary<string, string> listOf = new();

    public void Set<T>(T item, string propertyName)
    {
        var type = typeof(T);
        var props = type.GetProperties();
        PropertyInfo? keyPropertyName = props.FirstOrDefault(p => p.GetCustomAttribute<RedisKeyAttribute>() != null);
        if (keyPropertyName == null)
        {
            //TODO
            throw new Exception();
        }

        var prop = props.FirstOrDefault(p => p.Name == propertyName);
        if(prop == null)
        {
            //TODO
            throw new Exception();
        }

        var classKey = type.Name.ToLower();
        var keyValued = ValueConverter.ToRedisString(keyPropertyName.GetValue(item));
        SetAllList(classKey, keyValued);
        var key = $"{classKey}:{keyValued}:{propertyName}";
        var value = ValueConverter.ToRedisString(prop.GetValue(item));
        _db.StringSet(key, value);
    }

    public void Set<T>(T item)
    {
        var type = typeof(T);
        var props = type.GetProperties();
        PropertyInfo? keyPropertyName = props.FirstOrDefault(p => p.GetCustomAttribute<RedisKeyAttribute>() != null);
        if (keyPropertyName == null)
        {
            //TODO
            throw new Exception();
        }

        var classKey = type.Name.ToLower();
        var keyValued = ValueConverter.ToRedisString(keyPropertyName.GetValue(item));
        SetAllList(classKey, keyValued);

        var baseKey = $"{classKey}:{keyValued}";
        foreach (var prop in props)
        {
            if (prop.GetCustomAttribute<RedisValueAttribute>() == null)
            {
                continue;
            }

            var key = $"{baseKey}:{prop.Name}";
            var value = ValueConverter.ToRedisString(prop.GetValue(item));
            _db.StringSet(key, value);
        }
    }

    private void SetAllList(string classKey, string keyValued)
    {
        var previousKeys = _db.StringGet($"{classKey}:$all").ToString();
        if (previousKeys.Split(',').Contains(keyValued))
        {
            return;
        }

        previousKeys += "," + keyValued;
        var key = $"{classKey}:$all";
        var value = previousKeys;
        _db.StringSet(key, value);
    }

    public int? GetInt<T>(string key, string propertyName)
    {
        var raw = GetString<T>(key, propertyName);
        if(raw == default)
        {
            return default;
        }
        return ValueConverter.IntFromRedisString(raw);
    }

    public string? GetString<T>(string key, string propertyName)
    {
        var type = typeof(T);
        var classKey = type.Name.ToLower();
        var completeKey = $"{classKey}:{key}:{propertyName}";
        var value = _db.StringGet(completeKey);

        if (!value.HasValue)
        {
            return default;
        }

        return value.ToString();
    }
}
