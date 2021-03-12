using System;
using GraphQL.Types;
using GraphQL.Utilities;

namespace Mars
{
    public class SolSchema : Schema
    {
        public SolSchema(IServiceProvider sp) : base(sp)
        {
            Query = sp.GetRequiredService<SolDataQuery>();
            Mutation = sp.GetRequiredService<SolDataMutation>();
        }
    }
}