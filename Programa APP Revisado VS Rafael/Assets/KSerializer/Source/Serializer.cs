﻿#if (UNITY_WINRT || UNITY_WP_8_1) && !UNITY_EDITOR && !UNITY_WP8
#define RT_ENABLED
#endif

using System;
using System.Collections.Generic;
using Kilt.ExternLibs.KSerializer.Internal;
using System.Reflection;

namespace Kilt.ExternLibs.KSerializer {
    public class Serializer {

        #region Keys
        private static HashSet<string> _reservedKeywords;
        static Serializer() {
            _reservedKeywords = new HashSet<string> {
                Key_ObjectReference,
                Key_ObjectDefinition,
                Key_InstanceType,
                Key_Version,
                Key_Content
            };
        }
        /// <summary>
        /// Returns true if the given key is a special keyword that full serializer uses to
        /// add additional metadata on top of the emitted JSON.
        /// </summary>
        public static bool IsReservedKeyword(string key) {
            return _reservedKeywords.Contains(key);
        }

        /// <summary>
        /// This is an object reference in part of a cyclic graph.
        /// </summary>
        private const string Key_ObjectReference = "$ref";

        /// <summary>
        /// This is an object definition, as part of a cyclic graph.
        /// </summary>
        private const string Key_ObjectDefinition = "$id";

        /// <summary>
        /// This specifies the actual type of an object (the instance type was different from
        /// the field type).
        /// </summary>
        private const string Key_InstanceType = "$type";

        /// <summary>
        /// The version string for the serialized data.
        /// </summary>
        private const string Key_Version = "$version";

        /// <summary>
        /// If we have to add metadata but the original serialized state was not a dictionary,
        /// then this will contain the original data.
        /// </summary>
        private const string Key_Content = "$content";

        private static bool IsObjectReference(Data data) {
            if (data.IsDictionary == false) return false;
            return data.AsDictionary.ContainsKey(Key_ObjectReference);
        }
        private static bool IsObjectDefinition(Data data) {
            if (data.IsDictionary == false) return false;
            return data.AsDictionary.ContainsKey(Key_ObjectDefinition);
        }
        private static bool IsVersioned(Data data) {
            if (data.IsDictionary == false) return false;
            return data.AsDictionary.ContainsKey(Key_Version);
        }
        private static bool IsTypeSpecified(Data data) {
            if (data.IsDictionary == false) return false;
            return data.AsDictionary.ContainsKey(Key_InstanceType);
        }
        private static bool IsWrappedData(Data data) {
            if (data.IsDictionary == false) return false;
            return data.AsDictionary.ContainsKey(Key_Content);
        }

        /// <summary>
        /// Strips all deserialization metadata from the object, like $type and $content fields.
        /// </summary>
        /// <remarks>After making this call, you will *not* be able to deserialize the same object instance. The metadata is
        /// strictly necessary for deserialization!</remarks>
        public static void StripDeserializationMetadata(ref Data data) {
            if (data.IsDictionary && data.AsDictionary.ContainsKey(Key_Content)) {
                data = data.AsDictionary[Key_Content];
            }

            if (data.IsDictionary) {
                var dict = data.AsDictionary;
                dict.Remove(Key_ObjectReference);
                dict.Remove(Key_ObjectDefinition);
                dict.Remove(Key_InstanceType);
                dict.Remove(Key_Version);
            }
        }

        /// <summary>
        /// This function converts legacy serialization data into the new format, so that
        /// the import process can be unified and ignore the old format.
        /// </summary>
        private void ConvertLegacyData(ref Data data) {
            if (data.IsDictionary == false) return;

            var dict = data.AsDictionary;

            // fast-exit: metadata never had more than two items
            if (dict.Count > 2) return;

            // Key strings used in the legacy system
            string referenceIdString = "ReferenceId";
            string sourceIdString = "SourceId";
            string sourceDataString = "Data";
            string typeString = "Type";
            string typeDataString = "Data";

            // type specifier
            if (dict.Count == 2 && dict.ContainsKey(typeString) && dict.ContainsKey(typeDataString)) {
                data = dict[typeDataString];
                EnsureDictionary(ref data);
                ConvertLegacyData(ref data);

                data.AsDictionary[Key_InstanceType] = dict[typeString];
            }

            // object definition
            else if (dict.Count == 2 && dict.ContainsKey(sourceIdString) && dict.ContainsKey(sourceDataString)) {
                data = dict[sourceDataString];
                EnsureDictionary(ref data);
                ConvertLegacyData(ref data);

                data.AsDictionary[Key_ObjectDefinition] = dict[sourceIdString];
            }

            // object reference
            else if (dict.Count == 1 && dict.ContainsKey(referenceIdString)) {
                data = Data.CreateDictionary(Config);
                data.AsDictionary[Key_ObjectReference] = dict[referenceIdString];
            }
        }
        #endregion

