﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Core.Models
{
    public class License : DomainObject
    {
        public string? GymId { get; set; }
        public string? Plan { get; set; }
        public DateTime SubscribeDate { get; set; }
        public DateTime? SubscribeEndDate { get; set;}
        public string? Token { get; set; }
    }
}
