using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GG.Models
{
    public class Attendee : BaseEntity
    {
        public int ID {get; set;}
        public int UserID {get; set;}
        public User User {get; set;}
        public int LobbyID {get; set;}
        public Lobby Lobby {get; set;}
        
        [Required]
        [Display(Name = "Gamertag")]
        public string Gamertag {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public Attendee()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}