        #region Utility Methods

        private static void Invoke_DefaultCallback<T>(List<System.Reflection.MethodInfo> defaultCallbackMethods, object instance) where T : Attribute
        {
            foreach (System.Reflection.MethodInfo v_methodInfo in defaultCallbackMethods)
            {
                if (PortableReflection.GetAttribute<T>(v_methodInfo) != null)
                {
                    try
                    {
                        var v_parameters = v_methodInfo.GetParameters();
                        if (v_parameters.Length == 0)
                        {
                            v_methodInfo.Invoke(instance, null);
                        }
                        //SerializationContext will be null
                        else if (v_parameters.Length == 1)
                        {
                            v_methodInfo.Invoke(instance, new object[] { null });
                        }
                    }
                    catch { }
                    break;
                }
            }
        }

        private static void Invoke_OnBeforeSerialize(List<System.Reflection.MethodInfo> defaultCallbackMethods, List<ObjectProcessor> processors, Type storageType, object instance) {
            for (int i = 0; i < processors.Count; ++i) {
                processors[i].OnBeforeSerialize(storageType, instance);
            }
            Invoke_DefaultCallback<System.Runtime.Serialization.OnSerializingAttribute>(defaultCallbackMethods, instance);
        }
        private static void Invoke_OnAfterSerialize(List<System.Reflection.MethodInfo> defaultCallbackMethods, List<ObjectProcessor> processors, Type storageType, object instance, ref Data data) {
            // We run the after calls in reverse order; this significantly reduces the interaction burden between
            // multiple processors - it makes each one much more independent and ignorant of the other ones.

            for (int i = processors.Count - 1; i >= 0; --i) {
                processors[i].OnAfterSerialize(storageType, instance, ref data);
            }
            Invoke_DefaultCallback<System.Runtime.Serialization.OnSerializedAttribute>(defaultCallbackMethods, instance);
        }
        private static void Invoke_OnBeforeDeserialize(List<ObjectProcessor> processors, Type storageType, ref Data data) {
            for (int i = 0; i < processors.Count; ++i) {
                processors[i].OnBeforeDeserialize(storageType, ref data);
            }
        }
        private static void Invoke_OnBeforeDeserializeAfterInstanceCreation(List<System.Reflection.MethodInfo> defaultCallbackMethods, List<ObjectProcessor> processors, Type storageType, object instance, ref Data data) {
            for (int i = 0; i < processors.Count; ++i) {
                processors[i].OnBeforeDeserializeAfterInstanceCreation(storageType, instance, ref data);
            }
            Invoke_DefaultCallback<System.Runtime.Serialization.OnDeserializingAttribute>(defaultCallbackMethods, instance);
        }
        private static void Invoke_OnAfterDeserialize(List<System.Reflection.MethodInfo> defaultCallbackMethods, List<ObjectProcessor> processors, Type storageType, object instance) {

            for (int i = processors.Count - 1; i >= 0; --i) {
                processors[i].OnAfterDeserialize(storageType, instance);
            }
            Invoke_DefaultCallback<System.Runtime.Serialization.OnDeserializedAttribute>(defaultCallbackMethods, instance);
        }
        #endregion

        /// <summary>
        /// Ensures that the data is a dictionary. If it is not, then it is wrapped inside of one.
        /// </summary>
        private void EnsureDictionary(ref Data data) {
            if (data.IsDictionary == false) {
                var dict = Data.CreateDictionary(Config);
                dict.AsDictionary[Key_Content] = data;
                data = dict;
            }
        }

