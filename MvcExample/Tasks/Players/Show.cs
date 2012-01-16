﻿using System.Linq;
using MvcExample.Models.Players;
using Simpler;
using Simpler.Data.Tasks;
using Simpler.Web.Models;

namespace MvcExample.Tasks.Players
{
    public class Show : InOutTask<PlayerKey, ShowResult<PlayerShow>>
    {
        public RunSqlAndReturn<PlayerShow> FetchPlayer { get; set; }

        public override void Execute()
        {
            FetchPlayer.ConnectionName = Config.Database;
            FetchPlayer.Sql =
                @"
                select
                    PlayerId,
                    Player.FirstName + ' ' + Player.LastName as Name,
                    Team.Mascot as Team
                from 
                    Player
                    inner join
                    Team on
                        Player.TeamId = Team.TeamId
                where
                    PlayerId = @PlayerId
                ";
            FetchPlayer.Values = Inputs;
            FetchPlayer.Execute();

            Outputs = new ShowResult<PlayerShow>
                      {
                          Model = FetchPlayer.Models.Single()
                      };
        }
    }
}