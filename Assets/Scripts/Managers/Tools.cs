using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Json;
using System.Reflection;
using System.Globalization;

public static class Tools
{
    public static float[] ToArray(this Vector3 vector) => new[] { vector.x, vector.y, vector.z };
    public static Vector3 ToVector3(this float[] array) => new Vector3(array[0], array[1], array[2]);
    public static Vector3 MoveTowards(this Vector3 start, Vector3 end, float maxDelta, out bool reachedEnd)
    {
        var travel = end - start;
        var dist = travel.magnitude;
        if (maxDelta >= dist)
        {
            reachedEnd = true;
            return end;
        }
        reachedEnd = false;
        return start + travel.normalized * maxDelta;
    }
    public static Vector2 MoveTowards(this Vector2 start, Vector2 end, float maxDelta, out bool reachedEnd)
    {
        var travel = end - start;
        var dist = travel.magnitude;
        if (maxDelta >= dist)
        {
            reachedEnd = true;
            return end;
        }
        reachedEnd = false;
        return start + travel.normalized * maxDelta;
    }
    public static Vector3 LerpDirection(this Vector3 original, Vector3 target, float amount, bool onlyIfOpposing) => original.LerpDirection(target, target.magnitude, amount, onlyIfOpposing);
    public static Vector3 LerpDirection(this Vector3 original, Vector3 direction, float target, float amount, bool onlyIfOpposing) => original.LerpDirection(direction, target, amount, x => !onlyIfOpposing || x < 0);
    public static Vector3 LerpDirection(this Vector3 original, Vector3 target, float amount, Predicate<float> condition = null) => original.LerpDirection(target, target.magnitude, amount, condition);
    public static Vector3 LerpDirection(this Vector3 original, Vector3 direction, float target, float amount, Predicate<float> condition = null)
    {
        var a = direction == Vector3.zero ? default : Quaternion.LookRotation(direction);
        original = Quaternion.Inverse(a) * original;
        if (condition?.Invoke(original.z) ?? true)
        {
            original.z = Mathf.Lerp(original.z, target, amount);
        }
        return a * original;
    }
    public static bool Is<T>(this Type t) => t.Is(typeof(T));
    public static bool Is(this Type t, Type type) => type.IsAssignableFrom(t);

    public static bool TryAsType<T>(this object value, out T result) where T : IConvertible
    {
        if (value is T)
        {
            result = (T)value;
            return true;
        }
        if (value is IConvertible && typeof(T).Is<IConvertible>())
        {
            if (typeof(T).IsEnum)
            {
                result = (T)Enum.ToObject(typeof(T), value);
                return true;
            }
            var format = NumberFormatInfo.CurrentInfo;
            result = (T)(value as IConvertible).ToType(typeof(T), format);
            return true;
        }
        result = default;
        return false;
    }

    public static bool TryAsType(this object value, Type type, out object result)
    {
        if (type.IsSubclassOf(typeof(IConvertible)))
            throw new ArgumentException("The type must inherit the IConvertible interface", "type");
        if (type.IsInstanceOfType(value))
        {
            result = value;
            return true;
        }
        if (value is IConvertible)
        {
            if (type.IsEnum)
            {
                result = Enum.ToObject(type, value);
                return true;
            }
            var format = NumberFormatInfo.CurrentInfo;
            result = (value as IConvertible).ToType(type, format);
            return true;
        }
        result = null;
        return false;
    }

    public static bool TryGetValue<T>(this JsonObject obj, string key, ref T result)
    {
        if (obj.TryGetValue(key, out var value))
        {
            if (value is T cast)
            {
                result = cast;
                return true;
            }
            if (value is JsonPrimitive prim && typeof(T).Is<IConvertible>() && prim.Value.TryAsType(typeof(T), out var v))
                try
                {
                    result = (T)v;
                    return true;
                }
                catch { }
        }
        return false;
    }
    public static bool TryGetValue2<T>(this JsonObject obj, string key, out T result)
    {
        result = default;
        return obj.TryGetValue(key, ref result);
    }
    public static JsonObject CreateChild(this JsonObject obj, string key)
    {
        var n = new JsonObject();
        obj[key] = n;
        return n;
    }
    public static Vector3 GetRandomEular() => new Vector3(Mathf.Asin(UnityEngine.Random.Range(-1, 1f)) * 180, UnityEngine.Random.Range(0, 360f), 0);
}

public class Ref<T>
{
    public T value;
    public static implicit operator T(Ref<T> r) => r.value;
    public static implicit operator Ref<T>(T v) => new Ref<T> { value = v };
}