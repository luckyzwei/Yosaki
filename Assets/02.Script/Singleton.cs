using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;

public abstract class Singleton<T> where T : new()
{
    public static T Instance
    {
        get
        {
            //if (null == mInstance)
            if (object.ReferenceEquals(mInstance, null))
            {
                mInstance = new T();
                MethodInfo info = typeof(T).GetMethod("OnInstance", BindingFlags.NonPublic | BindingFlags.Instance);

                if (null != info)
                {
                    info.Invoke(mInstance, new Object[0]);
                }
            }

            return mInstance;
        }
    }

    protected static T mInstance;

    virtual protected void OnInstance() { }
}



public class SingletonMono<T> : MonoBehaviour where T : class, new()
{
    static T ms_Instance;
    static bool ms_bInstance = false;
    public bool m_DontDestroy = true;

    public static T Instance
    {
        get
        {
            if (!ms_bInstance)
            {
            }

            return ms_Instance;
        }
    }

    protected void Awake()
    {
        if (m_DontDestroy && ms_bInstance)
        {
        }

        ms_Instance = this as T;
        ms_bInstance = true;

        if (m_DontDestroy && transform.parent == null)
        {
            UnityEngine.Object.DontDestroyOnLoad(this);
        }
    }

    static public bool IsInstance()
    {
        return ms_bInstance;
    }

    protected void OnDestroy()
    {
        ms_bInstance = false;
        ms_Instance = null;
    }

    private void WhenEnemyDead()
    {

    }
}

public class SingletonEventTrigger<T> : EventTrigger where T : class, new()
{
    static T ms_Instance;
    static bool ms_bInstance = false;
    public bool m_DontDestroy = true;



    public static T Instance
    {
        get
        {
            if (!ms_bInstance)
            {
            }

            return ms_Instance;
        }
    }

    protected void Awake()
    {
        if (m_DontDestroy && ms_bInstance)
        {
        }

        ms_Instance = this as T;
        ms_bInstance = true;

        if (m_DontDestroy)
        {
            UnityEngine.Object.DontDestroyOnLoad(this);
        }
    }

    void OnDestroy()

    {
        ms_bInstance = false;
        ms_Instance = null;
    }
}