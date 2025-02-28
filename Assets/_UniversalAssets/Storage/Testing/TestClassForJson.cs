using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Testing
{
    public class TestClassForJson
    {
        [JsonProperty("str")]
        public string shlepaString;

        public TestClassForJson(string shlepaString)
        {
            this.shlepaString = shlepaString;
        }
    }
}

