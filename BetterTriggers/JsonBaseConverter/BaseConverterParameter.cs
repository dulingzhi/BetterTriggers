﻿using BetterTriggers.Models.SaveableData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterTriggers.JsonBaseConverter
{
    public class BaseConverterParameter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new JsonConverter_BT() };

        public override bool CanConvert(System.Type objectType)
        {
            return (objectType == typeof(Parameter));
        }

        public object Create(Type objectType, JObject jObject)
        {
            if (jObject.ContainsKey("ParamType"))
            {
                var type = (int)jObject.Property("ParamType");
                switch (type)
                {
                    case 1:
                        return new Function();
                    case 2:
                        return new Constant();
                    case 3:
                        return new VariableRef();
                    case 4:
                        return new TriggerRef();
                    case 5:
                        return new Value();
                    default:
                        return new Parameter();
                }

                throw new ApplicationException(String.Format("The given parameter type {0} is not supported!", type));
            }
            else
                return new Parameter();

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            var target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }


        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}
