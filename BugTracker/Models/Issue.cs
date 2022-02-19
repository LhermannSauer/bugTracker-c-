﻿

namespace BugTracker.Models
{
    public class Issue
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public Status? Status { get; set; }

        public Area? Area { get; set; }

        public Priority? Priority { get; set; }

        public Project? Project { get; set; }

        public IEnumerable<Activity>? Activivies { get; set; }

        public IdentityUser? Creator { get; set; }

        public Developer? AssignedTo { get; set; }

        [Display(Name = "Date Created"), DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Date Updated"), DataType(DataType.Date)]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Date Resolved"), DataType(DataType.Date)]
        public DateTime? ResolvedDate { get; set; }

        // To implement: NotifyUsers
    }
}
