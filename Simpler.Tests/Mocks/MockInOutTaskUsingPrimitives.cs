﻿using System;

namespace Simpler.Tests.Mocks
{
    public class MockInOutJobUsingPrimitives : InOutJob<int, string>
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
