using UnityEngine;

public static class Extensions
{
    //matrix access
    public static T At<T>(this T[,] array, Vector2Int pos) => array[pos.x, pos.y];
    public static void Set<T>(this T[,] array, Vector2Int pos, T value) => array[pos.x, pos.y] = value;
 
    public static T At<T>(this T[,] array, (int x, int y) pos) => array[pos.x, pos.y];
    public static void Set<T>(this T[,] array, (int x, int y) pos, T value) => array[pos.x, pos.y] = value;

    
    //transforms
    public static void SetWorldX(this Transform transform, float x) =>
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    public static void SetWorldY(this Transform transform, float y) =>
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    public static void SetWorldZ(this Transform transform, float z) =>
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    public static void SetWorldXY(this Transform transform, float x, float y) =>
        transform.position = new Vector3(x, y, transform.position.z);
    public static void SetWorldXY(this Transform transform, Vector2 pos) =>
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    public static void SetWorld(this Transform transform, Vector2 pos, float z) =>
        transform.position = new Vector3(pos.x, pos.y, z);
    
    
    public static void SetLocalX(this Transform transform, float x) =>
        transform.localPosition = new Vector3(x, transform.position.y, transform.position.z);
    public static void SetLocalY(this Transform transform, float y) =>
        transform.localPosition = new Vector3(transform.position.x, y, transform.position.z);
    public static void SetLocalZ(this Transform transform, float z) =>
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
    public static void SetLocalXY(this Transform transform, float x, float y) =>
        transform.localPosition = new Vector3(x, y, transform.localPosition.z);
    public static void SetLocalXY(this Transform transform, Vector2 pos) =>
        transform.localPosition = new Vector3(pos.x, pos.y, transform.localPosition.z);
    public static void SetLocal(this Transform transform, Vector2 pos, float z) =>
        transform.localPosition = new Vector3(pos.x, pos.y, z);
    
    
    
    //vectors
    public static bool ApproximatelyEqual(this Vector2 a, Vector2 b) =>
        Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);
    public static bool ApproximatelyEqual(this Vector3 a, Vector3 b) =>
        Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
    
    
    //floats
    public static float Abs(this float value) => Mathf.Abs(value);
    public static float Lerp(this float t, float value1, float value2) => Mathf.Lerp(value1, value2, t);
    public static float LowerBound(this float value, float bound) => Mathf.Max(value, bound);
    public static float UppedBound(this float value, float bound) => Mathf.Min(value, bound);
    public static void MoveRefTo(this ref float value, float target, float maxDelta) => value = Mathf.MoveTowards(value, target, maxDelta);
    public static float MoveTo(this float value, float target, float maxDelta) => Mathf.MoveTowards(value, target, maxDelta);
}
