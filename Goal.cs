using System;

namespace PersonalFianceTracker
{
    internal class Goal
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = "";

        public string Term { get; set; } = "";

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}