namespace BlogLibrary;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Article
{
    [Key]
    [Required]
    public int ArticleId { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    [ForeignKey("Contributor")]
    public string? ContributorId { get; set; }
    public User? Contributor { get; set; }

    [NotMapped] 
    public string? UserName => Contributor?.UserName;
}