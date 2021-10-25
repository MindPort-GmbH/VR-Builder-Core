using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace VRBuilder.Core.Serialization
{
    //[NewtonsoftConverter]
    //public class AnimationCurveConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        return typeof(AnimationCurve) == objectType;
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        AnimationCurve curve = (AnimationCurve)value;
    //        JObject data = new JObject();

    //        //data.Add("x", vec.x);
    //        //data.Add("y", vec.y);
    //        //data.Add("z", vec.z);

    //        data.WriteTo(writer);
    //    }
    //}
}
