using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Object instance.
        /// </summary>
        protected static T m_Instance;

        /// <summary>
        /// Get instance.
        /// </summary>
        public static T Instance
        {
            get
            {
                // If no references found, search it.
                if (m_Instance == null)
                {
                    m_Instance = (T)FindObjectOfType(typeof(T));
                }

                return m_Instance;
            }
        }

    
}
