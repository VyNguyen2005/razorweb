namespace razor09_razorweb.Helpers{
    // razor09_razorweb.Helpers.PagingModel
    public class PagingModel{
        public int currentpage { get; set; }
        public int countpages { get; set; }

        // phát sinh url 
        public Func<int?, string> generateUrl { get; set; }
    }
}