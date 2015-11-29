﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskVlopper.Base.Base;

namespace TaskVlopper.Base.Model
{
    public class MeetingParticipants : IBaseModel
    {
        [Key]
        [Index(IsUnique = true)]
        [Column(Order = 1)]
        public int ID { get; set; }

        public int UserID { get; set; }

        public int MeetingID { get; set; }
    }
}
