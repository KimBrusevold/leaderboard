using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using leaderboard.Shared;

namespace leaderboard.Shared.Createobjects;
public class Entry
{
    public Vehicle Vehicle { get; set; }
    public Track Track { get; set; }
    public User User { get; set; }
    public Game Game { get; set; }
    public TimeSpan Time { get; set; }
}
