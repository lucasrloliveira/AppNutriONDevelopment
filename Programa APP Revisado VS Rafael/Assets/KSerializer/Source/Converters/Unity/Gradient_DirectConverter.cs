#if !NO_UNITY
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kilt.ExternLibs.KSerializer {
    partial class ConverterRegistrar {
        public static Internal.DirectConverters.Gradient_DirectConverter Register_Gradient_DirectConverter;
    }
}

namespace Kilt.ExternLibs.KSerializer.Internal.DirectConverters {
    public class Gradient_DirectConverter : DirectConverter<Gradient> {
        protected override Result DoSerialize(Gradient model, Dictionary<string, Data> serialized) {
            var result = Result.Success;

            result += SerializeMember(serialized, "alphaKeys", model.alphaKeys);
            result += SerializeMember(serialized, "colorKeys", model.colorKeys);

            return result;
        }

        protected override Result DoDeserialize(Dictionary<string, Data> data, ref Gradient model) {
            var result = Result.Success;

            var t0 = model.alphaKeys;
            result += DeserializeMember(data, "alphaKeys", out t0);
            model.alphaKeys = t0;

            var t1 = model.colorKeys;
            result += DeserializeMember(data, "colorKeys", out t1);
            model.colorKeys = t1;

            return result;
        }

        public override object CreateInstance(Data data, Type storageType) {
            return new Gradient();
        }
    }
}
#endif