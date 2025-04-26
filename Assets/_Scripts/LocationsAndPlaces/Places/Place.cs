namespace _Scripts.LocationsAndPlaces.Places.CatherineHouse
{
    /// <summary>
    /// Stores info about place (with custom states)
    /// </summary>
    public abstract class Place
    {
        public string PlaceName { get; protected set; }
        public bool IsLocked { get; set; }
    }
}