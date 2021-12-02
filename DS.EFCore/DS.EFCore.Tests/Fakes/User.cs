using System;
using System.Collections.Generic;
using System.Text;

namespace DS.EFCore.Helper.Tests.Fakes
{
    internal class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
    }
}