        /// <summary>
        /// This manages instance writing so that we do not write unnecessary $id fields. We
        /// only need to write out an $id field when there is a corresponding $ref field. This is able
        /// to write $id references lazily because the Data instance is not actually written out to text
        /// until we have entirely finished serializing it.
        /// </summary>
        internal class LazyCycleDefinitionWriter {
            private Dictionary<int, Dictionary<string, Data>> _definitions = new Dictionary<int, Dictionary<string, Data>>();
            private HashSet<int> _references = new HashSet<int>();

            public void WriteDefinition(int id, Dictionary<string, Data> dict) {
                if (_references.Contains(id)) {
                    dict[Key_ObjectDefinition] = new Data(id.ToString());
                }

                else {
                    _definitions[id] = dict;
                }
            }

            public void WriteReference(int id, Dictionary<string, Data> dict) {
                // Write the actual definition if necessary
                if (_definitions.ContainsKey(id)) {
                    _definitions[id][Key_ObjectDefinition] = new Data(id.ToString());
                    _definitions.Remove(id);
                }
                else {
                    _references.Add(id);
                }

                // Write the reference
                dict[Key_ObjectReference] = new Data(id.ToString());
            }

            public void Clear() {
                _definitions.Clear();
            }
        }

        /// <summary>
        /// A cache from type to it's converter.
        /// </summary>
        private Dictionary<Type, BaseConverter> _cachedConverters;

        /// <summary>
        /// A cache from type to the set of processors that are interested in it.
        /// </summary>
        private Dictionary<Type, List<ObjectProcessor>> _cachedProcessors;

        /// <summary>
        /// A cache from type to the set of processors that are interested in it.
        /// </summary>
        private Dictionary<Type, List<System.Reflection.MethodInfo>> _cachedDefaultSerializationCallbacks;

        /// <summary>
        /// Converters that can be used for type registration.
        /// </summary>
        private readonly List<Converter> _availableConverters;

        /// <summary>
        /// Direct converters (optimized _converters). We use these so we don't have to
        /// perform a scan through every item in _converters and can instead just do an O(1)
        /// lookup. This is potentially important to perf when there are a ton of direct
        /// converters.
        /// </summary>
        private readonly Dictionary<Type, DirectConverter> _availableDirectConverters;

        /// <summary>
        /// Processors that are available.
        /// </summary>
        private readonly List<ObjectProcessor> _processors;

        /// <summary>
        /// Reference manager for cycle detection.
        /// </summary>
        private readonly CyclicReferenceManager _references;
        private readonly LazyCycleDefinitionWriter _lazyReferenceWriter;

        private readonly Config _config = null;
        public Config Config
        {
            get
            {
                return _config;
            }
        }

        public Serializer(Config p_config = null) {
            _config = p_config == null? Config.DefaultConfig : p_config;
            _cachedConverters = new Dictionary<Type, BaseConverter>();
            _cachedProcessors = new Dictionary<Type, List<ObjectProcessor>>();
            _cachedDefaultSerializationCallbacks = new Dictionary<Type, List<System.Reflection.MethodInfo>>();

            _references = new CyclicReferenceManager();
            _lazyReferenceWriter = new LazyCycleDefinitionWriter();

            // note: The order here is important. Items at the beginning of this
            //       list will be used before converters at the end. Converters
            //       added via AddConverter() are added to the front of the list.
            _availableConverters = new List<Converter> {
                new NullableConverter { Serializer = this },
                new GuidConverter { Serializer = this },
                new TypeConverter { Serializer = this },
                new DateConverter { Serializer = this },
                new EnumConverter { Serializer = this },
                new PrimitiveConverter { Serializer = this },
                new ArrayConverter { Serializer = this },
                new DictionaryConverter { Serializer = this },
                new IEnumerableConverter { Serializer = this },
                new KeyValuePairConverter { Serializer = this },
                new WeakReferenceConverter { Serializer = this },
                new ReflectedConverter { Serializer = this }
            };
            _availableDirectConverters = new Dictionary<Type, DirectConverter>();

            _processors = new List<ObjectProcessor>() {
                new SerializationCallbackProcessor()
            };

            Context = new Context();

            // Register the converters from the registrar
            foreach (var converterType in ConverterRegistrar.Converters) {
                AddConverter((BaseConverter)Activator.CreateInstance(converterType));
            }
        }

