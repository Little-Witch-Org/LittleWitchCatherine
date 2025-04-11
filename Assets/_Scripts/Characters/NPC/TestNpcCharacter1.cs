using UnityEngine;

namespace _Scripts.Characters.NPC
{/// <summary>
 /// Used as container for dialogue component and quest point //todo rename to quest component?
 /// </summary>
    public class TestNpcCharacter1 : NpcCharAbstract
    {
        /*public static TestNpcCharacter1 Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }*/

        //TODO HANDLE NPC APPEAR IN CURRENT PLACE (like trigger spawner) (add places/locations fileds, on/off for model(sprite/collider) but script go is on allways to track game states? npc manager with list of npc's (prefabs) ? methods to move to locations (some logic?)? sprites for emotions etc..
        //add time schedule and locations (in future, now static). all fadein fade out to sprite if we are in empty room and character appears by timing. 
        
    }
}