namespace razor09_razorweb.Helpers{
    public class PagingModel{
        public int currentpage { get; set; }
        public int countpages { get; set; }

        // phát sinh url 
        public Func<int?, string> generateUrl { get; set; }
    }
}