        /// <summary>
        /// A context object that Converters can use to customize how they operate.
        /// </summary>
        public Context Context;

        /// <summary>
        /// Add a new processor to the serializer. Multiple processors can run at the same time in the
        /// same order they were added in.
        /// </summary>
        /// <param name="processor">The processor to add.</param>
        public void AddProcessor(ObjectProcessor processor) {
            _processors.Add(processor);

            // We need to reset our cached processor set, as it could be invalid with the new
            // processor. Ideally, _cachedProcessors should be empty (as the user should fully setup
            // the serializer before actually using it), but there is no guarantee.
            _cachedProcessors = new Dictionary<Type, List<ObjectProcessor>>();
        }

        private List<System.Reflection.MethodInfo> GetDefaultSerializationCallbackMethods(Type type)
        {
            List<System.Reflection.MethodInfo>  v_returnList;
            if (!_cachedDefaultSerializationCallbacks.TryGetValue(type, out v_returnList))
            {
                List<System.Reflection.MethodInfo> v_methodList = new List<System.Reflection.MethodInfo>();
                System.Type v_type = type;
                while (v_type != null)
                {
                    v_methodList.AddRange(v_type.GetDeclaredMethods());
#if RT_ENABLED
                    v_type = v_type.GetTypeInfo().BaseType;
#else
                    v_type = v_type.BaseType;
#endif
                }
                v_returnList = new List<System.Reflection.MethodInfo>();
                foreach (System.Reflection.MethodInfo v_methodInfo in v_methodList)
                {
                    if ((PortableReflection.GetAttribute<System.Runtime.Serialization.OnDeserializedAttribute>(v_methodInfo) != null) ||
                        (PortableReflection.GetAttribute<System.Runtime.Serialization.OnDeserializingAttribute>(v_methodInfo) != null) ||
                        (PortableReflection.GetAttribute<System.Runtime.Serialization.OnSerializedAttribute>(v_methodInfo) != null) ||
                        (PortableReflection.GetAttribute<System.Runtime.Serialization.OnSerializingAttribute>(v_methodInfo) != null))
                    {
                        v_returnList.Add(v_methodInfo);
                    }
                }
                _cachedDefaultSerializationCallbacks[type] = v_returnList;
            }
            return v_returnList;
        }

        /// <summary>
        /// Fetches all of the processors for the given type.
        /// </summary>
        private List<ObjectProcessor> GetProcessors(Type type) {
            List<ObjectProcessor> processors;

            // Check to see if the user has defined a custom processor for the type. If they
            // have, then we don't need to scan through all of the processor to check which
            // one can process the type; instead, we directly use the specified processor.
            var attr = PortableReflection.GetAttribute<ObjectAttribute>(type);
            if (attr != null && attr.Processor != null) {
                var processor = (ObjectProcessor)Activator.CreateInstance(attr.Processor);
                processors = new List<ObjectProcessor>();
                processors.Add(processor);
                _cachedProcessors[type] = processors;
            }

            else if (_cachedProcessors.TryGetValue(type, out processors) == false) {
                processors = new List<ObjectProcessor>();

                for (int i = 0; i < _processors.Count; ++i) {
                    var processor = _processors[i];
                    if (processor.CanProcess(type)) {
                        processors.Add(processor);
                    }
                }

                _cachedProcessors[type] = processors;
            }

            return processors;
        }


        /// <summary>
        /// Adds a new converter that can be used to customize how an object is serialized and
        /// deserialized.
        /// </summary>
        public void AddConverter(BaseConverter converter) {
            if (converter.Serializer != null) {
                throw new InvalidOperationException("Cannot add a single converter instance to " +
                    "multiple Converters -- please construct a new instance for " + converter);
            }

            // TODO: wrap inside of a ConverterManager so we can control _converters and _cachedConverters lifetime
            if (converter is DirectConverter) {
                var directConverter = (DirectConverter)converter;
                _availableDirectConverters[directConverter.ModelType] = directConverter;
            }
            else if (converter is Converter) {
                _availableConverters.Insert(0, (Converter)converter);
            }
            else {
                throw new InvalidOperationException("Unable to add converter " + converter +
                    "; the type association strategy is unknown. Please use either " +
                    "DirectConverter or Converter as your base type.");
            }

            converter.Serializer = this;

            // We need to reset our cached converter set, as it could be invalid with the new
            // converter. Ideally, _cachedConverters should be empty (as the user should fully setup
            // the serializer before actually using it), but there is no guarantee.
            _cachedConverters = new Dictionary<Type, BaseConverter>();
        }

