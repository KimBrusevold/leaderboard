﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace leaderboard.Shared
{
    public class User
    {
        public string Name { get; set; }
        public string DiscrodId { get; set; }
    }
}
