using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using UnityEngine;

namespace VRBuilder.Core.Serialization
{
    [NewtonsoftConverter]
    public class AnimationCurveConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(AnimationCurve) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                try
                {                    
                    JObject data = JObject.Load(reader);
                    JArray keys = data["Keys"].Value<JArray>();
                    JsonReader keyReader = keys.CreateReader();
                    Keyframe[] keyframes = serializer.Deserialize<Keyframe[]>(keyReader);
                    AnimationCurve curve = new AnimationCurve(keyframes);
                    curve.preWrapMode = (WrapMode)data["PreWrapMode"].Value<int>();
                    curve.postWrapMode = (WrapMode)data["PostWrapMode"].Value<int>();

                    return curve;
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("Exception occured while trying to parse an animation curve.\n{0}", ex.Message);
                    return new AnimationCurve();
                }
            }
            Debug.LogWarning("Can't read/parse animation curve from JSON.");
            return new AnimationCurve();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            AnimationCurve curve = (AnimationCurve)value;
            JObject data = new JObject();

            data.Add("Keys", new JArray(curve.keys.Select(key => JObject.FromObject(key, serializer))));
            data.Add("PreWrapMode", (int)curve.preWrapMode);
            data.Add("PostWrapMode", (int)curve.postWrapMode);
            data.WriteTo(writer);
        }
    }
}
