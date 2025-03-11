using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Domain.ViewModels;

public class Social_linksViewModels
{
    //public Guid id { get; set; } = Guid.Empty!;

    [JsonIgnore]
    public int id { get; set; }
    public int course_id { get; set; }
    public string? facebook { get; set; }
    public string? X { get; set; }
    public string? telegram { get; set; }
    [Length(13, 13)]
    public string? phone { get; set; }
    [EmailAddress]
    public string? email { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    [JsonIgnore]
    public bool isDeleted { get; set; } = false;

}