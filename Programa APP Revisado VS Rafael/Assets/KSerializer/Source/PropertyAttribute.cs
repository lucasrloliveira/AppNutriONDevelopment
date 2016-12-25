using System;

namespace Kilt.ExternLibs.KSerializer {
    /// <summary>
    /// Explicitly mark a property to be serialized. This can also be used to give the name that the
    /// property should use during serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class PropertyAttribute : Attribute {
        /// <summary>
        /// The name of that the property will use in JSON serialization.
        /// </summary>
        public string Name;

        public PropertyAttribute()
            : this(string.Empty) {
        }

        public PropertyAttribute(string name) {
            Name = name;
        }
    }
}