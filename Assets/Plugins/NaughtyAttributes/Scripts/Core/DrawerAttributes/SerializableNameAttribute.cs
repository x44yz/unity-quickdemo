using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class SerializableNameAttribute : DrawerAttribute
    {
        public string Condition { get; private set; }

        public SerializableNameAttribute(string condition)
        {
            Condition = condition;
        }
    }
}
