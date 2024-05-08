namespace WebApplication1.Data.models
{
    public class Appoint
    {

        public string Name { get; set; }
        public string Date { get; set; }

        public Appoint(string name, string date)
        {

            this.Name = name;
            this.Date = date;

        }
    }
}
