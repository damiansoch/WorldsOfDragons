namespace Dragons.WebApi.Models.ApiV2.Worlds
{
    public class GetWorldListRequest
    {
        /// <summary>
        /// The number of worlds to skip
        /// </summary>
        public int? Skip { get; set; }
        /// <summary>
        /// The number of worlds to retrieve
        /// </summary>
        public int? Take { get; set; }
        /// <summary>
        /// A partial search query string
        /// </summary>
        public string? Search { get; set; }
    }
}