        /// <summary>
        /// Fetches a converter that can serialize/deserialize the given type.
        /// </summary>
        private BaseConverter GetConverter(Type type) {
            BaseConverter converter;

            // Check to see if the user has defined a custom converter for the type. If they
            // have, then we don't need to scan through all of the converters to check which
            // one can process the type; instead, we directly use the specified converter.
            var attr = PortableReflection.GetAttribute<ObjectAttribute>(type);
            if (attr != null && attr.Converter != null) {
                converter = (BaseConverter)Activator.CreateInstance(attr.Converter);
                converter.Serializer = this;
                _cachedConverters[type] = converter;
            }

            // There is no specific converter specified; try all of the general ones to see
            // which ones matches.
            else {
                if (_cachedConverters.TryGetValue(type, out converter) == false) {
                    if (_availableDirectConverters.ContainsKey(type)) {
                        converter = _availableDirectConverters[type];
                        _cachedConverters[type] = converter;
                    }
                    else {
                        for (int i = 0; i < _availableConverters.Count; ++i) {
                            if (_availableConverters[i].CanProcess(type)) {
                                converter = _availableConverters[i];
                                _cachedConverters[type] = converter;
                                break;
                            }
                        }
                    }
                }
            }

            if (converter == null) {
                throw new InvalidOperationException("Internal error -- could not find a converter for " + type);
            }
            return converter;
        }

        /// <summary>
        /// Helper method that simply forwards the call to TrySerialize(typeof(T), instance, out data);
        /// </summary>
        public Result TrySerialize<T>(T instance, out Data data) {
            return TrySerialize(typeof(T), instance, out data);
        }

        /// <summary>
        /// Generic wrapper around TryDeserialize that simply forwards the call.
        /// </summary>
        public Result TryDeserialize<T>(Data data, ref T instance) {
            object boxed = instance;
            var fail = TryDeserialize(data, typeof(T), ref boxed);
            if (fail.Succeeded) {
                instance = (T)boxed;
            }
            return fail;
        }

        /// <summary>
        /// Serialize the given value.
        /// </summary>
        /// <param name="storageType">The type of field/property that stores the object instance. This is
        /// important particularly for inheritance, as a field storing an IInterface instance
        /// should have type information included.</param>
        /// <param name="instance">The actual object instance to serialize.</param>
        /// <param name="data">The serialized state of the object.</param>
        /// <returns>If serialization was successful.</returns>
        public Result TrySerialize(Type storageType, object instance, out Data data) {
            var processors = GetProcessors(storageType);
            var v_defaultCallbacks = GetDefaultSerializationCallbackMethods(instance != null? instance.GetType() : storageType);
            Invoke_OnBeforeSerialize(v_defaultCallbacks, processors, storageType, instance);

            // We always serialize null directly as null
            if (ReferenceEquals(instance, null)) {
                data = new Data();
                Invoke_OnAfterSerialize(v_defaultCallbacks, processors, storageType, instance, ref data);
                return Result.Success;
            }

            var result = InternalSerialize_1_ProcessCycles(storageType, instance, out data);
            Invoke_OnAfterSerialize(v_defaultCallbacks, processors, storageType, instance, ref data);
            return result;
        }

