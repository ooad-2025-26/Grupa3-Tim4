namespace Aplikacija.Models
{
    public class PandaScoreMatch
    {
        public string? name { get; set; }
        public string? begin_at { get; set; }
        public PandaScoreVideogame? videogame { get; set; }
        public PandaScoreLeague? league { get; set; }
    }

    public class PandaScoreVideogame
    {
        public string? name { get; set; }
    }

    public class PandaScoreLeague
    {
        public string? name { get; set; }
    }
}
