using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GG.Models
{
    public class Lobby : BaseEntity
    {
        public int ID {get; set;}
        public int UserID {get; set;}
        public User User {get; set;}
        
        [Required]
        [Display(Name = "Lobby Name")]
        public string LobbyName {get; set;}

        [Required]
        [Display(Name = "Game Title")]
        public string GameTitle {get; set;}

        [Required]
        [Display(Name = "Platform")]
        public string Platform {get; set;}

        [Required]
        [Display(Name ="Number of Players")]
        public int NumPlayers {get; set;}

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [RestrictedDate(ErrorMessage = "Date must be today's date or a future date")]
        public DateTime StartDate {get; set;}

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime {get; set;}

        [Required]
        [Display(Name = "Description")]
        public string Description {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public List<Attendee> Attendees {get; set;}
        public Lobby()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Attendees = new List<Attendee>();
        }
    }
}