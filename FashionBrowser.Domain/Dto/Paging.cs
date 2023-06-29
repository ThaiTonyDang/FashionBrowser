using System;

namespace FashionBrowser.Domain.Dto
{ 
    public class Paging
	{
		public int TotalPages { get; private set; }
		public int CurrentPage { get; private set; }

		public Paging(int currentPage, int pageSize, int totalItems)
		{
			this.CurrentPage = currentPage;
			this.TotalPages = GetTotalPages(pageSize, totalItems);
		}

		private int GetTotalPages(int pageSize, int totalItems)
		{
			var totalPages = (double)totalItems / pageSize; 
			return (int)Math.Ceiling(totalPages);
		}
	}
}