        private Result InternalSerialize_1_ProcessCycles(Type storageType, object instance, out Data data) {
            // We have an object definition to serialize.
            try {
                // Note that we enter the reference group at the beginning of serialization so that we support
                // references that are at equal serialization levels, not just nested serialization levels, within
                // the given subobject. A prime example is serialization a list of references.
                _references.Enter();

                // This type does not need cycle support.
                if (GetConverter(instance.GetType()).RequestCycleSupport(instance.GetType()) == false) {
                    return InternalSerialize_2_Inheritance(storageType, instance, out data);
                }

                // We've already serialized this object instance (or it is pending higher up on the call stack).
                // Just serialize a reference to it to escape the cycle.
                // 
                // note: We serialize the int as a string to so that we don't lose any information
                //       in a conversion to/from double.
                if (_references.IsReference(instance)) {
                    data = Data.CreateDictionary(Config);
                    _lazyReferenceWriter.WriteReference(_references.GetReferenceId(instance), data.AsDictionary);
                    return Result.Success;
                }

                // Mark inside the object graph that we've serialized the instance. We do this *before*
                // serialization so that if we get back into this function recursively, it'll already
                // be marked and we can handle the cycle properly without going into an infinite loop.
                _references.MarkSerialized(instance);

                // We've created the cycle metadata, so we can now serialize the actual object.
                // InternalSerialize will handle inheritance correctly for us.
                var result = InternalSerialize_2_Inheritance(storageType, instance, out data);
                if (result.Failed) return result;

                EnsureDictionary(ref data);
                _lazyReferenceWriter.WriteDefinition(_references.GetReferenceId(instance), data.AsDictionary);

                return result;
            }
            finally {
                if (_references.Exit()) {
                    _lazyReferenceWriter.Clear();
                }
            }
        }
        private Result InternalSerialize_2_Inheritance(Type storageType, object instance, out Data data) {
            // Serialize the actual object with the field type being the same as the object
            // type so that we won't go into an infinite loop.
            var serializeResult = InternalSerialize_3_ProcessVersioning(instance, out data);
            if (serializeResult.Failed) return serializeResult;

            // Do we need to add type information? If the field type and the instance type are different
            // then we will not be able to recover the correct instance type from the field type when
            // we deserialize the object.
            //
            // Note: We allow converters to request that we do *not* add type information.
            if (Config.TypeWriterOption != Config.TypeWriterEnum.Never)
            {
                if ((Config.TypeWriterOption == Config.TypeWriterEnum.Always) || 
                    (storageType != instance.GetType() && GetConverter(storageType).RequestInheritanceSupport(storageType)))
                {
                    System.Type v_instanceType = GetSerializationType(instance);
                    EnsureDictionary(ref data);
                    if (v_instanceType != null)
                    {
                        // Add the inheritance metadata
                        data.AsDictionary[Key_InstanceType] = new Data(RemoveAssemblyDetails(v_instanceType.AssemblyQualifiedName));
                    }
                }
            }

            return serializeResult;
        }

        private static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            // loop through the type name and filter out qualified assembly details from nested type names
            bool writingAssemblyName = false;
            bool skippingAssemblyDetails = false;
            for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
            {
                char current = fullyQualifiedTypeName[i];
                switch (current)
                {
                    case '[':
                        writingAssemblyName = false;
                        skippingAssemblyDetails = false;
                        builder.Append(current);
                        break;
                    case ']':
                        writingAssemblyName = false;
                        skippingAssemblyDetails = false;
                        builder.Append(current);
                        break;
                    case ',':
                        if (!writingAssemblyName)
                        {
                            writingAssemblyName = true;
                            builder.Append(current);
                        }
                        else
                        {
                            skippingAssemblyDetails = true;
                        }
                        break;
                    default:
                        if (!skippingAssemblyDetails)
                            builder.Append(current);
                        break;
                }
            }

            return builder.ToString();
        }

        //Get TypeFallback if the attribute is defined
        System.Type GetSerializationType(object p_instance)
        {
            System.Type v_instanceType = p_instance != null? p_instance.GetType() : null;
            if (p_instance != null)
            {
                try
                {
#if RT_ENABLED
                    var v_customAttrs = v_instanceType.GetTypeInfo().GetCustomAttributes(typeof(TypeFallback), true);
                    List<object> v_list = new List<object>();
                    foreach (var v_attr in v_customAttrs)
                    {
                        v_list.Add(v_attr);
                    }
                    object[] v_attrs = v_list.ToArray();
#else
                    object[] v_attrs = v_instanceType.GetCustomAttributes(typeof(TypeFallback), true);
#endif
                    if (v_attrs != null && v_attrs.Length > 0)
                    {
                        TypeFallback v_attrFallback = v_attrs[0] as TypeFallback;
                        v_instanceType = v_attrFallback.TypeFallBack;
                    }
                }
                catch { }
            }
            return v_instanceType;
        }

