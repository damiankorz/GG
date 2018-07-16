using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using GG.Models;

namespace GG.Controllers
{
    public class HomeController : Controller
    {
        private GGContext _dBContext;
        public HomeController(GGContext context)
        {
            _dBContext = context;
        }
        
        //Method to return id of logged user from session 
        public int? LoggedUser()
        {
            return HttpContext.Session.GetInt32("id");
        }

        //GET: /home
        [HttpGet("home")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                HomeModel data = new HomeModel
                {
                    User = _dBContext.Users.Where(u => u.ID == LoggedUser()).SingleOrDefault(),
                    HostingLobbies = _dBContext.Lobbies.Where(l => l.UserID == LoggedUser()).OrderBy(l => l.StartDate).ThenBy(l => l.StartTime).ToList(),
                    ParticipatingLobbies = _dBContext.Attendees.Where(a => a.UserID == LoggedUser()).Include(a => a.User).Include(a => a.Lobby).ToList(),
                };
                return View(data);
            }
        }

        //GET: /lobbies
        [HttpGet("lobbies")]
        public IActionResult Lobbies()
        {      
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }      
            LobbiesModel data = new LobbiesModel
            {
                PlaystationLobbies = _dBContext.Lobbies.Where(l => (l.Platform == "Playstation 4" || l.Platform == "Playstation 3") && l.UserID != LoggedUser()).OrderBy(l => l.StartDate).ThenBy(l => l.StartTime).Include(l => l.Attendees).ToList(),
                XboxLobbies = _dBContext.Lobbies.Where(l => (l.Platform == "Xbox One" || l.Platform == "Xbox 360") && l.UserID != LoggedUser()).OrderBy(l => l.StartDate).ThenBy(l => l.StartTime).Include(l => l.Attendees).ToList(),
                User = _dBContext.Users.Where(u => u.ID == LoggedUser()).SingleOrDefault(),
            };
            return View(data);
        }

        //GET: /lobbies/id
        [HttpGet("lobbies/{id}")]
        public IActionResult Lobby(int id)
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            LobbyModel data = new LobbyModel
            {
                Lobby = _dBContext.Lobbies.Where(l => l.ID == id).Include(l => l.User).Include(l => l.Attendees).ThenInclude(a => a.User).SingleOrDefault(),
                User = _dBContext.Users.Where(u => u.ID == LoggedUser()).SingleOrDefault(),
            };
            return View(data);
        }

        //POST: /lobbies/id/join
        [HttpPost("lobbies/{id}/join")]
        public IActionResult Join(LobbyModel model, int id)
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            //Pass data to model
            model.Lobby = _dBContext.Lobbies.Where(l => l.ID == id).Include(l => l.User).Include(l => l.Attendees).ThenInclude(a => a.User).SingleOrDefault();
            model.User = _dBContext.Users.Where(u => u.ID == LoggedUser()).SingleOrDefault();
            if(ModelState.IsValid)
            {
                //Check if space in lobby to join
                if(model.Lobby.NumPlayers > 0)
                {
                    Attendee newAttendee = new Attendee
                    {
                        UserID = (int)LoggedUser(),
                        Gamertag = model.Attendee.Gamertag,
                        LobbyID = id,
                    };
                    Lobby lobby = _dBContext.Lobbies.Where(l => l.ID == id).SingleOrDefault();
                    lobby.NumPlayers -= 1;
                    _dBContext.Add(newAttendee);
                    _dBContext.SaveChanges(); 
                    return RedirectToAction("Lobby");
                }   
                else
                {
                    ModelState.AddModelError("Gamertag", "Lobby is full");
                    return View("Lobby", model);
                }
            }
            return View("Lobby", model);
        }

        //GET: /lobbies/id/leave
        [HttpGet("lobbies/{id}/leave")]
        public IActionResult Leave(int id)
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            Attendee attendee = _dBContext.Attendees.Where(a => a.LobbyID == id && a.UserID == LoggedUser()).SingleOrDefault();
            Lobby lobby = _dBContext.Lobbies.Where(l => l.ID == id).SingleOrDefault();
            lobby.NumPlayers += 1;
            _dBContext.Remove(attendee);
            _dBContext.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: /lobbies/id/delete
        [HttpGet("lobbies/{id}/delete")]
        public IActionResult Delete(int id)
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            List<Attendee> attendees = _dBContext.Attendees.ToList();
            Lobby lobby = _dBContext.Lobbies.Where(l => l.ID == id).SingleOrDefault();
            attendees.RemoveAll(a => a.LobbyID == id);
            _dBContext.Remove(lobby);
            _dBContext.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: /matchmaking
        [HttpGet("matchmaking")]
        public IActionResult Matchmaking()
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        //POST: /matchmaking/new
        [HttpPost("matchmaking/new")]
        public IActionResult New(Lobby model)
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            if(ModelState.IsValid)
            {
                //Initialize new lobby and add to db 
                Lobby newLobby = new Lobby
                {
                    UserID = (int)LoggedUser(),
                    LobbyName = model.LobbyName,
                    GameTitle = model.GameTitle,
                    Platform = model.Platform,
                    NumPlayers = model.NumPlayers,
                    StartDate = model.StartDate,
                    StartTime = model.StartTime,
                    Description = model.Description
                };
                _dBContext.Add(newLobby);
                _dBContext.SaveChanges();
                return RedirectToAction ("Index");
            }
            return View("Matchmaking", model);
        }

        //GET: /logout
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "User");
        }
    }
}
