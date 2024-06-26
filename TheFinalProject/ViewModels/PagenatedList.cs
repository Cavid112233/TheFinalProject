using System;
namespace TheFinalProject.ViewModels
{
    public class PagenatedList<T> : List<T>
    {
        public PagenatedList(List<T> queries, int pageindex, int totalPage)
        {
            PageIndex = pageindex;
            TotalPage = totalPage;

            if (totalPage >= 5)
            {
                int start = PageIndex - 1;
                int end = PageIndex + 1;

                if (start <= 0)
                {
                    end = end - (start - 1);
                    start = 1;
                }

                if (end > TotalPage)
                {
                    end = TotalPage;
                    start = totalPage - 2;
                }

                StartPage = start;
                EndPage = end;
            }
            else
            {
                StartPage = 1;
                EndPage = totalPage;
            }
            this.AddRange(queries);

        }

        public int PageIndex { get; }
        public int TotalPage { get; }
        public int StartPage { get; }
        public int EndPage { get; }
        public bool HasPrev => PageIndex > 1;
        public bool HasNext => PageIndex < TotalPage;

        public static PagenatedList<T> Create(IEnumerable<T> query, int pageIndex, int itemCount)
        {
            int totalPage = (int)Math.Ceiling((decimal)query.Count() / itemCount);
            query = query.Skip((pageIndex - 1) * itemCount).Take(itemCount);

            return new PagenatedList<T>(query.ToList(), pageIndex, totalPage);
        }
    }
}

