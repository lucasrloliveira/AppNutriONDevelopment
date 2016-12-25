﻿using System;
using System.Collections.Generic;
using System.Text;
using Kilt.ExternLibs.KSerializer.Internal;

namespace Kilt.ExternLibs.KSerializer {
    /// <summary>
    /// The direct converter is similar to a regular converter, except that it targets specifically only one type.
    /// This means that it can be used without performance impact when discovering converters. It is strongly
    /// recommended that you derive from DirectConverter{TModel}.
    /// </summary>
    /// <remarks>Due to the way that direct converters operate, inheritance is *not* supported. Direct converters
    /// will only be used with the exact ModelType object.</remarks>
    public abstract class DirectConverter : BaseConverter {
        public abstract Type ModelType { get; }
    }

    public abstract class DirectConverter<TModel> : DirectConverter {
        public override Type ModelType { get { return typeof(TModel); } }

        public sealed override Result TrySerialize(object instance, out Data serialized, Type storageType) {
            var serializedDictionary = new Dictionary<string, Data>();
            var result = DoSerialize((TModel)instance, serializedDictionary);
            serialized = new Data(serializedDictionary);
            return result;
        }

        public sealed override Result TryDeserialize(Data data, ref object instance, Type storageType) {
            var result = Result.Success;
            if ((result += CheckType(data, DataType.Object)).Failed) return result;

            var obj = (TModel)instance;
            result += DoDeserialize(data.AsDictionary, ref obj);
            instance = obj;
            return result;
        }

        protected abstract Result DoSerialize(TModel model, Dictionary<string, Data> serialized);
        protected abstract Result DoDeserialize(Dictionary<string, Data> data, ref TModel model);
    }
}
