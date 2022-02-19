﻿namespace BugTracker.Models
{
    public class Developer
    {
        public int Id { get; set; }

        public IdentityUser? User { get; set; }

        public IEnumerable<Issue>? Assignments { get; set; }

        public IEnumerable<Activity>? ActivityRecord { get; set; }
    }
}