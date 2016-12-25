#if !NO_UNITY
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kilt.ExternLibs.KSerializer {
    partial class ConverterRegistrar {
        public static Internal.DirectConverters.LayerMask_DirectConverter Register_LayerMask_DirectConverter;
    }
}

namespace Kilt.ExternLibs.KSerializer.Internal.DirectConverters {
    public class LayerMask_DirectConverter : DirectConverter<LayerMask> {
        protected override Result DoSerialize(LayerMask model, Dictionary<string, Data> serialized) {
            var result = Result.Success;

            result += SerializeMember(serialized, "value", model.value);

            return result;
        }

        protected override Result DoDeserialize(Dictionary<string, Data> data, ref LayerMask model) {
            var result = Result.Success;

            var t0 = model.value;
            result += DeserializeMember(data, "value", out t0);
            model.value = t0;

            return result;
        }

        public override object CreateInstance(Data data, Type storageType) {
            return new LayerMask();
        }
    }
}
#endif