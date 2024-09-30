using UnityEngine;

namespace COM3D2.MoreFacilities.Plugin
{
    public class MoreFacilitiesManager : MonoBehaviour
    {
        public bool Initialized { get; private set; }
        public void Initialize()
        {
            if (this.Initialized)
                return;
            MoreFacilitiesHooks.Initialize();
            this.Initialized = true;
            Debug.Log("MoreFacilities: Manager Initialize");
        }

        public void Awake()
        {
            Debug.Log("MoreFacilities: Manager Awake");
            DontDestroyOnLoad(this);
        }
    }
}