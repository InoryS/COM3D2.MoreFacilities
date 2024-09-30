using BepInEx;
using System;
using UnityEngine;

namespace COM3D2.MoreFacilities.Plugin
{
    [BepInPlugin("org.guest4168.plugins.morefacilitiesplugin", "More Facilities Plug-In", "1.0.3.0")]
    public class MoreFacilities : BaseUnityPlugin
    {
        #region Main Code

        // Constants
        public const int MaxFacilities = 60;
        private const int scrollJump = 1;

        // Variables for "Window Facility Details"
        private int wfd_scrollPosition = 0;
        private Vector3 wfd_originalPosition = new Vector3(-9001f, -9001f, -9001f);
        private GameObject windowFacilityDetails = null;
        private GameObject listToFakeScroll = null;
        private bool wfd_previousActiveState = false;

        // Variables for "Window Facility List"
        private int wfl_scrollPosition = 0;
        private Vector3 wfl_originalPosition = new Vector3(-9001f, -9001f, -9001f);
        private GameObject windowFacilityList = null;
        private GameObject parentButton = null;
        private bool wfl_previousActiveState = false;

        private void Start()
        {
            // Attempt to find the GameObjects at the start
            windowFacilityDetails = GameObject.Find("Window Facility Details");
            windowFacilityList = GameObject.Find("Window Facility List");
        }

        private void Update()
        {
            HandleWindowFacilityDetails();
            HandleWindowFacilityList();
        }

        private void HandleWindowFacilityDetails()
        {
            // Try to find the window if not already found
            if (windowFacilityDetails == null)
            {
                windowFacilityDetails = GameObject.Find("Window Facility Details");
                if (windowFacilityDetails == null)
                    return; // Still not found, exit early
            }

            bool currentActiveState = windowFacilityDetails.activeInHierarchy;
            if (currentActiveState != wfd_previousActiveState)
            {
                wfd_previousActiveState = currentActiveState;
                if (currentActiveState)
                {
                    // Window became active
                    listToFakeScroll = GameObject.Find("Parent Facility Button");
                    wfd_originalPosition = new Vector3(-9001f, -9001f, -9001f);
                    wfd_scrollPosition = 0;
                }
                else
                {
                    // Window became inactive
                    wfd_scrollPosition = -1;
                    listToFakeScroll = null;
                }
            }

            if (currentActiveState && listToFakeScroll != null)
            {
                fakeScroll(listToFakeScroll, 0, ref wfd_scrollPosition, 4, 3, scrollJump, ref wfd_originalPosition, true);
            }
        }

        private void HandleWindowFacilityList()
        {
            // Try to find the window if not already found
            if (windowFacilityList == null)
            {
                windowFacilityList = GameObject.Find("Window Facility List");
                if (windowFacilityList == null)
                    return; // Still not found, exit early
            }

            bool currentActiveState = windowFacilityList.activeInHierarchy;
            if (currentActiveState != wfl_previousActiveState)
            {
                wfl_previousActiveState = currentActiveState;
                if (currentActiveState)
                {
                    // Window became active
                    if (windowFacilityList.transform.childCount > 1)
                    {
                        parentButton = windowFacilityList.transform.GetChild(1).gameObject;
                        wfl_originalPosition = new Vector3(-9001f, -9001f, -9001f);
                        wfl_scrollPosition = 0;
                    }
                }
                else
                {
                    // Window became inactive
                    wfl_scrollPosition = -1;
                    parentButton = null;
                }
            }

            if (currentActiveState && parentButton != null)
            {
                fakeScroll(parentButton, 1, ref wfl_scrollPosition, 12, 1, scrollJump, ref wfl_originalPosition, false);
            }
        }

        private void fakeScroll(GameObject listToFakeScroll, int mode, ref int scrollPosition, int visibleRows, int numOfColumns, int scrollRowJump, ref Vector3 originalPosition, bool allowArrows)
        {
            // Check that the item you want to scroll exists
            if (listToFakeScroll != null)
            {
                int listCount = listToFakeScroll.transform.childCount - 1; // There's usually a dummy item in these lists at the beginning
                int maxScrollPosition = (int)Math.Ceiling((decimal)((listCount - 1) / (numOfColumns * scrollRowJump)));

                int scroll = 0;
                int oldScrollPosition = scrollPosition;

                // Initialize scrollPosition if necessary
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }

                // Capture Arrow Keys and Mouse Wheel Scroll
                if (Input.GetAxis("Mouse ScrollWheel") != 0f || (allowArrows && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))))
                {
                    // Scroll direction
                    scroll = (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.DownArrow)) ? 1 : -1;

                    // Store the original position of the grid for when we leave and have to come back here
                    if (originalPosition.x == (-9001f))
                    {
                        originalPosition = new Vector3(listToFakeScroll.transform.position.x,
                                                       listToFakeScroll.transform.position.y,
                                                       listToFakeScroll.transform.position.z);
                    }
                }

                // Do not want to scroll too far
                scrollPosition += scroll;
                scrollPosition = Math.Max(scrollPosition, 0);
                scrollPosition = Math.Min(scrollPosition, maxScrollPosition);

                // Loop the children to set visibility, regardless of if scrollPosition changed
                int minVisible = (scrollPosition * (numOfColumns * scrollRowJump)) + 1;
                int maxVisible = minVisible + ((numOfColumns * visibleRows) - 1);

                for (int i = 1; i < listToFakeScroll.transform.childCount; i++)
                {
                    if (minVisible <= i && i <= maxVisible)
                    {
                        // Positioning mode or activate mode
                        switch (mode)
                        {
                            case 0:
                                listToFakeScroll.transform.GetChild(i).gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                                break;
                            case 1:
                                listToFakeScroll.transform.GetChild(i).gameObject.SetActive(true);
                                break;
                        }
                    }
                    else
                    {
                        // Positioning mode or activate mode
                        switch (mode)
                        {
                            case 0:
                                listToFakeScroll.transform.GetChild(i).gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
                                break;
                            case 1:
                                listToFakeScroll.transform.GetChild(i).gameObject.SetActive(false);
                                break;
                        }
                    }
                }

                if (mode == 0)
                {
                    // Now see if we actually changed, and move the container grid object
                    if (originalPosition.x != (-9001f) && scrollPosition != oldScrollPosition)
                    {
                        float diff = listToFakeScroll.transform.GetChild(1).transform.position.y - listToFakeScroll.transform.GetChild(1 + (numOfColumns * scrollRowJump)).transform.position.y;
                        listToFakeScroll.transform.position = new Vector3(originalPosition.x,
                                                                          originalPosition.y + (scrollPosition * scrollRowJump * diff),
                                                                          originalPosition.z);
                    }
                }
            }
            else
            {
                // Reset
                scrollPosition = -1;
            }
        }

        #endregion

        #region Harmony Patching
        private GameObject managerObject;
        public void Awake()
        {
            Debug.Log("MoreFacilities: Awake");
            DontDestroyOnLoad(this);
            this.managerObject = new GameObject("moreFacilitiesManager");
            DontDestroyOnLoad(this.managerObject);
            this.managerObject.AddComponent<MoreFacilitiesManager>().Initialize();
        }
        #endregion
    }
}
