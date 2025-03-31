// Models/Webinar.cs
using System.ComponentModel.DataAnnotations;

public class Webinar
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Topic { get; set; }

    public string Description { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public string Time { get; set; }

    [Required]
    public string Venue { get; set; }

    public string Speaker { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Webex meeting properties

    [Required]
    public string WebexUrl { get; set; }
    [Required]
    public string WebexMeetingId { get; set; }
    [Required]
    public string WebexPasscode { get; set; }
}

// Models/Registration.cs


public class Registration
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int WebinarId { get; set; }

   // public Webinar Webinar { get; set; }

    [Required]
    public string StudentName { get; set; }

    [Required]
    public string Email { get; set; }

    public string Phone { get; set; }

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}

// Models/AdminUser.cs


public class AdminUser
{
    
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; } // In real app, store hashed password
}
public class WebinarDto
{
    public int Id { get; set; }
    public string Topic { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string Time { get; set; }
    public string Speaker { get; set; }
    public string VideoUrl { get; set; }
    public string WebexUrl { get; set; }
    public string WebexMeetingId { get; set; }
    public string WebexPasscode { get; set; }
}