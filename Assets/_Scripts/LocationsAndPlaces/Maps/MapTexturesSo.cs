using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.LocationsAndPlaces.Maps
{
    [CreateAssetMenu(fileName = "MapTexturesSo", menuName = "ScriptableObjects/MapTexturesSo")]
    public class MapTexturesSo:ScriptableObject
    {

        public Sprite ground;
        public List<Sprite> mapObjectsSprites;
    }
}