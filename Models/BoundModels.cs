using System;
using System.Collections.Generic;

namespace GG.Models
{
    public class HomeModel : BaseEntity
    {
        public User User {get; set;}
        public List<Lobby> HostingLobbies {get; set;}
        public List<Attendee> ParticipatingLobbies {get; set;}
    }
    public class LobbyModel : BaseEntity
    {
        public Lobby Lobby {get; set;}
        public User User {get; set;}
        public Attendee Attendee {get; set;}
    }
    public class LobbiesModel : BaseEntity
    {
        public List<Lobby> XboxLobbies {get; set;}
        public List<Lobby> PlaystationLobbies {get; set;}
        public User User {get; set;}
    }
}