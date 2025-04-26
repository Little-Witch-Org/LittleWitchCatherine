using System;
using System.Linq;

namespace _Scripts.NarrativeAndCutscenes.Checkpoints
{
    public abstract class Checkpoint
    {
        public int CheckpointID { get; protected set; }
        
        //get ID from name
        protected Checkpoint()
        {
            string className = GetType().Name;
            string numberStr = new string(className.Where(char.IsDigit).ToArray());
        
            // Пытаемся преобразовать в число
            if (int.TryParse(numberStr, out int id))
            {
                CheckpointID = id;
            }
            else
            {
                throw new ArgumentException($"Checkpoint class name must contain a number! Example: 'Checkpoint1'. Wrong name: {className}");
            }
        }

        public virtual void Activate()
        {
            
        }
    }
}