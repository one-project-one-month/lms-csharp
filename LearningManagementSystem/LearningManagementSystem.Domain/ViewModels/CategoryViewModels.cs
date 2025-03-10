﻿namespace LearningManagementSystem.Domain.ViewModels;

public class CategoryViewModels
{
    //public Guid id { get; set; } = Guid.Empty!;

    [JsonIgnore]
    public int id { get; set; }
    public string name { get; set; } = null!;
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    [JsonIgnore]
    public bool isDeleted { get; set; } = false;
}