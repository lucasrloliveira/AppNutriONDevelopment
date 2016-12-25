﻿using System;
using System.Collections;

namespace Kilt.ExternLibs.KSerializer.Internal {
    public class ReflectedConverter : Converter {

        public override bool CanProcess(Type type) {
            if (type.Resolve().IsArray ||
                typeof(ICollection).IsAssignableFrom(type)) {

                return false;
            }

            return true;
        }

        public override Result TrySerialize(object instance, out Data serialized, Type storageType) {
            serialized = Data.CreateDictionary(Serializer.Config);
            var result = Result.Success;

            MetaType metaType = Serializer.Config.MetaTypeCache.Get(instance.GetType());
            metaType.EmitAotData();

            for (int i = 0; i < metaType.Properties.Length; ++i) {
                MetaProperty property = metaType.Properties[i];
                if (property.CanRead == false) continue;

                Data serializedData;

                var itemResult = Serializer.TrySerialize(property.StorageType, property.Read(instance), out serializedData);
                result.AddMessages(itemResult);
                if (itemResult.Failed) {
                    continue;
                }

                serialized.AsDictionary[property.JsonName] = serializedData;
            }

            return result;
        }

        public override Result TryDeserialize(Data data, ref object instance, Type storageType) {
            var result = Result.Success;

            // Verify that we actually have an Object
            if ((result += CheckType(data, DataType.Object)).Failed) {
                return result;
            }

            MetaType metaType = Serializer.Config.MetaTypeCache.Get(storageType);
            metaType.EmitAotData();

            for (int i = 0; i < metaType.Properties.Length; ++i) {
                MetaProperty property = metaType.Properties[i];
                if (property.CanWrite == false) continue;

                Data propertyData;
                if (data.AsDictionary.TryGetValue(property.JsonName, out propertyData)) {
                    object deserializedValue = null;

                    var itemResult = Serializer.TryDeserialize(propertyData, property.StorageType, ref deserializedValue);
                    result.AddMessages(itemResult);
                    if (itemResult.Failed) continue;

                    property.Write(instance, deserializedValue);
                }
            }

            return result;
        }

        public override object CreateInstance(Data data, Type storageType) {
            MetaType metaType = Serializer.Config.MetaTypeCache.Get(storageType);
            return metaType.CreateInstance();
        }
    }
}