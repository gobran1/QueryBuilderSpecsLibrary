namespace QueryBuilderSpecs.DTO.Pagination
{
    public class PagedList<T>
    {
        public int TotalPages { get; set; }
        public int Count { get; set; }

        public List<T> Items { get; set; }

        public PagedList(List<T> items, int count, int pageSize)
        {
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            Count = count;
            Items = items;
        }
    }
}