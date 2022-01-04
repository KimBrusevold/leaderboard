using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace leaderboard.Shared;

public readonly record struct LoginResponse
{
    public string Token { get; init; }
    public DateTime Expires { get; init; }
}

