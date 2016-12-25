﻿using System;

namespace Kilt.ExternLibs.KSerializer {
    /// <summary>
    /// The given property or field annotated with [IgnoreAttribute] will not be serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class IgnoreAttribute : Attribute {
    }
}