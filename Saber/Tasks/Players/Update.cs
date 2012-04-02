﻿using Saber.Entities;
using Simpler;
using Simpler.Data.Tasks;

namespace Saber.Tasks.Players
{
    public class Update : InTask<Update.In>
    {
        public class In
        {
            public string _method { get; set; }
            public Player Player { get; set; }
        }

        public RunSql UpdatePlayer { get; set; }

        public override void Execute()
        {
            const string sql =
                @"
                update Player
                set
                    FirstName = @FirstName,
                    LastName = @LastName
                where
                    PlayerId = @PlayerId
                ";

            UpdatePlayer
                .Set(new RunSql.In
                         {
                             ConnectionName = Config.DatabaseName,
                             Sql = sql,
                             Values = Input.Player
                         })
                .Execute();
        }
    }
}