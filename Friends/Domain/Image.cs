﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Friends
{
    public class Image
    {
        public int ImageId { get; set; }

        public string Url { get; set; }
        
        public int TypeId { get; set; }
    }
}