        private Result InternalSerialize_3_ProcessVersioning(object instance, out Data data) {
            // note: We do not have to take a Type parameter here, since at this point in the serialization
            //       algorithm inheritance has *always* been handled. If we took a type parameter, it will
            //       *always* be equal to instance.GetType(), so why bother taking the parameter?

            // Check to see if there is versioning information for this type. If so, then we need to serialize it.
            Option<VersionedType> optionalVersionedType = VersionManager.GetVersionedType(instance.GetType());
            if (optionalVersionedType.HasValue) {
                VersionedType versionedType = optionalVersionedType.Value;

                // Serialize the actual object content; we'll just wrap it with versioning metadata here.
                var result = InternalSerialize_4_Converter(instance, out data);
                if (result.Failed) return result;

                // Add the versioning information
                EnsureDictionary(ref data);
                data.AsDictionary[Key_Version] = new Data(versionedType.VersionString);

                return result;
            }

            // This type has no versioning information -- directly serialize it using the selected converter.
            return InternalSerialize_4_Converter(instance, out data);
        }
        private Result InternalSerialize_4_Converter(object instance, out Data data) {
            var instanceType = instance.GetType();
            return GetConverter(instanceType).TrySerialize(instance, out data, instanceType);
        }

        /// <summary>
        /// Attempts to deserialize a value from a serialized state.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="storageType"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public Result TryDeserialize(Data data, Type storageType, ref object result) {
            var processors = GetProcessors(storageType);
            var v_defaultCallbacks = new List<MethodInfo>();
            Invoke_OnBeforeDeserialize(processors, storageType, ref data);

            if (data.IsNull) {
                result = null;
                Invoke_OnAfterDeserialize(v_defaultCallbacks, processors, storageType, null);
                return Result.Success;
            }

            // Convert legacy data into modern style data
            ConvertLegacyData(ref data);

