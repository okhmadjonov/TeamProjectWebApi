namespace TeamProject.Entity
{
    public class AuditLog
    {
        //public int Id { get; set; }
        //public string? UserName { get; set; }
        //public string? Action { get; set; }
        //public string? Path { get; set; }
        //public int ResponseStatusCode { get; set; }
        //public DateTime Timestamp { get; set; }
        //public DateTime ResponseTimestamp { get; set; }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; } // Can be "DataChange" or "SystemEvent"
        public string TableName { get; set; } // Optional for system events
        public string Action { get; set; } // Optional for data changes
        public string Path { get; set; } // Optional for system events
        public string OldValues { get; set; } // Optional for data changes
        public string NewValues { get; set; } // Optional for data changes
        public string AffectedColumns { get; set; } // Optional for data changes
        public string PrimaryKey { get; set; } // Optional for data changes
        public string UserName { get; set; } // Optional for system events
        public int ResponseStatusCode { get; set; } // Optional for system events
        public DateTime Timestamp { get; set; }
        public DateTime ResponseTimestamp { get; set; } // Optional for system events

    }
}
