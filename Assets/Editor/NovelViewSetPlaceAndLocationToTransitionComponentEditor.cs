using System;
using _Scripts.Components.TransitionComponents;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom (inspector) editor for location transition component
/// </summary>

[CustomEditor(typeof(NovelViewSetPlaceAndLocationToTransitionComponent))]
public class NovelViewSetPlaceAndLocationToTransitionComponentEditor : UnityEditor.Editor
{
    private SerializedProperty locationTypeProperty;
    private SerializedProperty selectedPlaceProperty;

    private void OnEnable()
    {
        // Initialize properties
        locationTypeProperty = serializedObject.FindProperty("locationType");
        selectedPlaceProperty = serializedObject.FindProperty("selectedPlace");
    }

    public override void OnInspectorGUI()
    {
        // Start modifying the object
        serializedObject.Update();

        // Save the previous value of LocationType
        var previousLocationType = (NovelViewSetPlaceAndLocationToTransitionComponent.LocationType)locationTypeProperty.enumValueIndex;

        // Display the dropdown menu for the location type
        EditorGUILayout.PropertyField(locationTypeProperty, new GUIContent("Location Type"));

        // Get the currently selected location type
        var currentLocationType = (NovelViewSetPlaceAndLocationToTransitionComponent.LocationType)locationTypeProperty.enumValueIndex;

        // If LocationType has changed, reset selectedPlace
        if (currentLocationType != previousLocationType)
        {
            ResetSelectedPlace(currentLocationType);
        }

        // Get the corresponding enum for the rooms
        if (((NovelViewSetPlaceAndLocationToTransitionComponent)target).locationTypeEnums.TryGetValue(currentLocationType, out Type roomEnumType))
        {
            // Get the room names
            string[] roomNames = Enum.GetNames(roomEnumType);

            // Get the index of the current selectedPlace
            int selectedIndex = Array.IndexOf(roomNames, selectedPlaceProperty.stringValue);

            // If selectedPlace is not found, reset it to the first value
            if (selectedIndex == -1)
            {
                selectedIndex = 0;
                selectedPlaceProperty.stringValue = roomNames[selectedIndex];
            }

            // Display the dropdown menu for selecting the room
            selectedIndex = EditorGUILayout.Popup("Selected Place", selectedIndex, roomNames);
            selectedPlaceProperty.stringValue = roomNames[selectedIndex];
        }
        else
        {
            EditorGUILayout.HelpBox("No room enum found for the selected location type.", MessageType.Warning);
        }

        // Apply changes
        serializedObject.ApplyModifiedProperties();
    }

    private void ResetSelectedPlace(NovelViewSetPlaceAndLocationToTransitionComponent.LocationType locationType)
    {
        // Get the corresponding enum for the rooms
        if (((NovelViewSetPlaceAndLocationToTransitionComponent)target).locationTypeEnums.TryGetValue(locationType, out Type roomEnumType))
        {
            // Get the room names
            string[] roomNames = Enum.GetNames(roomEnumType);

            // Set selectedPlace to the first value
            if (roomNames.Length > 0)
            {
                selectedPlaceProperty.stringValue = roomNames[0];
            }
        }
    }
}