﻿namespace BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public AppUser? Manager { get; set; }
    }
}