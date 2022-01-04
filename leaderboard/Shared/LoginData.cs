using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace leaderboard.Shared;
public readonly record struct LoginData
{
    public string Code { get; init; }

    public LoginData(string code)
    {
        Code = code;
    }
}

