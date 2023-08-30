using UnityEngine;

namespace Code.Helpers
{
    /// <summary>
    /// Mono singleton Class. Extend this class to make singleton component.
    /// Example:
    /// <code>
    /// public class Foo : MonoSingleton<Foo>
    /// </code>. To get the instance of Foo class, use <code>Foo.instance</code>
    /// Override <code>Init()</code> method instead of using <code>Awake()</code>
    /// from this class.
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                // Instance required for the first time, we look for it
                if (MonoSingleton<T>.instance == null)
                {
                    MonoSingleton<T>.instance = FindObjectOfType(typeof(T)) as T;

                    // Object not found, we create a temporary one
                    if (MonoSingleton<T>.instance == null)
                    {
                        Debug.LogWarning("No instance of " + typeof(T).ToString() + ", a temporary one is created.");

                        MonoSingleton<T>.IsTemporaryInstance = true;
                        MonoSingleton<T>.instance =
                            new GameObject("Temp Instance of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();

                        // Problem during the creation, this should not happen
                        if (MonoSingleton<T>.instance == null)
                        {
                            Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                        }
                    }

                    if (!MonoSingleton<T>.isInitialized)
                    {
                        MonoSingleton<T>.isInitialized = true;
                        MonoSingleton<T>.instance.Init();
                    }
                }

                return MonoSingleton<T>.instance;
            }
        }

        public static bool IsTemporaryInstance { private set; get; }

        private static bool isInitialized;

        // If no other monobehaviour request the instance in an awake function
        // executing before this one, no need to search the object.
        private void Awake()
        {
            if (MonoSingleton<T>.instance == null)
            {
                MonoSingleton<T>.instance = this as T;
            }
            else if (MonoSingleton<T>.instance != this)
            {
                Debug.LogError("Another instance of " + this.GetType() + " is already exist! Destroying self...");
                DestroyImmediate(this);

                return;
            }

            if (!MonoSingleton<T>.isInitialized)
            {
                //DontDestroyOnLoad(this.gameObject);
                MonoSingleton<T>.isInitialized = true;
                MonoSingleton<T>.instance.Init();
            }
        }

        /// <summary>
        /// This function is called when the instance is used the first time
        /// Put all the initializations you need here, as you would do in Awake
        /// </summary>
        public virtual void Init()
        {
        }

        /// Make sure the instance isn't referenced anymore when the user quit, just in case.
        private void OnApplicationQuit()
        {
            MonoSingleton<T>.instance = null;
        }
    }
}