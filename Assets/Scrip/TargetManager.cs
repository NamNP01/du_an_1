using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    private static TargetManager _instance;
    public static TargetManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TargetManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("TargetManager");
                    _instance = go.AddComponent<TargetManager>();
                }
            }
            return _instance;
        }
    }

    private HashSet<Transform> targets = new HashSet<Transform>();

    public bool RegisterTarget(Transform target)
    {
        if (!targets.Contains(target))
        {
            targets.Add(target);
            return true;
        }
        return false;
    }

    public void UnregisterTarget(Transform target)
    {
        if (targets.Contains(target))
        {
            targets.Remove(target);
        }
    }
}
