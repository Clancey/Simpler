﻿using MvcExample.Entities;
using Simpler;
using Simpler.Data.Tasks;

namespace MvcExample.Tasks.Players
{
    public class Index : OutTask<Index.Out>
    {
        public class Out
        {
            public Player[] Players { get; set; }
        }

        public RunSqlAndReturn<Player> SelectPlayers { get; set; }

        public override void Execute()
        {
            const string sql = @"
                select 
                    PlayerId,
                    Player.FirstName,
                    Player.LastName,
                    Player.TeamId,
                    Player.FirstName + ' ' + Player.LastName as FullName,
                    Team.Mascot as Team
                from 
                    Player
                    inner join
                    Team on
                        Player.TeamId = Team.TeamId
                ";

            var players = SelectPlayers
                .Set(new RunSqlAndReturn<Player>.In
                         {
                             ConnectionName = Config.DatabaseName,
                             Sql = sql
                         })
                .Get().Models;

            Output = new Out { Players = players };
        }
    }
}