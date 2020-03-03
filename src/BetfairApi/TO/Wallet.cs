﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace BetfairApi.TO
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Wallet
    {
        UK,
        AUSTRALIAN
    }
}
