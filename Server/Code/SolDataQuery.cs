using System;
using GraphQL.Types;
using Microsoft.Extensions.Logging;

namespace Mars
{
    public class SolDataQuery : ObjectGraphType<object>
    {
        ILogger<SolDataQuery> logger;
        public SolDataQuery(INasaProvider nasaProvider, ILogger<SolDataQuery> _logger)
        {
            logger = _logger;
            try
            {
                Field<ListGraphType<MarsWeatherType>>("weather", resolve: context => nasaProvider.GetAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SolDataQuery constructor");
            }
        }
    }

    public class MarsWeatherType : ObjectGraphType<MarsWeather>
    {
        public MarsWeatherType()
        {
            Field(w => w.Sol);
            Field(w => w.FirstUTC);
            Field(w => w.LastUTC);
            Field<SeasonEnum>("season", resolve: w => w.Source.MarsSeason);
            Field<DataDescriptionType>("atmosphericPressure", resolve: w => w.Source.AtmosphericPressure);
            Field<ListGraphType<StringGraphType>>("photos", resolve: w => w.Source.Photos);
            Field<ListGraphType<RoverInfoType>>("rovers", resolve: w => w.Source.Rovers);
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

    public class RoverInfoType : ObjectGraphType<RoverInfo>
    {
        public RoverInfoType()
        {
            Field(r => r.Name);
            Field(r => r.LandingDate);
            Field(r => r.LaunchDate);
            Field(r => r.Status);
        }
    }
}