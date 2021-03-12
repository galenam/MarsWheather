using GraphQL.Types;

namespace Mars
{
    public class SolDataQuery : ObjectGraphType<object>
    {
        public SolDataQuery(INasaProvider nasaProvider)
        {
            Field<MarsWheatherType>("wheather", resolve: context => nasaProvider.GetAsync());
        }
    }

    public class MarsWheatherType : ObjectGraphType<MarsWheather>
    {
        public MarsWheatherType()
        {
            Field<IntGraphType>("sol");
            Field<DateTimeGraphType>("firstUTC");
            Field<DateTimeGraphType>("lastUTC");
            Field<SeasonEnum>("season");
            Field<DataDescriptionType>("AtmosphericPressure");
        }
    }

    public class SeasonEnum : EnumerationGraphType<Season>
    { }

    public class DataDescriptionType : ObjectGraphType<DataDescription>
    {
        public DataDescriptionType()
        {
            Field(d => d.Average).Description("Average of samples over the Sol");
            Field(d => d.Maximum).Description("Maximum data sample over the sol");
            Field(d => d.Minimum).Description("Minimum data sample over the sol");
            Field(d => d.TotalCount).Description("Total number of recorded samples over the Sol");
        }
    }
}