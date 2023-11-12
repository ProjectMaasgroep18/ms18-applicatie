﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Maasgroep.Database.Members;

namespace Maasgroep.Database.team_c.Repository.ViewModel
{
    public class MemberViewModel
    {
        
        public long? ID { get; set; }
        public string Name { get; set; }

        public MemberViewModel(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public MemberViewModel(Member dbRec)
        {
            ID = dbRec.Id;
            Name = dbRec.Name;
        }
        
        [JsonConstructor]
        public MemberViewModel() { }
        
    }
}
