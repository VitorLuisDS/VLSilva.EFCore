using System;

namespace VLSilva.EFCore.Extensions.Tests.Fakes
{
    internal class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
    }
}
