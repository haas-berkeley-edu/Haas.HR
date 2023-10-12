﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    public interface IEmployee
    {
        int ID { get; set; }

        DateTime CreateOn { get; set; }
        DateTime LastUpdatedOn { get; set; }
        DateTime DeletedOn { get; set; }
    }
}
