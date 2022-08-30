namespace database.helper.Entitites
{
    public class Series : Media
    {
        public List<Season> Seasons { get; set; }

        public DateTime? LastAirYear { get; set; }
    }
}
