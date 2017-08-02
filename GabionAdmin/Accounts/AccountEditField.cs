﻿using System;
using GabionAdmin.Database;

namespace GabionAdmin.Accounts
{
    public class AccountEditField : DatabaseObject
    {
        public long Action { get; set; }
        public String FieldName { get; set; }
        public String OldValue { get; set; }
        public String NewValue { get; set; }

        public AccountEditField()
        {
            Action = 0;
            FieldName = "";
            OldValue = "";
            NewValue = "";
        }
    }
}