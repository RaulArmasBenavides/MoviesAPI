namespace ApiMovies.Application.Dtos
{
    public class PaginatedResponseDto<T>
    {
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResponseDto(List<T> data, int totalCount, int offset, int limit)
        {
            Data = data;
            TotalCount = totalCount;
            Offset = offset;
            Limit = limit;
            TotalPages = (int)Math.Ceiling((double)totalCount / limit);
        }
    }
}