            try {
                // We wrap the entire deserialize call in a reference group so that we can properly
                // deserialize a "parallel" set of references, ie, a list of objects that are cyclic
                // with regards to the list
                _references.Enter();

                return InternalDeserialize_1_CycleReference(data, storageType, ref result, processors);
            }
            finally {
                _references.Exit();
                v_defaultCallbacks = GetDefaultSerializationCallbackMethods(result != null ? result.GetType() : storageType);
                Invoke_OnAfterDeserialize(v_defaultCallbacks, processors, storageType, result);
            }
        }

        private Result InternalDeserialize_1_CycleReference(Data data, Type storageType, ref object result, List<ObjectProcessor> processors) {
            // We handle object references first because we could be deserializing a cyclic type that is
            // inherited. If that is the case, then if we handle references after inheritances we will try
            // to create an object instance for an abstract/interface type.

            // While object construction should technically be two-pass, we can do it in
            // one pass because of how serialization happens. We traverse the serialization
            // graph in the same order during serialization and deserialization, so the first
            // time we encounter an object it'll always be the definition. Any times after that
            // it will be a reference. Because of this, if we encounter a reference then we
            // will have *always* already encountered the definition for it.
            if (IsObjectReference(data)) {
                int refId = int.Parse(data.AsDictionary[Key_ObjectReference].AsString);
                result = _references.GetReferenceObject(refId);
                return Result.Success;
            }

            return InternalDeserialize_2_Version(data, storageType, ref result, processors);
        }

        private Result InternalDeserialize_2_Version(Data data, Type storageType, ref object result, List<ObjectProcessor> processors) {
            if (IsVersioned(data)) {
                // data is versioned, but we might not need to do a migration
                string version = data.AsDictionary[Key_Version].AsString;

                Option<VersionedType> versionedType = VersionManager.GetVersionedType(storageType);
                if (versionedType.HasValue &&
                    versionedType.Value.VersionString != version) {

                    // we have to do a migration
                    var deserializeResult = Result.Success;

                    List<VersionedType> path;
                    deserializeResult += VersionManager.GetVersionImportPath(version, versionedType.Value, out path);
                    if (deserializeResult.Failed) return deserializeResult;

                    // deserialize as the original type
                    deserializeResult += InternalDeserialize_3_Inheritance(data, path[0].ModelType, ref result, processors);
                    if (deserializeResult.Failed) return deserializeResult;

                    for (int i = 1; i < path.Count; ++i) {
                        result = path[i].Migrate(result);
                    }

                    return deserializeResult;
                }
            }

            return InternalDeserialize_3_Inheritance(data, storageType, ref result, processors);
        }

        private Result InternalDeserialize_3_Inheritance(Data data, Type storageType, ref object result, List<ObjectProcessor> processors) {
            var deserializeResult = Result.Success;

            Type objectType = storageType;

            // If the serialized state contains type information, then we need to make sure to update our
            // objectType and data to the proper values so that when we construct an object instance later
            // and run deserialization we run it on the proper type.
            if (IsTypeSpecified(data)) {
                Data typeNameData = data.AsDictionary[Key_InstanceType];

                // we wrap everything in a do while false loop so we can break out it
                do {
                    if (typeNameData.IsString == false) {
                        deserializeResult.AddMessage(Key_InstanceType + " value must be a string (in " + data + ")");
                        break;
                    }

                    string typeName = typeNameData.AsString;
                    Type type = TypeLookup.GetType(typeName);
                    if (type == null) {
                        deserializeResult.AddMessage("Unable to locate specified type \"" + typeName + "\"");
                        break;
                    }

                    if (storageType.IsAssignableFrom(type) == false) {
                        deserializeResult.AddMessage("Ignoring type specifier; a field/property of type " + storageType + " cannot hold an instance of " + type);
                        break;
                    }

                    objectType = type;
                } while (false);
            }

            // Construct an object instance if we don't have one already. We also need to construct
            // an instance if the result type is of the wrong type, which may be the case when we
            // have a versioned import graph.
            if (ReferenceEquals(result, null) || result.GetType() != objectType || (objectType != null && objectType.IsAbstract()))
            {
                //Added Try-Catch to prevent desserialization errors when type changed inside deserialized object
                try
                {
                    result = GetConverter(objectType).CreateInstance(data, objectType);
                }
                catch
                {
                    result = null;
                }
            }

            var v_defaultCallbacks = GetDefaultSerializationCallbackMethods(result != null? result.GetType() : storageType);
            // We call OnBeforeDeserializeAfterInstanceCreation here because we still want to invoke the
            // method even if the user passed in an existing instance.
            Invoke_OnBeforeDeserializeAfterInstanceCreation(v_defaultCallbacks, processors, storageType, result, ref data);

            // NOTE: It is critically important that we pass the actual objectType down instead of
            //       using result.GetType() because it is not guaranteed that result.GetType()
            //       will equal objectType, especially because some converters are known to
            //       return dummy values for CreateInstance() (for example, the default behavior
            //       for structs is to just return the type of the struct).

            return deserializeResult += InternalDeserialize_4_Cycles(data, objectType, ref result);
        }

        private Result InternalDeserialize_4_Cycles(Data data, Type resultType, ref object result) {
            if (IsObjectDefinition(data)) {
                // NOTE: object references are handled at stage 1

                // If this is a definition, then we have a serialization invariant that this is the
                // first time we have encountered the object (TODO: verify in the deserialization logic)

                // Since at this stage in the deserialization process we already have access to the
                // object instance, so we just need to sync the object id to the references database
                // so that when we encounter the instance we lookup this same object. We want to do
                // this before actually deserializing the object because when deserializing the object
                // there may be references to itself.

                int sourceId = int.Parse(data.AsDictionary[Key_ObjectDefinition].AsString);
                _references.AddReferenceWithId(sourceId, result);
            }

            // Nothing special, go through the standard deserialization logic.
            return InternalDeserialize_5_Converter(data, resultType, ref result);
        }

        private Result InternalDeserialize_5_Converter(Data data, Type resultType, ref object result) {
            if (IsWrappedData(data)) {
                data = data.AsDictionary[Key_Content];
            }

            return GetConverter(resultType).TryDeserialize(data, ref result, resultType);
        }
    }
}