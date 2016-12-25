﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Kilt.ExternLibs.KSerializer.Internal {
    // While the generic IEnumerable converter can handle dictionaries, we process them separately here because
    // we support a few more advanced use-cases with dictionaries, such as inline strings. Further, dictionary
    // processing in general is a bit more advanced because a few of the collection implementations are buggy.
    public class DictionaryConverter : Converter {

        public override bool CanProcess(Type type) {
            return typeof(IDictionary).IsAssignableFrom(type);
        }

        public override object CreateInstance(Data data, Type storageType) {
            return Serializer.Config.MetaTypeCache.Get(storageType).CreateInstance();
        }

        public override Result TryDeserialize(Data data, ref object instance_, Type storageType) {
            var instance = (IDictionary)instance_;
            var result = Result.Success;

            Type keyStorageType, valueStorageType;
            GetKeyValueTypes(instance.GetType(), out keyStorageType, out valueStorageType);

            if (data.IsList) {
                var list = data.AsList;
                for (int i = 0; i < list.Count; ++i) {
                    var item = list[i];

                    Data keyData, valueData;
                    if ((result += CheckType(item, DataType.Object)).Failed) return result;
                    if ((result += CheckKey(item, "Key", out keyData)).Failed) return result;
                    if ((result += CheckKey(item, "Value", out valueData)).Failed) return result;

                    object keyInstance = null, valueInstance = null;
                    if ((result += Serializer.TryDeserialize(keyData, keyStorageType, ref keyInstance)).Failed) return result;
                    if ((result += Serializer.TryDeserialize(valueData, valueStorageType, ref valueInstance)).Failed) return result;

                    AddItemToDictionary(instance, keyInstance, valueInstance);
                }
            }
            else if (data.IsDictionary) {
                foreach (var entry in data.AsDictionary) {
                    if (Serializer.IsReservedKeyword(entry.Key)) continue;

                    Data keyData = new Data(entry.Key), valueData = entry.Value;
                    object keyInstance = null, valueInstance = null;

                    if ((result += Serializer.TryDeserialize(keyData, keyStorageType, ref keyInstance)).Failed) return result;
                    if ((result += Serializer.TryDeserialize(valueData, valueStorageType, ref valueInstance)).Failed) return result;

                    AddItemToDictionary(instance, keyInstance, valueInstance);
                }
            }
            else {
                return FailExpectedType(data, DataType.Array, DataType.Object);
            }

            return result;
        }

        public override Result TrySerialize(object instance_, out Data serialized, Type storageType) {
            serialized = Data.Null;

            var result = Result.Success;

            var instance = (IDictionary)instance_;

            Type keyStorageType, valueStorageType;
            GetKeyValueTypes(instance.GetType(), out keyStorageType, out valueStorageType);

            // No other way to iterate dictionaries and still have access to the key/value info
            IDictionaryEnumerator enumerator = instance.GetEnumerator();

            bool allStringKeys = true;
            var serializedKeys = new List<Data>(instance.Count);
            var serializedValues = new List<Data>(instance.Count);
            while (enumerator.MoveNext()) {
                Data keyData, valueData;
                if ((result += Serializer.TrySerialize(keyStorageType, enumerator.Key, out keyData)).Failed) return result;
                if ((result += Serializer.TrySerialize(valueStorageType, enumerator.Value, out valueData)).Failed) return result;

                serializedKeys.Add(keyData);
                serializedValues.Add(valueData);

                allStringKeys &= keyData.IsString;
            }

            if (allStringKeys) {
                serialized = Data.CreateDictionary(Serializer.Config);
                var serializedDictionary = serialized.AsDictionary;

                for (int i = 0; i < serializedKeys.Count; ++i) {
                    Data key = serializedKeys[i];
                    Data value = serializedValues[i];
                    serializedDictionary[key.AsString] = value;
                }
            }
            else {
                serialized = Data.CreateList(serializedKeys.Count);
                var serializedList = serialized.AsList;

                for (int i = 0; i < serializedKeys.Count; ++i) {
                    Data key = serializedKeys[i];
                    Data value = serializedValues[i];

                    var container = new Dictionary<string, Data>();
                    container["Key"] = key;
                    container["Value"] = value;
                    serializedList.Add(new Data(container));
                }
            }

            return result;
        }

        private Result AddItemToDictionary(IDictionary dictionary, object key, object value) {
            // Because we're operating through the IDictionary interface by default (and not the
            // generic one), we normally send items through IDictionary.Add(object, object). This 
            // works fine in the general case, except that the add method verifies that it's
            // parameter types are proper types. However, mono is buggy and these type checks do
            // not consider null a subtype of the parameter types, and exceptions get thrown. So,
            // we have to special case adding null items via the generic functions (which do not
            // do the null check), which is slow and messy.
            //
            // An example of a collection that fails deserialization without this method is
            // `new SortedList<int, string> { { 0, null } }`. (SortedDictionary is fine because
            // it properly handles null values).
            if (key == null || value == null) {
                // Life would be much easier if we had MakeGenericType available, but we don't. So
                // we're going to find the correct generic KeyValuePair type via a bit of trickery.
                // All dictionaries extend ICollection<KeyValuePair<TKey, TValue>>, so we just
                // fetch the ICollection<> type with the proper generic arguments, and then we take
                // the KeyValuePair<> generic argument, and whola! we have our proper generic type.

                var collectionType = ReflectionUtility.GetInterface(dictionary.GetType(), typeof(ICollection<>));
                if (collectionType == null) {
                    return Result.Warn(dictionary.GetType() + " does not extend ICollection");
                }

                var keyValuePairType = collectionType.GetGenericArguments()[0];
                object keyValueInstance = Activator.CreateInstance(keyValuePairType, key, value);
                MethodInfo add = collectionType.GetFlattenedMethod("Add");
                add.Invoke(dictionary, new object[] { keyValueInstance });
                return Result.Success;
            }

            dictionary.Add(key, value);
            return Result.Success;
        }

        private static void GetKeyValueTypes(Type dictionaryType, out Type keyStorageType, out Type valueStorageType) {
            // All dictionaries extend IDictionary<TKey, TValue>, so we just fetch the generic arguments from it
            var interfaceType = ReflectionUtility.GetInterface(dictionaryType, typeof(IDictionary<,>));
            if (interfaceType != null) {
                var genericArgs = interfaceType.GetGenericArguments();
                keyStorageType = genericArgs[0];
                valueStorageType = genericArgs[1];
            }

            else {
                // Fetching IDictionary<,> failed... we have to encode full type information :(
                keyStorageType = typeof(object);
                valueStorageType = typeof(object);
            }
        }
    }
}