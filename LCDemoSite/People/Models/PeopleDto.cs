﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace People.Models
{
    public class PeopleDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string Qoute { get; set; }
    }
}